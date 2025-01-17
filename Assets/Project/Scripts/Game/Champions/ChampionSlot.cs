﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChampionSlot : MonoBehaviour
{
	private ChampionController occupiedChampion;

	// Occupant Methods
	/// <summary>
	/// Get the current occupant of this slot.
	/// </summary>
	/// <returns></returns>
	public ChampionController CurrentOccupant()
	{
		return occupiedChampion;
	}
	/// <summary>
	/// Sets `champion` to the occupant of this slot.
	/// </summary>
	/// <param name="champion"></param>
	public void SetOccupant(ChampionController champion)
	{
		if (IsOccupied())
		{
			switch (CurrentOccupant().teamMembers.Contains(champion))
			{
				case true:
					FindNextVacantSlot("Ally").SetOccupant(CurrentOccupant());
					break;
				case false:
					FindNextVacantSlot("Opponent").SetOccupant(CurrentOccupant());
					break;
			}
		}

		champion.transform.localPosition = GetComponent<RectTransform>().localPosition;
		occupiedChampion = champion;
		occupiedChampion.slot = this;
	}
	/// <summary>
	/// Clear the occupant of this slot.
	/// </summary>
	public void ClearOccupant()
	{
		occupiedChampion.slot = null;
		occupiedChampion = null;
	}
	/// <summary>
	/// Checks if this slot is currently occupied.
	/// </summary>
	/// <returns></returns>
	public bool IsOccupied()
	{
		return CurrentOccupant() switch
		{
			null => false,
			_ => true
		};
	}
	/// <summary>
	/// Finds and returns the next vacant slot, given that a GameController exists within the scene.
	///
	/// Possible filters: "Normal" (default), "Ally", "Opponent"
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public static ChampionSlot FindNextVacantSlot(string filter = "Normal")
	{
		foreach (ChampionSlot championSlot in GameManager.instance.slots)
		{
			if (championSlot.occupiedChampion != null) continue;

			Vector2 position = championSlot.GetComponent<RectTransform>().localPosition;
			switch (filter)
			{
				case "Ally":
					if (position != defaultLocations[1]
					    || position == defaultLocations[2]
					    || position == defaultLocations[3]
					    || position == defaultLocations[4]
					    || position == defaultLocations[7]) continue;
					break;
				case "Opponent":
					if (position == defaultLocations[1]
					    || position != defaultLocations[2]
					    || position != defaultLocations[3]
					    || position != defaultLocations[4]
					    || position != defaultLocations[7]) continue;
					break;
			}

			return championSlot;
		}

		Debug.LogError("No vacant slot was found!");
		return null;
	}

	public IEnumerator ShakeOccupant(float duration, float magnitude)
	{
		RectTransform occupantRectTransform = occupiedChampion.GetComponent<RectTransform>();
		Vector3 originalPos = GetComponent<RectTransform>().localPosition;
		for (float t = 0; t < 1; t += Time.deltaTime / duration)
		{
			float x = Random.Range(originalPos.x - 1f * magnitude, originalPos.x + 1f * magnitude);
			float y = Random.Range(originalPos.y - 1f * magnitude, originalPos.y + 1f * magnitude);
			Vector3 shake = new Vector3(x, y, originalPos.z);
			occupantRectTransform.localPosition = shake;
			yield return null;
		}
		occupantRectTransform.localPosition = originalPos;
	}

	// Static Methods

	/// <summary>
	/// Creates all the default slots based on the default locations, given that a GameController exists within the scene.
	/// </summary>
	public static void CreateDefaultSlots()
	{
		foreach (Vector3 vector3 in defaultLocations)
		{
			ChampionSlot slot = Instantiate(PrefabManager.instance.championSlotPrefab, vector3, Quaternion.identity).GetComponent<ChampionSlot>();
			GameManager.instance.slots.Add(slot);
			slot.transform.SetParent(GameManager.instance.gameArea.transform, false);
		}
	}

	/// <summary>
	/// Default locations for common & verified slots.
	/// </summary>
	public static readonly List<Vector2> defaultLocations = new List<Vector2>
	{
		new Vector2(-856, -85), // Player Slot
		new Vector2(856, -85),  // *Usually* Ally Slot
		new Vector2(856, 335),  // Enemy Slot 1
		new Vector2(-856, 335), // Enemy Slot 2
		new Vector2(0, 376),    // Enemy Slot 3
		new Vector2(-856, 64),  // Miscellaneous Slot 1 (Usually Minion)
		new Vector2(856, 64)    // Miscellaneous Slot 2 (Usually Minion)
	};
}
