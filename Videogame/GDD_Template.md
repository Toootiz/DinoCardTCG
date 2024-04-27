# **Dino Card TCG**

## _Game Design Document_

---

##### **[Copyright license](/LICENSE) /[Authors information](#team)** 

##
## _Index_

---

1. [Index](#index)
2. [Game Design](#game-design)
    1. [Summary](#summary)
    2. [Gameplay](#gameplay)
    3. [Mindset](#mindset)
3. [Technical](#technical)
    1. [Screens](#screens)
    2. [Controls](#controls)
    3. [Mechanics](#mechanics)
4. [Level Design](#level-design)
    1. [Themes](#themes)
        1. Ambience
        2. Objects
            1. Ambient
            2. Interactive
        3. Challenges
    2. [Game Flow](#game-flow)
5. [Development](#development)
    1. [Abstract Classes](#abstract-classes--components)
    2. [Derived Classes](#derived-classes--component-compositions)
6. [Graphics](#graphics)
    1. [Style Attributes](#style-attributes)
    2. [Graphics Needed](#graphics-needed)
7. [Sounds/Music](#soundsmusic)
    1. [Style Attributes](#style-attributes-1)
    2. [Sounds Needed](#sounds-needed)
    3. [Music Needed](#music-needed)
8. [Schedule](#schedule)

## _Game Design_

---

### **Summary**

This is a card game where players battle to destroy each other’s bases. The goal is to strategically use offensive, defensive, and spell cards to reduce the opponent’s base points to zero.

### **Gameplay**

The game should be strategic and engaging, with the primary goal of destroying the opponent's base while defending your own. Players must navigate through a dynamic battlefield using offensive, defensive, and spell cards to outmaneuver and outwit their opponents.

### **Mindset**

The main obstacle players face is the opposing player's tactics and card choices, which may include powerful attacks, clever defenses, and unexpected spells. To overcome these obstacles, players must carefully manage their resources, anticipate their opponent's moves, and adapt their strategy on the fly.

## _Technical_

---

### **Screens**

1. Title Screen
    1. Options
        - Play
        - Decks
        - Credits
        - Exit
2. Game
3. Decks
        - Deck Editor
        - See all cards
4. Credits
5. Exit


_(example)_

### **Controls**

Mouse Click 

### **Mechanics**

---

(image) 

- At the beginning of the game, the player is assigned 5 cards from the created deck, which will contain 20 cards, of which at least 3 must be spell-type cards, at least 5 for defense and 5 for attack.

- Cards can be raised to the bench as long as you have enough elixir to be able to summon them. The minimum number of cards that can be raised to the bench does not exist, but the maximum number of cards in the bench is 5.

- Up to 5 cards can be raised per turn if you have the necessary elixir.

- Each turn the player will be assigned a certain amount of elixir points and this amount will increase to a maximum of 8 per round.

- Once on the bench the next turn the player can decide whether to attack with the bank card or can choose between drawing one or more other cards or passing without doing anything in the turn.

- In case of attacking or being attacked, you can only defend yourself if the person affected (the one being attacked) has cards on the bench.

- There are three types of cards, attack which are cards that have more damage points but less life, defense cards which have more life but less attack points and spell type which have almost no attack points. and life but they have abilities that enhance or affect the other cards.

- Within the costs of the letters, there are:
  - Cards of 1 - 3 cost, are cards which are fine at the beginning of the game, these have almost no damage or life.
  - Cards of 4 - 5 cost, are cards more focused on the defense and attack of other cards.
  - Cards of 6 - 8 cost are cards that have special abilities which can mean the “win condition”.

- After 15 turns you can select a card from a total of 8 possibilities that directly affect the game.

- To win you have to destroy the enemy base, which has 20 life points.

- There are cards that allow you to regenerate life in some way.

- After turn 12 the base can no longer regenerate life.

- Who starts the game is selected with a coin.

## _Level Design_

---

There are not levels, only games

### **Themes**

1. Forest
    1. Mood
        1. Dark, calm, foreboding
    2. Objects
        1. _Ambient_
            1. Fireflies
            2. Beams of moonlight
            3. Tall grass
        2. _Interactive_
            1. Wolves
            2. Goblins
            3. Rocks
2. Castle
    1. Mood
        1. Dangerous, tense, active
    2. Objects
        1. _Ambient_
            1. Rodents
            2. Torches
            3. Suits of armor
        2. _Interactive_
            1. Guards
            2. Giant rats
            3. Chests
          
    1. 

_(example)_

### **List of assets**



### **Game Flow**

1. Player starts in forest
2. Pond to the left, must move right
3. To the right is a hill, player jumps to traverse it (&quot;jump&quot; taught)
4. Player encounters castle - door&#39;s shut and locked
5. There&#39;s a window within jump height, and a rock on the ground
6. Player picks up rock and throws at glass (&quot;throw&quot; taught)
7. … etc.

_(example)_

## _Development_

---

### **Abstract Classes / Components**

1. BasePhysics
    1. BasePlayer
    2. BaseEnemy
    3. BaseObject
2. BaseObstacle
3. BaseInteractable

_(example)_

### **Derived Classes / Component Compositions**

1. BasePlayer
    1. PlayerMain
    2. PlayerUnlockable
2. BaseEnemy
    1. EnemyAI
        - Enemy atack
        - Enemy defense
3. BaseObject
    1. ObjectBase
    2. Object
    3. ObjectGoldCoin (cha-ching!)
    4. ObjectKey (pick-up-able, throwable)
4. BaseObstacle
    1. ObstacleWindow (destroyed with rock)
    2. ObstacleWall
    3. ObstacleGate (watches to see if certain buttons are pressed)
5. BaseInteractable
    1. InteractableButton

_(example)_

## _Graphics_

---

### **Style Attributes**

What kinds of colors will you be using? Do you have a limited palette to work with? A post-processed HSV map/image? Consistency is key for immersion.

What kind of graphic style are you going for? Cartoony? Pixel-y? Cute? How, specifically? Solid, thick outlines with flat hues? Non-black outlines with limited tints/shades? Emphasize smooth curvatures over sharp angles? Describe a set of general rules depicting your style here.

Well-designed feedback, both good (e.g. leveling up) and bad (e.g. being hit), are great for teaching the player how to play through trial and error, instead of scripting a lengthy tutorial. What kind of visual feedback are you going to use to let the player know they&#39;re interacting with something? That they \*can\* interact with something?

### **Graphics Needed**

1. Characters
    1. Human-like
        1. Goblin (idle, walking, throwing)
        2. Guard (idle, walking, stabbing)
        3. Prisoner (walking, running)
    2. Other
        1. Wolf (idle, walking, running)
        2. Giant Rat (idle, scurrying)
2. Blocks
    1. Dirt
    2. Dirt/Grass
    3. Stone Block
    4. Stone Bricks
    5. Tiled Floor
    6. Weathered Stone Block
    7. Weathered Stone Bricks
3. Ambient
    1. Tall Grass
    2. Rodent (idle, scurrying)
    3. Torch
    4. Armored Suit
    5. Chains (matching Weathered Stone Bricks)
    6. Blood stains (matching Weathered Stone Bricks)
4. Other
    1. Chest
    2. Door (matching Stone Bricks)
    3. Gate
    4. Button (matching Weathered Stone Bricks)

_(example)_


## _Sounds/Music_

---

### **Style Attributes**

Again, consistency is key. Define that consistency here. What kind of instruments do you want to use in your music? Any particular tempo, key? Influences, genre? Mood?

Stylistically, what kind of sound effects are you looking for? Do you want to exaggerate actions with lengthy, cartoony sounds (e.g. mario&#39;s jump), or use just enough to let the player know something happened (e.g. mega man&#39;s landing)? Going for realism? You can use the music style as a bit of a reference too.

 Remember, auditory feedback should stand out from the music and other sound effects so the player hears it well. Volume, panning, and frequency/pitch are all important aspects to consider in both music _and_ sounds - so plan accordingly!

### **Sounds Needed**

1. Effects
    1. Soft Footsteps (dirt floor)
    2. Sharper Footsteps (stone floor)
    3. Soft Landing (low vertical velocity)
    4. Hard Landing (high vertical velocity)
    5. Glass Breaking
    6. Chest Opening
    7. Door Opening
2. Feedback
    1. Relieved &quot;Ahhhh!&quot; (health)
    2. Shocked &quot;Ooomph!&quot; (attacked)
    3. Happy chime (extra life)
    4. Sad chime (died)

_(example)_

### **Music Needed**

1. Slow-paced, nerve-racking &quot;forest&quot; track
2. Exciting &quot;castle&quot; track
3. Creepy, slow &quot;dungeon&quot; track
4. Happy ending credits track
5. Rick Astley&#39;s hit #1 single &quot;Never Gonna Give You Up&quot;

_(example)_


## _Schedule_

---

_(define the main activities and the expected dates when they should be finished. This is only a reference, and can change as the project is developed)_

1. develop base classes
    1. base entity
        1. base player
        2. base enemy
        3. base block
  2. base app state
        1. game world
        2. menu world
2. develop player and basic block classes
    1. physics / collisions
3. find some smooth controls/physics
4. develop other derived classes
    1. blocks
        1. moving
        2. falling
        3. breaking
        4. cloud
    2. enemies
        1. soldier
        2. rat
        3. etc.
5. design levels
    1. introduce motion/jumping
    2. introduce throwing
    3. mind the pacing, let the player play between lessons
6. design sounds
7. design music

_(example)_

## _Team_ 

---

- Gabriel Muñoz Luna
- Karen Nikole Morales Rosas
- Felipe De Araújo Barbosa

