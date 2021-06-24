# Land of Heroes Official Changelog

This is the official Land of Heroes Changelog. Any changes to the game will be documented here for ease-of-use and convenient reference, as well as allowing clients and players to understand the key differences between updates for those who do not understand C# or code in general.

## v0.1.1 Beta

v0.1.1 is a minor beta release that addresses some bugs and issues that arose when play-testing v0.1, specifically with play-testing lower-difficulty Sandbox matches. No major additions have been added.

- Bug Fixes:
	- In a scenario where the player clicks a discarded card (happens more often than you think), the game would producce a NullReferenceException. This has been fixed.
		- If the player starts an attack, and then clicks a discarded card, they will select that discarded card as their "combat card". This has also been fixed.
	- In a rare scenario where a bot is looking for a target, they would target a nemesis (even if they were dead.) This loophole bug has been fixed.
- Miscellaneous Notes:
	- Moved "Main Canvas" of the Main Menu to World Space, fixing the camera shake that was quite weird previously. (Although the values of the "focus shake" was slightly altered too.)
	- Added a "CLICK TO START" prompt in the Main Menu, in case anyone was confused about the blur.
	- Various error tooltips have been added along with the bug fixes addressed in this minor update.

## v0.1 Beta

This is the first official (beta) release of Land of Heroes! Since this is the first official version, I will document all of the notable additions and point-of-interest mechanics in the changelog. Keep in mind that a lot of things have already been added, and I may have missed a thing or two.

- Current scenes: Main Menu and Sandbox Scene.
- Main Menu:
	- Buttons to quit or play a match of Sandbox.
	- Button to enter the Shop.
	- Currently, you can purchase champions for the shop (a default is given if you have insufficient funds.) This will be changed and improved in the future as I progress and add new content into the game.
	- A confirmation to purchase will be prompted, so accidental clicks will not result in accidental purchases and waste of money.
	- You cannot purchase duplicate items.<br>
	- Upon start-up, the Main Menu will be unfocused. A shake-effect on the camera and the logo will be applied and the menu will be focused on a click (for dramatic effect.)
- Sandbox:
	- Sandbox mode currently has two *offline* gamemodes: Competitive 2v2 and FFA (Free-For-All).
		- In Competitive 2v2, you fight against two other opponents with an ally.
		- In FFA, you fight alone against  two other opponents (however, the two opponents are not on the same team either, hence the name "FFA".)
	- Additionally, Sandbox mode currently has four difficulty levels:
		- Noob: Noob.
		- Novice: A beginner-friendly difficulty that allows the player to get away with grave mistakes, and generally allow any inexperienced player to casually win.
		- Warrior: This difficulty amps the game up. The bots are semi-experienced with the mechanics of the game, and a semi-experienced player should require some logic in order to casually win.
		- Champion: OHHHHHH YEAHHHHHHHH! The way the game was meant to be played. The bots have advanced coherence, can make important decisions that may save their own (or an ally's) life, and understand the proper way to use a card to it's maximum potential. An experienced player must use skill, advanced logic, and knowledge of their own champion in order to achieve victorious. Bonus loot and gold awarded from this difficulty.
	- At the beginning of the game, you are dealt 4 cards. At the beginning of your turn, you are dealt 2 cards.
		- Currently, the player will always be the player with the first turn. This is to change in the near future.
	- During a turn, there are three phases:
		- Beginning Phase: The phase where you draw 2 cards (and check for any abilities automatically activated during the Beginning Phase.)
		- Action Phase: The phase you play your cards and activate certain abilities (and check for any abilities automatically activated during the Action Phase.)
		- End Phase: The phase where you discard additional cards (and check for any abilities automatically activated during the End Phase.)
			- Since you're only allowed to have a maximum of 6 cards, you will be forced to discard until you have 6 cards in your hand during the End Phase.
			- This doesn't matter however if it's not your End Phase..., yet. If you have more than 6 cards, you can keep them until the End Phase. This rule is only enforced *during **your** End Phase.*
			- Discarded cards will be labeled "DISCARDED".
		- After these three phases, the game will check for the next player's turn.
		- Glow effects will indicate which player the current turn belongs to.
	- Every card will have a slightly/vastly different function.
		- Spades: Initializes an attack when played normally.
			- Upon initializing an attack, the player will be prompted to select a target.
				- The target will glow red when they are selected.
			- Additionally, the player will also be prompted to select a "combat card."
				- The "combat card" will glow white when it is selected.
				- After the prerequisites are selected and the attack is confirmed, the defender will be prompted to select a "combat card" as well.
				- Alternatively, you can "gamble" a "combat card", which will randomly generate a card for you as your "combat card". The downside of this is that you have no idea what that card is until after the attack is finished (as the "combat card" will be flipped), and it is extremely easy to lose an attack using this method.
			- The "combat cards" will be compared, and the larger of the two will deal damage to the other player. If the "combat cards" have the same value (tie), no damage will be dealt.
				- Combat Advantage is the modifier to a "combat card's" value. A positive Combat Advantage will give the "combat card" a positive advantage during combat. A value of 13 with a +1 Combat Advantage will have a value of 14 during combat. This is also true for negative Combat Advantages.
				- The camera and the champion's avatar will shake when that champion takes damage.
				- Blood will splatter from champions when they take damage. The more damage taken, the more blood splatter.
		- Hearts: Heal for a certain amount when played normally.
			- Bonus: Healing (by any means) has an unique sound effect.
		- Clubs: Trades for another card when played normally.
		- Diamonds: Miscellaneous actions when played normally.
	- The player can choose their own champion. Every champion has a different skill-set, list of abilities, physical attributes, and avatar. Players can also purchase new champions from the shop in the Main Menu, and every champion has a different cost (although it is quite balanced and can typically be grouped into particular price groups.) In the near future, some champions may have additional voice lines (given I have the resources to record these voice lines.)
		- There are currently seven playable champions in the game; more will be added down the line:
			- Arya (The Paladin): Large health pool allows for tanking damage, and punishes other players when healing.
			- Ambush Trooper: **Stealth** allows for the Ambush Trooper to avoid being targeted for attacks if the Ambush Trooper has not played a card in the last turn.
			- Regime Soldier: The default champion. Average health and damage with no special abilities.
			- Regime Captain: An upgraded Regime Soldier. Can be dealt additional cards for the amount of champions with the same faction.
			- Apprentice: **Bojutsu** gives attackers -1 Combat Advantage when using "combat cards" with large values against the Apprentice. **Quick Heal** also allows for a chance to rapidly heal a significant amount.
			- Castlefel Rebel: **Strategic Maneuver** allows the Castlefel Rebel to draw back cards when using high-value "combat cards" to defend. Not to mention, Castlefel Rebel is also one of the cheapest champions currently in the game.
			- Hoplomachus: **Hoplite Shield** gives the Hoplomachus a chance to cut incoming melee/ranged damage in half.
		- Bots randomly choose their champion. The higher the difficulty, the smarter they will be when choosing their champion.
		- Certain abilities that are not automatically activated may ask for your permission prior to activating. This will show up via a confirmation prompt near the bottom-thirds of your screen.
		- When abilities are activated, they will pop up as a small, red feed on their respective champion's avatar. Currently, the process of doing the same but with a persistent state the champion is supposedly in is operational, but isn't implemented just yet.
		- Champions will start to drip blood after reaching a certain health percentage. Furthermore, champions will start to bleed after reaching a critical health percentage.
		- Bonus: All champions are hand-drawn by the creator. Some of the drawings may be terrible; others may be fantastic. Hopefully in the future we could colorize or remake the sprites of the champion avatars, but this will have to do for now.
	- Hovering over certain objects will provide a more detailed, verbose, and accurate description of the particular object that the player is hovering over. This is done via a tooltip system. For example, if the player hovers over a champion in-game, it will show their current health, attack damage, amount of cards in their hand, and abilities.
		- When the game prevents the player from doing a particular action, an error tooltip will appear. This tooltip, unlike regular tooltips, do not update their position to the mouse in real-time, and will fade away after a certain duration.
	- The player will be awarded in-game currency (gold) when they win or lose a match of Sandbox. The amount rewarded will be dependent on the difficulty of the match. Gold can be used to purchase champions and other items (coming soon) in the shop on the Main Menu.
		- Owned gold and owned champions are serialized and stored in JSON. The file format that it saves to is a custom file extension: `.lohsave` <br>
		This file extension can be modified with a text editor, and is saved under a `Saves` folder under the application's path.
- Compatibility:
	- The project currently supports Windows, MacOS, Linux, iOS (additional testing needed), and Android. You can build the project yourself using a compatible version of Unity to an operating system of your choice.
	- As of writing, the current version of Unity the project supports is `Unity 2020.3.11f1`.
	- As of writing, the current IDEs the project supports is JetBrains Rider, Visual Studio 2019 (recommended), and Visual Studio Code.
- Miscellaneous Notes:
	- Champions are created by creating new Champion ScriptableObjects. These ScriptableObjects are then loaded onto a template to be used.
	- Cards are created by creating new Card ScriptableObjects. These ScriptableObjects are then loaded onto a template whenever cards are dealt, ready for use.