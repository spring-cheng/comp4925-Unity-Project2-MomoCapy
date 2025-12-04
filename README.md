# MomoCapy: Cat Shelter Adventure 🐈🏡
A Unity WebGL game where you play as a cat inside a shelter environment. The game features movement, spawning cats, interaction with objects, and persistent game result stored through a backend API.

### Game Mechanics Overview
**1. Player Movement**

Move the cat using standard keyboard controls (WASD or arrow keys).

Movement is blocked by collisions to prevent walking through walls and objects.

Allows free exploration of the shelter scene.

**2. Cat Spawning System**

A timed spawner generates new cats every few seconds.

Ensures consistent sprite scaling using proper Pixels Per Unit settings.

Adds activity and challenge to the game environment.

**3. Interaction System**

Clicking or activating in-scene buttons triggers specific actions.

Examples include:
* Spawning new cats
* Updating or displaying UI labels
* Triggering shelter events

**4. Win and Lose Conditions**

Win Condition: Successfully save 10 cats.

Lose Condition: Miss 5 cats (allowing them to escape or failing to interact in time).

**5. Player Progress Tracking**

The game stores:
* Number of cats saved
* Number of cats missed

**6. Cloud Save**

Player score is stored remotely using:
* A Node.js/Express backend hosted on Render
* MongoDB Atlas as the data store

### How to Play

Open the game through the hosted WebGL link.

Move the cat using: the arrow keys

Explore the shelter and interact with objects when prompted. Cats spawn periodically.

**Your goal:**
Save cats before they escape. Avoid missing too many.

**The game ends when:**
You save 10 cats (win) or You miss 5 cats (lose)

### Technologies Used
* Unity WebGL (frontend)
* Netlify (static hosting)
* Node.js + Express (backend API)
* Render (backend hosting)
* MongoDB Atlas (database)
