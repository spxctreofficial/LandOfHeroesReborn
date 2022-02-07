using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Deck {
	public DeckPool genericBasicCards, genericSpecialCards, genericWeapons;
	public DeckPool uniqueBasicCards, uniqueSpecialCards, uniqueWeapons;

	public Deck(Deck deck) {
		genericBasicCards = deck.genericBasicCards;
		genericSpecialCards = deck.genericSpecialCards;
		genericWeapons = deck.genericWeapons;
		uniqueBasicCards = deck.uniqueBasicCards;
		uniqueSpecialCards = deck.uniqueSpecialCards;
		uniqueWeapons = deck.uniqueWeapons;
    }

	public DeckPool[] DeckPools {
		get {
			return new DeckPool[] {
				genericBasicCards,
				genericSpecialCards,
				genericWeapons,
				uniqueBasicCards,
				uniqueSpecialCards,
				uniqueWeapons
			};
        }
    }

	public CardData Retrieve(ChampionController championController) {
		CardData retrievedCardData = null;
		if (championController.equippedWeapon is null && championController.hand.GetWeaponCards().Length == 0 && Random.Range(0f, 1f) < 0.2f) {
			retrievedCardData = genericWeapons.Retrieve().cardData;
		}
		else if (Random.Range(0f, 1f) < 2f / 3f) {
			retrievedCardData = genericBasicCards.Retrieve().cardData;
		}
		else {
			retrievedCardData = genericSpecialCards.Combine(genericWeapons).Retrieve().cardData;
		}

		if (retrievedCardData is null) Debug.LogError("Could not retrieve a card! Is the deck empty?");
		return retrievedCardData;
	}

	public Deck Indoctrinate(Deck indoctrinatedDeck) {
		Deck newDeck = new Deck(this);

		for (int index = 0; index < DeckPools.Length; index++) {
			DeckPool newDeckPool = newDeck.DeckPools[index];
			DeckPool indoctrinatedDeckPool = indoctrinatedDeck.DeckPools[index];

			foreach (DeckEntry indoctrinatedEntry in indoctrinatedDeckPool.entries) {
				if (indoctrinatedEntry.replaceMatchID is { } || indoctrinatedEntry.replaceMatchID != string.Empty) {
					for (int entryIndex = 0; entryIndex < newDeckPool.entries.Length; entryIndex++) {
						DeckEntry replacedEntry = newDeckPool.entries[entryIndex];
						if (replacedEntry.entryID != indoctrinatedEntry.replaceMatchID) continue;
						newDeckPool.entries[entryIndex] = new DeckEntry(indoctrinatedEntry);
                    }
				}
				else {
					newDeckPool.entries.ToList<DeckEntry>().Add(new DeckEntry(indoctrinatedEntry));
                }
            }
        }

		return newDeck;
    }

	[System.Serializable]
	public class DeckPool {
		public DeckEntry[] entries;

		public DeckPool(DeckPool deckPool) {
			entries = deckPool.entries;
        }

		public DeckEntry Retrieve() {
			List<DeckEntry> pool = new List<DeckEntry>();

			if (entries is null || entries.Length <= 0) {
				Debug.LogError("The entry was null!");
				return null;
			}
			foreach (DeckEntry cardRegEntry in entries) {
				for (int i = 0; i < cardRegEntry.weight; i++) {
					pool.Add(cardRegEntry);
				}
			}

			return pool[Random.Range(0, pool.Count)];
		}
		public DeckPool Combine(DeckPool pool) {
			DeckPool newPool = new DeckPool(this);
			newPool.entries.Concat(pool.entries);
			return newPool;
		}

	}

	[System.Serializable]
	public class DeckEntry {
		public DeckEntry(DeckEntry deckEntry) {
			cardData = deckEntry.cardData;
			weight = deckEntry.weight;

			entryID = deckEntry.entryID;
			replaceMatchID = deckEntry.replaceMatchID;
        }

		public CardData cardData;
		[Range(1, short.MaxValue)]
		public int weight;

		public string entryID, replaceMatchID;

		public bool Match(DeckEntry deckEntry) {
			if (deckEntry.cardData == cardData && deckEntry.entryID == entryID) return true;
			return false;
        }
	}

	[System.Serializable]
	public class DefaultDecks {
		public Deck defaultDeck;

		public Deck warriorDeck, berserkerDeck, mageDeck, warlockDeck, priestDeck, rogueDeck, roninDeck;
    }
}