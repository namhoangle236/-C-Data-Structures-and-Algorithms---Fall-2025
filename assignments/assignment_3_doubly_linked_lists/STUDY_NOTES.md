# STUDY_NOTES.md  
### Doubly Linked List & Music Playlist Project  

---
## Documenting Progress  

At the start, I had to understand how the **Node** and **DoublyLinkedList skeleton** worked together — the Node representing one element with `Data`, `Next`, and `Previous`, while the list itself manages structure and traversal (plus a bunch of other functionalities live here)  

From there, I learned how to **traverse** using `head` and `tail`, moving forward or backward without relying on array indices. Understanding how `Next` and `Previous` linked each node was key to grasping the entire structure.  

Once that made sense, I began implementing small operations step by step following the comment guide:  
- Started with **adding a node at the beginning** (`AddFirst`) and **adding at the end** (`AddLast`).  
- Once I understood how to link nodes for adding, **removing nodes** followed the same logic in reverse.  
- Practiced **inserting in the middle**, then moved on to **searching** and **traversal display**. etc...
- Using DoublyLinkedList for the music player was similar to calling API, accessing vaarious functions.  

## Challenges Faced  

1. **Understanding Access Modifiers**  
   Confused between private and public access of `head` and `tail`. Later learned that private fields store actual references, while public properties like `First` and `Last` provide safe read-only access.  

2. **C# Comparison Syntax**  
   Struggled with `EqualityComparer<T>.Default.Equals`. Did not know how to compares multiple data types within the node to each other.

3. **Array Mindset vs. Linked List Thinking**  
   Initially kept using index-based loops (`for`, `while`) instead of traversing nodes using `head` and `tail`.  

4. **Unnecessary Temporary Variables**  
   Created redundant variables like `current` in simple insert/remove operations where direct node reference was sufficient.  

5. **Yield Return and Foreach Confusion**  
   Didn’t understand how `foreach` worked without an array. Learned that `yield return` allows C# to lazily generate each node’s data, enabling smooth iteration without extra memory usage.  

---

## Reflections  

Working through this project improved my understanding of **pointer manipulation**, **data structure efficiency**, and **object-oriented design** in C#.  
I would use linked lists for node-type structures, where each nodes can be plucked and placed anywhere in the chain without having the rest of the chain's position be recalculated unless needed. Fast index access is one big down side, since it's O(n) as the chain grows larger, so I would not use it for tasks like building dictionary, or book cataloging.
