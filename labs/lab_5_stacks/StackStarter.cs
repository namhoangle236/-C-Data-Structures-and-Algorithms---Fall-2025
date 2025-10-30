using System;
using System.Collections.Generic;

/*
=== QUICK REFERENCE GUIDE ===

Stack<T> Essential Operations:
- new Stack<string>()           // Create empty stack
- stack.Push(item)              // Add item to top (LIFO)
- stack.Pop()                   // Remove and return top item
- stack.Peek()                  // Look at top item (don't remove)
- stack.Clear()                 // Remove all items
- stack.Count                   // Get number of items

Safety Rules:
- ALWAYS check stack.Count > 0 before Pop() or Peek()
- Empty stack Pop() throws InvalidOperationException
- Empty stack Peek() throws InvalidOperationException

Common Patterns:
- Guard clause: if (stack.Count > 0) { ... }
- LIFO order: Last item pushed is first item popped
- Enumeration: foreach gives top-to-bottom order

Helpful icons!:
- ✅ Success
- ❌ Error
- 👀 Look
- 📋 Display out
- ℹ️ Information
- 📊 Stats
- 📝 Write
*/

namespace StackLab
{
    /// <summary>
    /// Student skeleton version - follow along with instructor to build this out!
    /// Uncomment the class name and Main method when ready to use this version.
    /// </summary>
    // class Program  // Uncomment this line when ready to use
    class StudentSkeleton
    {

        // TODO: Step 1 - Declare two stacks for action history and undo functionality
        static Stack<string> actionHistory = new Stack<string>();           // Note: static here, so this can be accessed from all static methods
        static Stack<string> undoHistory = new Stack<string>();             // same shiii

        // TODO: Step 2 - Add a counter for total operations
        static int totalOperations = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Interactive Stack Demo ===");
            Console.WriteLine("Building an action history system with undo/redo\n");

            bool running = true;
            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.ToLower() ?? "";

                switch (choice)
                {
                    case "1":
                    case "push":
                        HandlePush();
                        break;
                    case "2":
                    case "pop":
                        HandlePop();
                        break;
                    case "3":
                    case "peek":
                    case "top":
                        HandlePeek();
                        break;
                    case "4":
                    case "display":
                        HandleDisplay();
                        break;
                    case "5":
                    case "clear":
                        HandleClear();
                        break;
                    case "6":
                    case "undo":
                        HandleUndo();
                        break;
                    case "7":
                    case "redo":
                        HandleRedo();
                        break;
                    case "8":
                    case "stats":
                        ShowStatistics();
                        break;
                    case "9":
                    case "exit":
                        running = false;
                        ShowSessionSummary();
                        break;
                    default:
                        Console.WriteLine("❌ Invalid choice. Please try again.\n");
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("┌─ Stack Operations Menu ─────────────────────────┐");
            Console.WriteLine("│ 1. Push      │ 2. Pop       │ 3. Peek/Top    │");
            Console.WriteLine("│ 4. Display   │ 5. Clear     │ 6. Undo        │");
            Console.WriteLine("│ 7. Redo      │ 8. Stats     │ 9. Exit        │");
            Console.WriteLine("└─────────────────────────────────────────────────┘");
            // TODO: Step 3 - add stack size and total operations to our display
            Console.WriteLine($"\n📋 Current Stack Size: {actionHistory.Count}");
            Console.WriteLine($"📝 Total Operations: {totalOperations}");

            Console.Write("\nChoose operation (number or name): ");
        }

        // TODO: Step 4 - Implement HandlePush method
        static void HandlePush()
        {
            // TODO: 
            // 1. Prompt user for input
            Console.Write("📝 Enter an action to push: ");
            string? input = Console.ReadLine();

            // 2. Validate input is not empty
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Input cannot be empty.\n");
                return;
            }

            // 3. Push to actionHistory stack
            actionHistory.Push(input);

            // 4. Clear undoHistory stack (new action invalidates redo)
            undoHistory.Clear();

            // 5. Increment totalOperations
            totalOperations++;

            // 6. Show confirmation message
            Console.WriteLine($"✅ '{input}' added to the stack.\n");
        }

        // TODO: Step 5 - Implement HandlePop method
        static void HandlePop()
        {
            // TODO:
            // 1. Check if actionHistory stack has items (guard clause!)
            if (actionHistory.Count == 0)
            {
                // 2. If empty, show error message
                Console.WriteLine("❌ Stack is empty. Nothing to pop.\n");
                return;
            }

            // 3. If not empty:
            //    - Pop from actionHistory
            string removed = actionHistory.Pop();

            //    - Push popped item to undoHistory (for redo)
            undoHistory.Push(removed);

            //    - Increment totalOperations
            totalOperations++;

            //    - Show what was popped
            Console.WriteLine($"👀 Popped: '{removed}'");

            //    - Show new top item (if any)
            if (actionHistory.Count > 0)
                Console.WriteLine($"👆 New top item: {actionHistory.Peek()}\n");
            else
                Console.WriteLine("ℹ️ Stack is now empty.\n");
        }

        // TODO: Step 6 - Implement HandlePeek method
        static void HandlePeek()
        {
            // TODO:
            // 1. Check if actionHistory stack has items
            if (actionHistory.Count == 0)
            {
                // 2. If empty, show appropriate message
                Console.WriteLine("ℹ️ Stack is empty. Nothing to peek.\n");
                return;
            }

            // 3. If not empty, peek at top item and display
            string top = actionHistory.Peek();

            // 4. Remember: Peek doesn't modify the stack!
            Console.WriteLine($"👀 Top of stack: '{top}' (not removed)\n");
        }

        // TODO: Step 7 - Implement HandleDisplay method
        static void HandleDisplay()
        {
            // TODO:
            // 1. Show a header for the display
            Console.WriteLine("\n📋 Current Stack Contents (Top → Bottom):");

            // 2. Check if stack is empty
            if (actionHistory.Count == 0)
            {
                Console.WriteLine("ℹ️ Stack is empty.\n");
                return;
            }

            // 3. If not empty, enumerate through stack (foreach)
            int index = actionHistory.Count;
            foreach (string item in actionHistory)
            {
                // 4. Show items in LIFO order with position numbers
                if (index == actionHistory.Count)
                    Console.WriteLine($"[{index}] {item} 👈 (Top)");
                else
                    Console.WriteLine($"[{index}] {item}");
                index--;
            }

            // 6. Show total count
            Console.WriteLine($"📊 Total items: {actionHistory.Count}\n");
        }

        // TODO: Step 8 - Implement HandleClear method
        static void HandleClear()
        {
            // TODO:
            // 1. Check if there are items to clear
            if (actionHistory.Count == 0 && undoHistory.Count == 0)
            {
                // 2. If empty, show info message
                Console.WriteLine("ℹ️ Nothing to clear. Both stacks are empty.\n");
                return;
            }

            // 3. If not empty:
            //    - Remember count before clearing
            int clearedCount = actionHistory.Count + undoHistory.Count;

            //    - Clear both actionHistory and undoHistory
            actionHistory.Clear();
            undoHistory.Clear();

            //    - Increment totalOperations
            totalOperations++;

            //    - Show confirmation with count cleared
            Console.WriteLine($"✅ Cleared {clearedCount} total items from history.\n");
        }

        // TODO: Step 9 - Implement HandleUndo method (Advanced)
        static void HandleUndo()
        {
            // TODO:
            // 1. Check if undoHistory has items to restore
            if (undoHistory.Count == 0)
            {
                // 2. If empty, show "nothing to undo" message
                Console.WriteLine("ℹ️ Nothing to undo.\n");
                return;
            }

            // 3. If not empty:
            //    - Pop from undoHistory
            string restored = undoHistory.Pop();

            //    - Push back to actionHistory
            actionHistory.Push(restored);

            //    - Increment totalOperations
            totalOperations++;

            //    - Show what was restored
            Console.WriteLine($"↩️ Undid action: '{restored}'\n");
        }

        // TODO: Step 10 - Implement HandleRedo method (Advanced)
        static void HandleRedo()
        {
            // TODO:
            // 1. Check if actionHistory has items to redo
            if (actionHistory.Count == 0)
            {
                // 2. If empty, show "nothing to redo" message
                Console.WriteLine("ℹ️ Nothing to redo.\n");
                return;
            }

            // 3. If not empty:
            //    - Pop from actionHistory
            string redoItem = actionHistory.Pop();

            //    - Push to undoHistory
            undoHistory.Push(redoItem);

            //    - Increment totalOperations
            totalOperations++;

            //    - Show what was redone
            Console.WriteLine($"🔁 Redid action: '{redoItem}'\n");
        }

        // TODO: Step 11 - Implement ShowStatistics method
        static void ShowStatistics()
        {
            // TODO:
            // Display current session statistics:
            // - Current stack size
            // - Undo stack size
            // - Total operations performed
            // - Whether stack is empty
            // - Current top action (if any)
            Console.WriteLine("\n📊 Session Statistics:");
            Console.WriteLine($"• Action stack size: {actionHistory.Count}");
            Console.WriteLine($"• Undo stack size: {undoHistory.Count}");
            Console.WriteLine($"• Total operations: {totalOperations}");
            Console.WriteLine($"• Stack empty? {(actionHistory.Count == 0 ? "Yes" : "No")}");
            if (actionHistory.Count > 0)
                Console.WriteLine($"• Current top action: {actionHistory.Peek()}");
            Console.WriteLine();
        }

        // TODO: Step 12 - Implement ShowSessionSummary method
        static void ShowSessionSummary()
        {
            // TODO:
            // Show final summary when exiting:
            // - Total operations performed
            // - Final stack size
            // - List remaining actions (if any)
            // - Encouraging message
            // - Wait for keypress before exit

            Console.WriteLine("\n=== 📋 Session Summary ===");
            Console.WriteLine($"Total operations performed: {totalOperations}");
            Console.WriteLine($"Final stack size: {actionHistory.Count}");

            if (actionHistory.Count > 0)
            {
                Console.WriteLine("\nRemaining actions in stack:");
                foreach (string item in actionHistory)
                    Console.WriteLine($"- {item}");
            }
            else
            {
                Console.WriteLine("Stack is empty. All actions cleared!");
            }

            Console.WriteLine("\n🎉 Great job experimenting with stacks!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
