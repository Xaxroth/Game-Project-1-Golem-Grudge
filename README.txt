Welcome to Golem Grudge - a game inspired by Worms 3D.

The game was created as a part of the Game Project 1 course at Future Games.

The features of the game include:

(1: GENERAL) 
Main Menu, Play Scene, Pause Screen and a Game Over feature. 
The player also has the ability to restart the game or exit the application if desired.

The Main Menu uses the UnityEngine.SceneManagement namespace to handle scene navigation.

The Pause Screen uses global variables to temporarily pause the game,
such as AudioListener.volume and Time.timeScale.

(2: GAMEPLAY) 
In the Main Menu, the player can select up to 4 players (2-4) which take turns locally.

(3: TERRAIN) 
The game features one level with a pit that kills players that fall into it.

Upon falling into the trigger that is set underneath the map, the golem takes damage equal to its current health.

Level was constructed with the help of the ProBuilder tool, along with free materials from the asset store.

(4: PLAYER) 
Each team consists of 4 Golems that the player can control. They can switch
between them by using the "TAB" key.

The golems use the Rigidbody system inside of Unity to control movement. Movement is determined
by the use of forces that are applied to the player.

A golem can only move a pre-determined amount, but reaching the maximum move limit
does not end the player's turn. They can either switch and reposition the next golem,
or they can perform an attack.

Each golem has 100 health, and will perish if their health reaches 0. Falling outside the map
will instantly kill any golem, regardless of health level.

(5: CAMERA) 
The camera inside the game focuses on the active player, but will also switch based on events,
for example if a rock is thrown it will follow the rock to its destination.

The player can also aim their attacks in greater detail if they press "Right-Click" whilst
aiming a rock attack.

(6: WEAPONS SYSTEM)
Golems can either throw heavy rocks that detonate on impact with the environment/players, or 
they can choose to perform a smash attack that deals moderate damage and pushes the player
far in the target direction.


Created by William Michelson FGP22

