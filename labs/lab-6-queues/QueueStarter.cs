using System;
using System.Collections.Generic;

/*
=== QUICK REFERENCE GUIDE ===

Queue<T> Essential Operations:
- new Queue<SupportTicket>()        // Create empty queue
- queue.Enqueue(item)               // Add item to back (FIFO)
- queue.Dequeue()                   // Remove and return front item
- queue.Peek()                      // Look at front item (don't remove)
- queue.Clear()                     // Remove all items
- queue.Count                       // Get number of items

Safety Rules:
- ALWAYS check queue.Count > 0 before Dequeue() or Peek()
- Empty queue Dequeue() throws InvalidOperationException
- Empty queue Peek() throws InvalidOperationException

Common Patterns:
- Guard clause: if (queue.Count > 0) { ... }
- FIFO order: First item enqueued is first item dequeued
- Enumeration: foreach gives front-to-back order

Helpful Icons:
- ✅ Success
- ❌ Error
- 👀 Look
- 📋 Display
- ℹ️ Information
- 📊 Stats
- 🎫 Ticket
- 🔄 Process
*/

namespace QueueLab
{
    /// <summary>
    /// Student skeleton version - follow along with instructor to build this out!
    /// Complete the TODO steps to build a complete IT Support Desk Queue system.
    /// </summary>
    class Program
    {
        // TODO Step 1: Set up your data structures and tracking variables

        // Queue to hold all support tickets
        private static Queue<SupportTicket> ticketQueue = new Queue<SupportTicket>();

        // Counter for generating unique ticket IDs (T001, T002, etc.)
        private static int ticketCounter = 1;

        // Track total number of queue operations (enqueue, dequeue, etc.)
        private static int totalOperations = 0;

        // Track when the session started (for statistics later)
        private static DateTime sessionStart = DateTime.Now;


        // Pre-defined ticket options for easy selection during demos
        private static readonly string[] CommonIssues = {
            "Login issues - cannot access email",
            "Password reset request",
            "Software installation help",
            "Printer not working",
            "Internet connection problems",
            "Computer running slowly",
            "Email not sending/receiving",
            "VPN connection issues",
            "Application crashes on startup",
            "File recovery assistance",
            "Monitor display problems",
            "Keyboard/mouse not responding",
            "Video conference setup help",
            "File sharing permission issues",
            "Security software alert"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("🎫 IT Support Desk Queue Management");
            Console.WriteLine("===================================");
            Console.WriteLine("Building a ticket queue system with FIFO processing\n");

            bool running = true;
            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.ToLower() ?? "";

                switch (choice)
                {
                    case "1":
                    case "submit":
                        HandleSubmitTicket();
                        break;
                    case "2":
                    case "process":
                        HandleProcessTicket();
                        break;
                    case "3":
                    case "peek":
                    case "next":
                        HandlePeekNext();
                        break;
                    case "4":
                    case "display":
                    case "queue":
                        HandleDisplayQueue();
                        break;
                    case "5":
                    case "urgent":
                        HandleUrgentTicket();
                        break;
                    case "6":
                    case "search":
                        HandleSearchTicket();
                        break;
                    case "7":
                    case "stats":
                        HandleQueueStatistics();
                        break;
                    case "8":
                    case "clear":
                        HandleClearQueue();
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
            string nextTicket = ticketQueue.Count > 0 ? ticketQueue.Peek().TicketId : "None";

            Console.WriteLine("┌─ Support Desk Queue Operations ─────────────────┐");
            Console.WriteLine("│ 1. Submit      │ 2. Process    │ 3. Peek/Next  │");
            Console.WriteLine("│ 4. Display     │ 5. Urgent     │ 6. Search      │");
            Console.WriteLine("│ 7. Stats       │ 8. Clear      │ 9. Exit        │");
            Console.WriteLine("└─────────────────────────────────────────────────┘");
            Console.WriteLine($"Queue: {ticketQueue.Count} tickets | Next: {nextTicket} | Total ops: {totalOperations}");
            Console.Write("\nChoose operation (number or name): ");
        }

        // TODO Step 2: Handle submitting new tickets (Enqueue)
        static void HandleSubmitTicket()
        {
            Console.WriteLine("\n📝 Submit New Support Ticket");
            Console.WriteLine("Choose from common issues or enter custom:");

            // Math.Min() for safe array access - prevents index out of bounds errors
            // Display quick selection options
            for (int i = 0; i < Math.Min(5, CommonIssues.Length); i++)
            {
                Console.WriteLine($"  {i + 1}. {CommonIssues[i]}");
            }
            Console.WriteLine("  6. Enter custom issue");
            Console.WriteLine("  0. Cancel");
            
            Console.Write("\nSelect option (0-6): ");
            string? choice = Console.ReadLine();
            
            if (choice == "0")
            {
                Console.WriteLine("❌ Ticket submission cancelled.\n");
                return;
            }
            
            string description = "";
            // int.TryParse() for safe number conversion - better than catching exceptions
            if (int.TryParse(choice, out int index) && index >= 1 && index <= 5)
            {
                description = CommonIssues[index - 1];
            }
            else if (choice == "6")
            {
                Console.Write("Enter issue description: ");
                description = Console.ReadLine()?.Trim() ?? "";
            }

            // Input validation with multiple options - professional apps handle user choice
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("❌ Description cannot be empty. Ticket submission cancelled.\n");
                return;
            }

            // TODO:
            // 1. Create ticket ID using ticketCounter (format: "T001", "T002", etc.)
            string ticketId = $"T{ticketCounter:D3}";

            // 2. Create new SupportTicket with ID, description, "Normal" priority, and "User"
            var newTicket = new SupportTicket(ticketId, description, "Normal", "User");

            // 3. Enqueue the ticket to ticketQueue
            ticketQueue.Enqueue(newTicket);

            // 4. Increment ticketCounter and totalOperations
            ticketCounter++;
            totalOperations++;

            // 5. Show success message with ticket ID, description, and queue position
            Console.WriteLine($"✅ Ticket {ticketId} submitted successfully!");
            Console.WriteLine($"   Issue: {description}");
            Console.WriteLine($"   Position in queue: {ticketQueue.Count}\n");
        }

        // TODO Step 3: Handle processing tickets (Dequeue)
        static void HandleProcessTicket()
        {
            // TODO:
            // 1. Display header "Process Next Ticket"
            Console.WriteLine("\n🔄 Process Next Ticket");

            // 2. Check if ticketQueue has items (guard clause!)
            if (ticketQueue.Count == 0)
            {
                // 3. If empty, show "No tickets in queue to process" message
                Console.WriteLine("❌ No tickets in queue to process.\n");
                return;
            }

            // 4. If not empty:
            //    - Dequeue the next ticket from front of queue
            var ticket = ticketQueue.Dequeue();

            //    - Increment totalOperations
            totalOperations++;

            //    - Display "Processing ticket:" message
            Console.WriteLine($"🎫 Processing ticket: {ticket.TicketId}");

            //    - Show ticket details using ToDetailedString() method
            Console.WriteLine(ticket.ToDetailedString());

            //    - Check if queue still has tickets after dequeue
            if (ticketQueue.Count > 0)
            {
                //    - If more tickets exist, show next ticket info using Peek()
                Console.WriteLine($"\nNext up: {ticketQueue.Peek().TicketId} ({ticketQueue.Peek().Description})");
            }
            else
            {
                //    - If queue is now empty, show "all tickets processed" message
                Console.WriteLine("\n✨ All tickets processed. Queue is now empty!");
            }

            Console.WriteLine();
        }

        // TODO Step 4: Handle peeking at next ticket
        static void HandlePeekNext()
        {
            // TODO:
            // 1. Display header "View Next Ticket"
            Console.WriteLine("\n👀 View Next Ticket");

            // 2. Check if ticketQueue has items (guard clause!)
            if (ticketQueue.Count == 0)
            {
                // 3. If empty, show "Queue is empty. No tickets to view" message
                Console.WriteLine("❌ Queue is empty. No tickets to view.\n");
                return;
            }

            // 4. If not empty:
            //    - Use Peek() to look at front ticket without removing it
            var nextTicket = ticketQueue.Peek();

            //    - Display "Next ticket to be processed:" message
            Console.WriteLine("Next ticket to be processed:");

            //    - Show ticket details using ToDetailedString() method
            Console.WriteLine(nextTicket.ToDetailedString());

            //    - Show position information (1 of X in queue)
            Console.WriteLine($"Position: 1 of {ticketQueue.Count} in queue\n");

            // 5. Remember: Peek doesn't modify the queue!
        }

        // TODO Step 5: Handle displaying the full queue
        static void HandleDisplayQueue()
        {
            // TODO:
            // 1. Display header "Current Support Queue (FIFO Order):"
            Console.WriteLine("\n📋 Current Support Queue (FIFO Order):");

            // 2. Check if queue is empty
            if (ticketQueue.Count == 0)
            {
                // 3. If empty, show "Queue is empty - no tickets waiting" and return
                Console.WriteLine("❌ Queue is empty - no tickets waiting.\n");
                return;
            }

            // 4. If not empty:
            //    - Show total ticket count
            Console.WriteLine($"Total tickets in queue: {ticketQueue.Count}\n");

            //    - Use foreach to enumerate through queue (front to back order)
            int position = 1;
            foreach (var ticket in ticketQueue)
            {
                //    - Display each ticket with position number (01, 02, 03, etc.)
                string marker = position == 1 ? " ← Next" : "";
                Console.WriteLine($"{position:D2}. {ticket}{marker}");
                position++;
            }

            Console.WriteLine();
        }

        // TODO Step 6: Handle clearing the queue
        static void HandleClearQueue()
        {
            // TODO:
            // 1. Display header "Clear All Tickets"
            Console.WriteLine("\n⚠️ Clear All Tickets");

            // 2. Check if queue is empty
            if (ticketQueue.Count == 0)
            {
                // 3. If empty, show "Queue is already empty. Nothing to clear" and return
                Console.WriteLine("Queue is already empty. Nothing to clear.\n");
                return;
            }

            // 4. If not empty:
            //    - Save current ticket count before clearing
            int countBefore = ticketQueue.Count;

            //    - Ask for confirmation: "This will remove X tickets. Are you sure? (y/N):"
            Console.Write($"This will remove {countBefore} tickets. Are you sure? (y/N): ");
            string? confirm = Console.ReadLine()?.Trim().ToLower();

            //    - If response is "y" or "yes":
            if (confirm == "y" || confirm == "yes")
            {
                //      - Clear the ticketQueue
                ticketQueue.Clear();

                //      - Increment totalOperations
                totalOperations++;

                //      - Show success message with count of cleared tickets
                Console.WriteLine($"✅ Cleared {countBefore} tickets successfully.\n");
            }
            else
            {
                //    - If response is anything else, show "Clear operation cancelled"
                Console.WriteLine("❌ Clear operation cancelled.\n");
            }
        }

        // TODO Step 7: Handle urgent ticket submission (Priority)
        static void HandleUrgentTicket()
        {
            // TODO:
            // 1. Display header "Submit Urgent Ticket"
            Console.WriteLine("\n🚨 Submit Urgent Ticket");

            // 2. Show explanation: "Urgent tickets are processed first!"
            Console.WriteLine("Urgent tickets are processed first!\n");

            // 3. Prompt for urgent issue description
            Console.Write("Enter urgent issue description: ");
            string description = Console.ReadLine()?.Trim() ?? "";

            // 4. Validate description is not empty or whitespace
            if (string.IsNullOrWhiteSpace(description))
            {
                // 5. If empty, show error and return
                Console.WriteLine("❌ Description cannot be empty. Ticket submission cancelled.\n");
                return;
            }

            // 6. If valid:
            //    - Create ticket ID using "U" prefix and ticketCounter (format: "U001", "U002", etc.)
            string ticketId = $"U{ticketCounter:D3}";

            //    - Create new SupportTicket with ID, description, "Urgent" priority, and "User"
            var urgentTicket = new SupportTicket(ticketId, description, "Urgent", "User");

            //    - For basic implementation: use regular Enqueue (note: real system would prioritize)
            ticketQueue.Enqueue(urgentTicket);

            //    - Increment ticketCounter and totalOperations
            ticketCounter++;
            totalOperations++;

            //    - Show success message with ticket ID and description
            Console.WriteLine($"✅ Urgent ticket {ticketId} submitted successfully!");
            Console.WriteLine($"   Issue: {description}\n");

            //    - Add note explaining that real systems would jump to front of queue
            Console.WriteLine("ℹ️ Note: In a real system, urgent tickets would jump to the front of the queue.\n");
        }

        // TODO Step 8: Handle searching for tickets
        static void HandleSearchTicket()
        {
            // TODO:
            // 1. Display header "Search Tickets"
            Console.WriteLine("\n🔍 Search Tickets");

            // 2. Check if queue is empty
            if (ticketQueue.Count == 0)
            {
                // 3. If empty, show "Queue is empty. No tickets to search" and return
                Console.WriteLine("❌ Queue is empty. No tickets to search.\n");
                return;
            }

            // 4. If not empty:
            //    - Prompt for search term: "Enter ticket ID or description keyword:"
            Console.Write("Enter ticket ID or description keyword: ");
            string? term = Console.ReadLine()?.Trim();

            //    - Validate search term is not empty or whitespace
            if (string.IsNullOrWhiteSpace(term))
            {
                //    - If empty, show error and return
                Console.WriteLine("❌ Search term cannot be empty.\n");
                return;
            }

            //    - Convert search term to lowercase for case-insensitive search
            string searchTerm = term.ToLower();

            //    - Initialize found flag to false and position counter to 1
            bool found = false;
            int position = 1;

            //    - Display "Search results:" header
            Console.WriteLine("\nSearch results:");

            //    - Loop through queue using foreach:
            foreach (var ticket in ticketQueue)
            {
                //      - Check if ticket ID or description contains search term (use ToLower())
                if (ticket.TicketId.ToLower().Contains(searchTerm) || ticket.Description.ToLower().Contains(searchTerm))
                {
                    //      - If match found, display position and ticket info, set found flag
                    Console.WriteLine($"  {position:D2}. {ticket}");
                    found = true;
                }

                //      - Increment position counter
                position++;
            }

            //    - After loop, if no matches found, show "No tickets found matching '[searchterm]'"
            if (!found)
            {
                Console.WriteLine($"❌ No tickets found matching '{term}'.");
            }

            Console.WriteLine();
        }


        static void HandleQueueStatistics()
        {
            Console.WriteLine("\n📊 Queue Statistics");
            
            TimeSpan sessionDuration = DateTime.Now - sessionStart;
            
            Console.WriteLine($"Current Queue Status:");
            Console.WriteLine($"- Tickets in queue: {ticketQueue.Count}");
            Console.WriteLine($"- Total operations: {totalOperations}");
            Console.WriteLine($"- Session duration: {sessionDuration:hh\\:mm\\:ss}");
            Console.WriteLine($"- Next ticket ID: T{ticketCounter:D3}");
            
            if (ticketQueue.Count > 0)
            {
                var oldestTicket = ticketQueue.Peek();
                Console.WriteLine($"- Longest waiting: {oldestTicket.TicketId} ({oldestTicket.GetFormattedWaitTime()})");
                
                // Count by priority
                int normal = 0, high = 0, urgent = 0;
                foreach (var ticket in ticketQueue)
                {
                    switch (ticket.Priority.ToLower())
                    {
                        case "normal": normal++; break;
                        case "high": high++; break;
                        case "urgent": urgent++; break;
                    }
                }
                Console.WriteLine($"- By priority: 🟢 Normal({normal}) 🟡 High({high}) 🔴 Urgent({urgent})");
            }
            else
            {
                Console.WriteLine("- Queue is empty");
            }
            Console.WriteLine();
        }

        static void ShowSessionSummary()
        {
            Console.WriteLine("\n📋 Final Session Summary");
            Console.WriteLine("========================");
            
            TimeSpan sessionDuration = DateTime.Now - sessionStart;
            
            Console.WriteLine($"Session Statistics:");
            Console.WriteLine($"- Duration: {sessionDuration:hh\\:mm\\:ss}");
            Console.WriteLine($"- Total operations: {totalOperations}");
            Console.WriteLine($"- Tickets remaining: {ticketQueue.Count}");
            
            if (ticketQueue.Count > 0)
            {
                Console.WriteLine($"- Unprocessed tickets:");
                int position = 1;
                foreach (var ticket in ticketQueue)
                {
                    Console.WriteLine($"  {position:D2}. {ticket}");
                    position++;
                }
                Console.WriteLine("\n⚠️ Remember to process remaining tickets!");
            }
            else
            {
                Console.WriteLine("✨ All tickets processed - excellent work!");
            }
            
            Console.WriteLine("\nThank you for using the Support Desk Queue System!");
            Console.WriteLine("You've learned FIFO queue operations and real-world ticket management! 🎫\n");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}