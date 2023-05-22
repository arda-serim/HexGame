# Castle Wars

## Game Overview
Castle Wars is a 2-player game set on a 20x11 hex-tiled map. Each player aims to be the last one standing with their castle intact.

## Winning Condition
The player is considered the winner if they are the only player whose castle is not destroyed.

## Losing Condition
The player loses if their castle is destroyed.

## Game Setup
Each player has their castle placed on predetermined tiles. All players start with 10 credits to spend in the first round.

## Game Rules
1. Every round, each player is given credits according to the formula `8 + (x / 15 + 2) * (x / 5)`, where `x` is the round number. The result is an integer.
2. Players can place units by clicking on their castle.
3. Players can move their units by clicking on them.
4. Each unit can either move or attack only once in every round.
5. The round ends for a player once they click the “Finish Round” button.
6. Each unit can attack a unit within their attack range or move within their movement points and attack.
7. Each castle starts with 20 HP.

## Units
All units have specific attributes:

| Unit | HP (Hit Point) | AP (Attack Point) | MP (Movement Point) | Credit Cost | Range |
| ---- | ---- | ---- | ---- | ---- | ---- |
| Warrior | 10 | 1d8 | 2 | 8 | 1 |
| Archer | 8 | 1d6 | 2 | 6 | 3 |
| Cavalry | 10 | 1d8 | 4 | 12 | 1 |
| Knight | 16 | 1d8 | 1 | 20 | 1 |
| Assassin | 6 | 2d6 | 4 | 20 | 2 |

Players can place Warrior and Archer units starting with the first round. Cavalry units can be placed starting round 5, Knights in round 7, and Assassins in round 10.
