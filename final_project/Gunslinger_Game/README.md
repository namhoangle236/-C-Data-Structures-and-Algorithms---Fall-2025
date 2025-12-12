# Gunslinger text-based game

> A simple text based turn based action game where you can choose your own path; while also allowing user to create their own map with predefined room types

---

## What I Built (Overview)

**Problem this solves:**  
- This game demonstrates how graphs, queues, list, and game state can be used together to model player choices and progression. It also allows users to experiment by building their own maps to better understand graph connections.

**Core features:**  
- Graph-based map navigation (rooms connected as vertices and edges)  
- Turn-based combat with a bullet queue system  
- Multiple room types with different effects (Enemy, Heal, Ammo, Boss, Goal)  
- Custom map builder using predefined room types

## How to Run 
- Clone the project
- Install .NET SDK 8.0
- Navigate to the project folder
- Run `dotnet run` in the terminal

```bash
git clone <your-repo-url>
cd <your-folder>
dotnet build
```

**Run:**  
```bash
dotnet run
```

**Sample data (if applicable):**  
- A premade map is in GameLogic.cs along with the commemted visualization


## Using the App (Quick Start)

**Typical workflow:**  
1. Launch the game and choose to play a premade map or create a custom map.  
2. Navigate between rooms by selecting connected paths in the map.  
3. Engage in turn-based combat or interact with special rooms (Heal, Ammo, Boss).  
4. Reach a Goal room to win or lose if your HP reaches zero.

**Input tips:**  
- For custom map maker, READ the RULES!

## Data Structures (Brief Summary)

> Full rationale is in **DESIGN.md**.

**Data structures used:**  
- `Dictionary<string, List<string>>` → Represents the game map as a graph, where rooms are vertices and paths are edges.
- `List<string>` → Stores neighboring rooms and user-created room connections.
- `Queue<Bullet>` → Models the bullet chamber so bullets fire in the order they are loaded (FIFO).
- `Enum (BulletType)` → Defines different bullet types with distinct behaviors.
- `Custom classes (PlayerState, Bullet)` → Store and manage game state and bullet data.

---

## Manual Testing Summary

**Test scenarios:**

**Scenario 1: Premade Map Playthrough**
- Steps:
  - Start the game and choose the premade map.
  - Move through connected rooms and engage in combat.
  - Reach the Goal room.
- Expected result:
  - Player moves correctly between rooms, combat resolves properly, and the game ends with a win message.
- Actual result:
  - Game behaved as expected and ended when the Goal room was reached.

**Scenario 2: Bullet Queue Order**
- Steps:
  - Enter an Enemy room with multiple bullets loaded.
  - Fire bullets one by one.
- Expected result:
  - Bullets fire in the order they were added to the chamber and deal correct damage.
- Actual result:
  - Bullets fired in FIFO order and damage values were applied correctly.

**Scenario 3: Custom Map Creation**
- Steps:
  - Choose the custom map option.
  - Add valid edges using predefined room types.
  - Play through the custom map.
- Expected result:
  - Custom map builds successfully and gameplay follows the user-defined paths.
- Actual result:
  - Custom map loaded correctly and gameplay followed the created structure.


---

## Known Limitations

**Limitations and edge cases:**
- Custom maps are not validated to guarantee a reachable Goal room.
- Room names are user-defined, so duplicate or poorly structured maps are possible.
- No save/load system; the game must be played in one session.

## Comparers & String Handling

**Keys comparer:**  
The default string comparer is used for dictionary keys. Room names are case-sensitive, which keeps behavior simple and predictable for this project.

**Normalization:**  
User input is trimmed to remove leading and trailing whitespace. Room behavior relies on consistent name prefixes (Start, Enemy, Heal, Ammo, Boss, Goal) to trigger the correct logic.


---

## Credits & AI Disclosure

- Google and ChatGPT is used for reasearch into data flow layout; more specifically, where moving between verticies logic is added. AI were also used to help fix bugs, syntax helps such as string check,and user input mapping structure.


## Challenges and Solutions

**Biggest challenge faced, and how I solved it:**  
- Figuring out where to implement the verticies traverse logic, I initially want to have them in GameLogic.cs; but it's much better to have them in GameUI, as it perform the traverse prompt after the vertex's logic finish running.
- Validating user input and map them to pass in correct vertex's name. I first only rely on user's entry for the entire vertex name, but it comes with a lot of string check logic; so I decided to limit the user to a few choices, displaying error for anything else aside from those choices. This makes it easier to control user's input, but also make it more tedious for the user to create verticies edges.

**Most confusing concept:**  
- N/A, the concepts I used in this assignment are the ones that I know well, the only difficulties comes from unfaamiliar syntax, mostly related to input and string check operations, as well as mapping them to pass in correct verticies names

## Code Quality

**What you're most proud of in your implementation:**  
- Implementation of rapid fire (consume all bullet to deal massive damage); and the ability to make your own map, albeit lacaking in clarity and fluidity



**What you would improve if you had more time:**  
- I had plan for real time magazine bullet consumtion and fire effect display (with text), but left it out to focus more on the core functionality and bug fixing
- I would also implement a way for user to create their own custom Vertex type (still based on the predefined room type) with modified numbers
- Custom grahp visualization (The big fish! could be another project of its own.)

**Your Answer:**

## Real-World Applications

**How this relates to real-world systems:**  
- This project is similar to real systems that use graphs and queues, such as navigation systems, workflow engines, and game state management. The custom map builder is comparable to tools that let users define processes or routes.

**What you learned about data structures and algorithms:**  
- I learned how choosing the right data structure makes features easier to implement, such as using a queue for ordered actions and a dictionary for fast lookups. This project also showed how data structures work together to support real application behavior.

## Submission Checklist

- [✔️] Public GitHub repository link submitted
- [✔️] README.md completed (this file)
- [✔️] DESIGN.md completed
- [✔️] Source code included and builds successfully
- [] (Optional) Slide deck or 5–10 minute demo video link (unlisted)

**Demo Video Link (optional):**
