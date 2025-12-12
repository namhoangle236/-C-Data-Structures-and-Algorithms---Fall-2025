# Project Design & Rationale

---

## Data Model & Entities

**Core entities:**  

**Entity A: PlayerState**
- Name: PlayerState
- Key fields: HP, Chamber (Queue<Bullet>), CurrentRoom
- Identifiers: CurrentRoom
- Relationships: Interacts with rooms in the graph and uses bullets during combat

**Entity B: Bullet**
- Name: Bullet
- Key fields: Type, Damage
- Identifiers: N/A
- Relationships: Stored in the player's bullet chamber and consumed during fights

**Identifiers (keys) and why they're chosen:**  
- Room names (strings) are used as keys because they are easy to read, debug, and reference in a text-based game. Using strings also makes it simple to build custom maps and connect rooms without additional ID systems.

---

## Data Structures — Choices & Justification

### Structure #1

**Chosen Data Structure:**  
`Dictionary<string, List<string>>` (graph adjacency list)

**Purpose / Role in App:**  
Stores the map. Each room name maps to a list of connected rooms (movement options).

**Why it fits:**  
Fast lookup of a room’s neighbors by name, and easy to add edges. Works well for a small-to-medium map.

**Alternatives considered:**  
2D matrix / adjacency matrix (wastes space for sparse graphs), or custom graph classes (more code without added value here).

---

### Structure #2

**Chosen Data Structure:**  
`Queue<Bullet>`

**Purpose / Role in App:**  
Bullet chamber. Bullets fire in the same order they are loaded (Shoot / Reload / Rapid Fire).

**Why it fits:**  
FIFO behavior matches the game rule directly, and enqueue/dequeue are simple and efficient.

**Alternatives considered:**  
`List<Bullet>` (would need manual index tracking/removal), `Stack<Bullet>` (wrong order).

---

### Structure #3

**Chosen Data Structure:**  
`List<string>`

**Purpose / Role in App:**  
Stores neighbor lists for each room and supports building custom graphs.

**Why it fits:**  
Neighbors are displayed in order, iterated often, and the size is small.

**Alternatives considered:**  
`HashSet<string>` (no ordering, and not needed unless preventing duplicate neighbors).

---

### Additional Structures (if applicable)

**Your Answer:**  
`enum BulletType` to define bullet categories (Normal/Heavy), and simple classes (`PlayerState`, `Bullet`) to store game state and bullet data.


---

## Comparers & String Handling

**Comparer choices:**  
Default string comparison is used to keep behavior simple and predictable for a console-based project.

**For keys:**  
Room names use the default string comparer and are case-sensitive to avoid unexpected matches.

**For display sorting (if different):**  
N/A — room lists are displayed in the order they are added.

**Normalization rules:**  
User input is trimmed to remove leading and trailing whitespace. Room behavior relies on fixed name prefixes (Start, Enemy, Heal, Ammo, Boss, Goal).

**Bad key examples avoided:**  
- Auto-generated random IDs, which would make debugging harder in a text-based game.  
- Keys with trailing spaces, which could cause duplicate or unreachable rooms.  
- Keys based on display text or dynamic values that could change during gameplay.

---

## Performance Considerations

**Expected data scale:**  
Small scale. A typical map is around 10–20 rooms with a few connections each, and the bullet chamber is usually under 20 bullets.

**Performance bottlenecks identified:**  
No major bottlenecks at this scale. The main loops are simple (movement + combat). Using a dictionary for the map keeps neighbor lookup fast.

**Big-O analysis of core operations:**
- Add: `AddEdge` is O(1) average to find/create keys, plus O(1) to append neighbor.
- Search: `GetNeighbors` is O(1) average by room name.
- List: Showing neighbors is O(n), where n is the number of neighbors.
- Update: Combat HP updates are O(1); reloading is O(1); rapid fire is O(n) where n is bullets in the chamber.
- Delete: Shooting (dequeue) is O(1); rapid fire clears the queue in O(n).

---

## Design Tradeoffs & Decisions

**Key design decisions:**  
- Used an adjacency-list graph (`Dictionary<string, List<string>>`) to represent the map and make movement simple.  
- Used a queue for bullets so firing order matches how bullets are loaded (FIFO).  
- Kept `GameLogic` separate from `GameUI` so game rules are not mixed with printing and input.

**Tradeoffs made:**  
- Kept room names case-sensitive and simple instead of adding more complex validation or comparers.  
- Custom map builder is flexible, but it does not guarantee the map is winnable.

**What you would do differently with more time:**  
- Add validation to ensure a custom map has a reachable Goal room.  
- Add save/load and better map visualization.  
- Add more enemy/bullet types and balance the combat system.
