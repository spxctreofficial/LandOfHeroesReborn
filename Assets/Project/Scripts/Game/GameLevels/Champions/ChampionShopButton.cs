using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class ChampionShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[HideInInspector]
	public Champion champion;

	[SerializeField]
	private Image avatar;
	[SerializeField]
	private TMP_Text goldCostText;

	private static int delay;

	private void Start() {
		UpdateInformation();
	}

	public void OnClick() {
		StartPurchase();
	}

	private void StartPurchase() {
		if (DataManager.instance.GoldAmount - int.Parse(goldCostText.text) < 0) return;

		string description = "Are you sure you want to purchase " + champion.championName + " for " + goldCostText.text + " gold? This purchase is irreversible, and is therefore a permanent purchase.";
		var confirmDialog = ConfirmDialog.CreateNew("Purchase", description, () => {
			ConfirmDialog.instance.Hide();
		}, () => {
			ConfirmDialog.instance.Hide();

			Debug.Log("PURCHASE SUCCESSFUL!");
			DataManager.instance.GoldAmount -= champion.shopCost;
			DataManager.instance.OwnedChampions.Add(champion);
			AudioController.instance.Play("CoinToss0" + Random.Range(1, 3));
			UpdateInformation();
		});
		confirmDialog.transform.SetParent(MainMenuController.instance.shopPanel.transform, false);
	}
	
	
	private void UpdateInformation() {
		// Updates information
		avatar.sprite = champion.avatar;
		goldCostText.text = champion.shopCost.ToString();

		foreach (var champion in DataManager.instance.OwnedChampions) {
			if (champion != this.champion) continue;
			GetComponent<Button>().enabled = false;
			goldCostText.text = "PURCHASED";
			goldCostText.color = new Color32(128, 128, 128, 255);
			break;
		}
	}

	// Pointer Events
	public void OnPointerEnter(PointerEventData eventData) {
		delay = LeanTween.delayedCall(0.5f, () => {
			string attackType() {
				return champion.attackDamageType switch {
					DamageType.Melee => "Melee",
					DamageType.Ranged => "Ranged",
					DamageType.Fire => "Fire",
					DamageType.Lightning => "Lightning",
					DamageType.Shadow => "Shadow",
					DamageType.Unblockable => "Unblockable",
					_ => throw new ArgumentOutOfRangeException()
				};
			}
			string abilityType(Ability ability) {
				return ability.abilityType switch {
					Ability.AbilityType.Passive => "Passive",
					Ability.AbilityType.Active => "Active",
					Ability.AbilityType.AttackB => "Attack Bonus",
					Ability.AbilityType.DefenseB => "Defense Bonus",
					Ability.AbilityType.Ultimate => "Ultimate",
					_ => throw new ArgumentOutOfRangeException()
				};
			}

			var body = "Health: " + champion.maxHP; // max health
			body += "\n" + champion.attackName + " (Attack): " + champion.attackDamage + " " + attackType() + " Damage"; // attack & damage

			body += "\nAbilities:"; // abilities
			foreach (var ability in champion.abilities) body += "\n" + ability.abilityName + " (" + abilityType(ability) + ")"; // print all abilities

			body += "\n\nCost: " + champion.shopCost; // cost to buy (assumes that the champion is a shop item)

			TooltipSystem.instance.Show(body, champion.championName); // show the tooltip
		}).id;
	}
	public void OnPointerExit(PointerEventData eventData) {
		LeanTween.cancel(delay);
		TooltipSystem.instance.Hide(TooltipSystem.TooltipType.Tooltip);
	}
}