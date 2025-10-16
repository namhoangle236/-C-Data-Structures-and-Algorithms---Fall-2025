# Data Structure Operation Complexity Analysis

## PREDICTION 
Big-O for each operation (assume typical .NET implementations and average cases unless noted):

| Structure | Operation | Big-O (Avg) | One-sentence rationale |
|---|---|---|---|
| Array | Access by index |  O(1) | directly access just the index, doesn't need to loop |
| Array | Search (unsorted) | O(n) | loop through each item, perform task on each, depends on n size |
| List<T> | Add at end | O(1) | simple add at the end, gives it the latest index, doesn't need to recalculate the rest|
| List<T> | Insert at index | O(n) | similar to add at the end, but needs to recaculate index of the other items |
| Stack<T> | Push / Pop / Peek | O(1) | similar to add at the end, perform operation only on the item at the top of the stack |
| Queue<T> | Enqueue / Dequeue / Peek | O(1) | similar to stack, perform operation only on the item at the top of queue (FIFO) or adds to the bottom of the queue |
| Dictionary<K,V> | Add / Lookup / Remove | O(1) | key-value stuff, similar to index, get the key, return the value, no ned for loop |
| HashSet<T> | Add / Contains / Remove | O(1) | basically an unordered unique item, each has unique hash, so looking up an item is like directly accessing the index |