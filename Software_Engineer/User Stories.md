# Agents
- Player
- Product Owner
- User
- Database manager
- Web manager
- IA (Enemy)
- Developers

# User Stories

## Game
- As a Product Owner, I want an entertaining card game, then people can entertain themselves while thinking about a strategy.
- As a player, I want to play a strategic card game, so have fun while doing strategy.
- As a player, I want to have a high variety of cards, so I can change decks between games.
- As a player, I want the cards in my hand to rotate, then it won't become repetitive.
- As a player, I want the game to have a certain difficulty, so I have to think of a better strategy each game.
- As a player, I want to have the possibility of playing in offline mode even if the data is not saved.

## Progression
- As a player, I want to be able to unlock cards when I win games, to be able to create new strategies.
- As a player, I want the game to give me experience for winning games, then I can demonstrate my level.
- As a Web manager, I want the percentage of elixir spent in games to be saved, then I can show which cards are played the most.
- As a player, I want to have an in-game achievement system that rewards me for completing specific objectives.                                    


## User interface
- As a player, I want the game interface to be intuitive, then I will be able to navigate the game easily.
- As a player, I want the controls to be easy to operate, so I can enjoy the game.
- As a player, I want the game to have sound, so the game won't be so heavy.
- As a player, I want the game interface to include a guide that shows me the basic mechanics and strategies of the game, to learn quickly and start playing.

## Connectivity
- As a player, I want the game to be stable, then I can enjoy my time playing. 
- As a Web manager, I want to send the game statistics, so players can see possible ways to improve.

## Creation of decks and cards 
- As a player, I want to be able to create custom decks before the game, then I can change the style of the game.
- As a player, I want each card to have cost, attack and life, then it will make it fun.
- As a player, I want there to be cards that enhance other cards, so I can do more damage or take less damage.
- As a player, I want to be able to choose my cards in the menu, so I can improve my experience and strategy.

## Login and Saving
- As a user, I want to be able to secure my account / progress, then I don't worry about losing my progress or being hacked.

---

|Start game:||
|:---|---|
|Description:|The user starts a new game.|
|Actors:|User|
|Main flow:|The user selects the option to start a new game. The system creates a new game and sets the initial state of the game. Cards are distributed to the players.It is determined who starts the game (coin toss or some other method).The system displays the game board and the first turn begins.|

|Play card:||
|:---|---|
|Description:|User plays a card during his turn.|
|Actors:|User|
|Main flow:|User selects a card from his hand. The user chooses an action for that card (attack, defend, use ability, etc.). The system checks if the user has the necessary points to play the card. The system applies the effects of the card in the game (damage to the enemy base, self-defense, etc.).|

|Attack enemy base:||
|:---|---|
|Description:|User decides to attack the enemy base during his turn.|
|Actors:|User|
|Main flow:|The user selects the enemy base as the target of his attack. The user chooses the cards to use for the attack, if necessary. The system calculates the total damage inflicted to the enemy base. The amount of hit points of the enemy base is reduced according to the calculated damage.|

|Defend own base:||
|:---|---|
|Description:|The user decides to defend his own base during his turn.|
|Actors:|User|
|Main flow:|The user selects the cards from his hand that he will use for defense. The system checks if the user has the necessary points to play the selected cards. The system applies the effects of the cards on the self-defense, reducing or avoiding the damage received.|

