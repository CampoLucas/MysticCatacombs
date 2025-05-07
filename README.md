# Mystic Catacombs

## Description
A small 3D demo of a top-down fantasy game. Features a single playable level showcasing basic enemy AI behavior and a prototype combat system.

## Setup Instructions
### Requirements
- **Unity Version**: 2022.3.46f1
- **Platform**: Windows

### How to Run
1. Clone the repository
   ```bash
   https://github.com/(your git username)/MysticCatacombs.git

2. Install Unity [2021.3.18f1](https://unity.com/releases/editor/whats-new/2021.3.18#installs)
3. Open the project in Unity
4. Open the scene "Main Menu" in "Assets/_Main/Scenes/"

## Technical Info
### Design Patterns
This demo implements the following design patterns:
- **Factory:** For instancing projectiles.
- **Mediator:** For the Main Menu.
- **Observer:** For managing the game states (eg. Game Over).
- **Pool:** Used to recycle projectiles spawned with **Factory**
- **Predicates:** Used for the transitions in the **FSM**

### Algorithms
The demo uses the following algorithims for the enemies and the player:
- **Finate State Machine:** Executes the player's and enemies states.
- **A Star Path Finding:** Calculates the path of the enemies.
- **Flocking:** Simulates group movement dynamics for a more natural enemy movement.
- **Steering Behaviours:** Used to achive diferent type of movements for the enemies.
- **Custom Behaviour Tree:** Created a custom-built tool for creating and editing behaviour trees directly from the inspector. 

