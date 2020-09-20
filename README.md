# Blackjack

## Features/Rules Implemented:
1. Dealer stands at 17

2. Ace counts as 1 or 11 (best hand value is chosen automatically)

3. Betting - with customizable Minimum Bet and Maximum Bet

4. Customizable Deck Count

5. Customizable Starting Money

6. Lifetime Stats:
  - Hands Won
  - Hands Lost
  - Hands Tied
  - Money Won
  - Money Lost
  - Blackjacks
  
7. Stats can be reset

8. Continue game until money runs out

9. Tweening animations for Card Flip and Deal

10. Single player vs Dealer


## Code Architecture

### GameController (class)
GameController is a class derived from MonoBehavior and is a singleton. It handles the initialization, game state management, save/load, references, core UI components and coordination between different modules in the code architeture.

### Deck (class)
Deck class is responsible for creating card deck of any given size.
-DrawCard() method returns a card that can be dealt to any Participant.
-ResetDeck() methods inserts all drawn cards back into the deck at random indices. 

### Card (class)
Card class contains CardData for each card and handles its behaviour and animations.

### Participant (class)
Base class for all participants in the game. Game currently features two 'Participants' - Player (class) and Dealer (class). Handles game logic for when a card is dealt to this Participant and notifies GameController of the outcomes.

### AssetController (class)
Refers to the card sprite sheet and returns appropriate sprite for requested Card value and suit.

### GameSettings (class)
- Handles UI and input in the Game settings panels. 
- Holds the parameters for starting a customized game
- Uses SettingsData struct which can be serialized, saved and loaded to and from PlayerPrefs.

### GameStats (class)
- Handles UI and input in the Lifetime Stats panel
- Tracks and updates stats from the gameplay
- Saves/Loads StatsData struct to and from PlayerPrefs

The game uses JsonUtility to serialize objects into JSON and deserialize JSON into objects from PlayerPrefs.

## Developer Notes

**Time spent (approx)**
- Animations: < 1 hour
- Core Gameplay: 3-4 hours
- UI/Visuals: 1-2 hours
- Bonus Features: 1-2 hours 
