using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class AbilityController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public ChampionController champion;
	public Ability ability;

	public Dictionary<string, int> abilityInts = new Dictionary<string, int>();
	public Dictionary<string, float> abilityFloats = new Dictionary<string, float>();
	[HideInInspector]
	public List<string> tags = new List<string>();

	private static LTDescr delay;

	// Constructors
	public void Setup(ChampionController champion, Ability ability) {
		// Setting up variables
		this.champion = champion;
		this.ability = ability;

		// Setting up appearance
		gameObject.GetComponent<Image>().sprite = ability.sprite;
		switch (ability.abilityType) {
			case Ability.AbilityType.Passive:
			case Ability.AbilityType.AttackB:
			case Ability.AbilityType.DefenseB:
			case Ability.AbilityType.Ultimate:
				gameObject.GetComponent<Button>().interactable = false;
				break;
			default:
				gameObject.AddComponent<SmartHover>();
				break;
		}
	}

	// Click Event
	public void OnClick() {}

	// Checks
	public bool CheckForAbility(string searchCriteria) {
		if (champion.abilities.Count == 0) return false;
		foreach (var ability in champion.abilities) {
			if (!IsExclusive()) return false;
			if (searchCriteria == ability.abilityID) return true;
		}
		return false;
	}
	public bool IsExclusive() {
		if (ability.isExclusiveTo.Count == 0) return true;
		foreach (var champion in ability.isExclusiveTo) {
			if (champion == this.champion.champion) return true;
		}
		return false;
	}

	// Triggers
	/// <summary>
	/// Checks for abilities on start of Beginning Phase.
	/// </summary>
	/// <returns></returns>
	public IEnumerator OnBeginningPhase() {
		switch (ability.abilityID) {
			case "Ability_QuickAssist":
				yield return StartCoroutine(QuickAssist());
				break;
		}
	}
	/// <summary>
	/// Checks for abilities on start of Action Phase.
	/// </summary>
	/// <returns></returns>
	public IEnumerator OnActionPhase() {
		yield break;
	}
	/// <summary>
	/// Checks for abilities on start of End Phase.
	/// </summary>
	/// <returns></returns>
	public IEnumerator OnEndPhase() {
		switch (ability.abilityID) {
			case "Ability_Rejuvenation":
				yield return StartCoroutine(Rejuvenation());
				break;
		}
	}
	/// <summary>
	/// Checks for abilities when calculating the next turn.
	/// </summary>
	/// <returns></returns>
	public IEnumerator OnNextTurnCalculate() {
		yield break;
	}

	/// <summary>
	/// Checks for abilities when `dealtTo` is dealt a card.
	/// </summary>
	/// <param name="card"></param>
	/// <param name="dealtTo"></param>
	/// <returns></returns>
	public IEnumerator OnDeal(Card card, ChampionController dealtTo) {
		switch (ability.abilityID) {
			case "Ability_HopliteTradition":
				yield return StartCoroutine(HopliteTradition(card, dealtTo));
				break;
		}
	}
	/// <summary>
	/// Checks for abilities when this ability's champion is damaged.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns></returns>
	public IEnumerator OnDamage(int amount) {
		yield break;
	}
	/// <summary>
	/// Checks for ability when `damagedChampion` is damaged.
	/// </summary>
	/// <param name="damagedChampion"></param>
	/// <param name="amount"></param>
	/// <returns></returns>
	public IEnumerator OnDamage(ChampionController damagedChampion, int amount) {
		yield break;
	}
	/// <summary>
	/// Checks for abilities, then returns a bonus that is added on to incoming damage.
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="damageType"></param>
	/// <returns></returns>
	public int DamageCalculationBonus(int amount, DamageType damageType) {
		switch (ability.abilityID) {
			case "Ability_HopliteShield":
				return HopliteShield(amount, damageType);
			default:
				return 0;
		}

	}
	/// <summary>
	/// Checks for abilities when this ability's champion is healed.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns></returns>
	public IEnumerator OnHeal(int amount) {
		switch (ability.abilityID) {
			case "Ability_Smite":
				yield return StartCoroutine(Smite(amount));
				break;
			case "Ability_QuickHeal":
				yield return StartCoroutine(QuickHeal(amount));
				break;
		}
	}
	/// <summary>
	/// Checks for abilities when `healedChampion` is healed.
	/// </summary>
	/// <param name="healedChampion"></param>
	/// <param name="amount"></param>
	/// <returns></returns>
	public IEnumerator OnHeal(ChampionController healedChampion, int amount) {
		yield break;
	}
	/// <summary>
	/// Checks for abilities when this ability right before this ability's champion dies.
	/// </summary>
	/// <returns></returns>
	public IEnumerator OnDeath() {
		yield break;
	}

	/// <summary>
	/// Checks for abilities when this ability's champion calculates the result of a combat as the attacker.
	/// </summary>
	/// <param name="attackingCard"></param>
	/// <param name="defendingCard"></param>
	/// <returns></returns>
	public IEnumerator OnCombatCalculationAttacker(Card attackingCard, Card defendingCard) {
		yield break;
	}
	/// <summary>
	/// Checks for abilities when this ability's champion calculates the result of a combat as the defender.
	/// </summary>
	/// <param name="attackingCard"></param>
	/// <param name="defendingCard"></param>
	/// <returns></returns>
	public IEnumerator OnCombatCalculationDefender(Card attackingCard, Card defendingCard) {
		switch (ability.abilityID) {
			case "Ability_Bojutsu":
				yield return StartCoroutine(Bojutsu(attackingCard));
				break;
			case "Ability_StrategicManeuver":
				yield return StartCoroutine(StrategicManeuver(defendingCard));
				break;
		}
	}
	public bool CanBeTargetedByAttack() {
		switch (ability.abilityID) {
			case "Ability_Stealth":
				return Stealth();
		}
		return true;
	}

	// Ability Methods
	private IEnumerator QuickAssist() {
		if (!champion.isMyTurn) yield break;

		yield return new WaitForSeconds(1f);

		foreach (var selectedChampion in GameController.instance.champions) {
			if (selectedChampion == champion || selectedChampion.isDead || selectedChampion.faction != champion.faction || selectedChampion.faction == Champion.Faction.Undefined) continue;

			yield return StartCoroutine(champion.hand.Deal(1));
			Debug.Log(ability.abilityName + " was activated for " + champion.championName + ". Dealing that champion a card!");
			champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
		}
	}
	private IEnumerator QuickHeal(int amount) {
		Debug.Log(ability.abilityName + " activated for " + champion.championName + ", 50% chance to heal for double the amount!");
		if (Random.Range(0f, 1f) < 0.5f || champion.currentHP == champion.maxHP) yield break;

		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(champion.Heal(amount, false));
		champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
	}
	private bool Stealth() {
		Debug.Log("being run");
		Debug.Log(IsExclusive());
		if (!IsExclusive()) return true;
		if (champion.spadesBeforeExhaustion == 1 && champion.heartsBeforeExhaustion == 3 && champion.diamondsBeforeExhaustion == 1) {
			AudioController.instance.Play(ability.customAudioClips[0], false, 0.5f);
			Debug.Log("oh look it worked");
			champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
			return false;
		}
		return true;
	}
	private IEnumerator HopliteTradition(Card card, ChampionController dealtTo) {
		if (!IsExclusive()) yield break;
		if (dealtTo == champion || card.cardScriptableObject.cardValue <= 10) yield break;

		Debug.Log(ability.abilityName + " was activated for " + champion.championName + " because " + dealtTo.championName + " was dealt a card with a value higher than 10. A 50% chance to heal for 20!");

		if (Random.Range(0f, 1f) < 0.5f && champion.currentHP != champion.maxHP) {
			Debug.Log("Check succeeded! Healing " + champion.championName + " for 20.");
			yield return StartCoroutine(champion.Heal(20, false));
			champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
			yield break;
		}
		Debug.Log("Check failed! Nothing happens.");
	}
	private int HopliteShield(int amount, DamageType damageType) {
		if (!IsExclusive()) return 0;
		switch (damageType) {
			case DamageType.Melee:
			case DamageType.Ranged:
				break;
			default:
				return 0;
		}

		Debug.Log(ability.abilityName + " was activated for " + champion.championName + ". A 20% chance to negate the damage by half!" +
		          "\n This chance is increased to 50% if the damage is fatal.");

		var chance = champion.currentHP - amount <= 0 ? 0.5f : 0.2f;
		if (Random.Range(0f, 1f) < chance) {
			AudioController.instance.Play(ability.customAudioClips[0]);
			champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
			return -amount / 2;
		}
		return 0;
	}
	private IEnumerator Bojutsu(Card attackingCard) {
		if (!IsExclusive()) yield break;
		if (attackingCard.cardScriptableObject.cardValue < 10) yield break;

		attackingCard.cardScriptableObject.cardValue--;
		Debug.Log(ability.abilityName + " was activated for " + champion.championName + " because another champion attacked with a J or higher. " + " That card's value is reduced by 1. It is now " + attackingCard.cardScriptableObject.cardValue);
		champion.abilityFeed.NewAbilityFeedEntry(ability, champion);
	}
	private IEnumerator Rejuvenation() {
		if (!IsExclusive()) yield break;
		if (Random.Range(0f, 1f) < 0.5f || champion.currentHP == champion.maxHP) yield break;

		Debug.Log(ability.abilityName + " was activated for " + champion.championName + ". Healing for 5.");
		yield return StartCoroutine(champion.Heal(5, false));
		champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
	}
	private IEnumerator Smite(int amount) {
		if (!IsExclusive()) yield break;
		
		Debug.Log(ability.abilityName + " was activated for " + champion.championName + ".");
		
		var markedForSmite = GameController.instance.champions[Random.Range(0, GameController.instance.champions.Count)];
		var tries = 0;
		while ((markedForSmite.isDead || markedForSmite == champion || markedForSmite.teamMembers.Contains(champion)) && tries <= 15) {
			markedForSmite = GameController.instance.champions[Random.Range(0, GameController.instance.champions.Count)];
			tries++;
		}
		yield return StartCoroutine(markedForSmite.Damage(amount, DamageType.Lightning, champion, true));
		AudioController.instance.Play(ability.customAudioClips[0], false, 0.5f);
		champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
	}
	private IEnumerator StrategicManeuver(Card defendingCard) {
		if (!IsExclusive()) yield break;
		if (defendingCard.cardScriptableObject.cardValue < 10) yield break;

		Debug.Log(ability.abilityName + " was activated for " + champion.championName + ".");

		yield return StartCoroutine(champion.hand.Deal(1));
		champion.abilityFeed.NewAbilityFeedEntry(ability, champion, 2f);
	}






	// Image Shake Borrowed From ChampionController
	public void OnPointerEnter(PointerEventData eventData) {
		delay = LeanTween.delayedCall(0.5f, () => {
			var body = ability.abilityDescription;

			string abilityType() {
				return ability.abilityType switch {
					Ability.AbilityType.Passive => "Passive",
					Ability.AbilityType.Active => "Active",
					Ability.AbilityType.AttackB => "Attack Bonus",
					Ability.AbilityType.DefenseB => "Defense Bonus",
					Ability.AbilityType.Ultimate => "Ultimate",
					_ => throw new ArgumentOutOfRangeException()
				};
			}

			body += "\n Ability Type: " + abilityType();

			TooltipSystem.instance.Show(body, ability.abilityName);
		});
	}
	public void OnPointerExit(PointerEventData eventData) {
		LeanTween.cancel(delay.uniqueId);
		TooltipSystem.instance.Hide(TooltipSystem.TooltipType.Tooltip);
	}
}