using System;
using System.Collections.Generic;

namespace Gunslinger_Game
{
    public static class GameUI
    {
        public static void Run()
        {
            // -----------------------------------------------
            // Main menu
            // -----------------------------------------------

            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("        GUNSLINGER GAME");
            Console.WriteLine("===================================");
            Console.WriteLine("1) Play premade map");
            Console.WriteLine();
            // premade map layout
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("       Start                ");
            Console.WriteLine("         |                  ");
            Console.WriteLine("       Enemy1               ");
            Console.WriteLine("     /   |    \\            ");
            Console.WriteLine("Heal1  Ammo1  Enemy2        ");
            Console.WriteLine("   \\    |      |           ");
            Console.WriteLine("   Enemy3    Heal2          ");
            Console.WriteLine("     \\  |     /            ");
            Console.WriteLine("        Ammo2               ");
            Console.WriteLine("         |                  ");
            Console.WriteLine("       Boss1                ");
            Console.WriteLine("         |                  ");
            Console.WriteLine("       Goal1                ");
            Console.WriteLine("-----------------------------------");
            // end of premade map layout
            Console.WriteLine();
            Console.WriteLine("2) Play custom map");
            Console.WriteLine("3) Quit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                Play(GameLogic.BuildPremadeGraph());        // Play premade map
            }
            else if (choice == "2")
            {
                Play(BuildCustomGraph());                   // Play custom map
            }
        }


        // -----------------------------------------------
        // Play logic
        // -----------------------------------------------
        private static void Play(Dictionary<string, List<string>> graph)    // Play game with given graph
        {
            var player = GameLogic.CreateNewPlayer();                       // Create new player

            // Initial room
            Console.WriteLine(GameLogic.EnterRoom(player, "Start"));        // Put player in starting room
            // Room's End conditions handled by GameLogic messages
            // After room logic ends, while loop for next room choices

            while (true)
            {               
                if (!player.IsAlive || player.CurrentRoom.StartsWith("Goal"))
                    return;                                                                     // End game if player is dead or reached goal, end game                        

                List<string> neighbors = GameLogic.GetNeighbors(graph, player.CurrentRoom);     // Get neighboring rooms of current room the player is in

                if (neighbors.Count == 0)
                    return;                                                                     // No more rooms to go to, end game


                // Display neighboring rooms
                Console.WriteLine();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Choose next room:");

                for (int i = 0; i < neighbors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {neighbors[i]}");                              // List neighboring rooms with indices
                }

                Console.Write("Choose: ");
                string input = Console.ReadLine() ?? "";                                        // Get user input, if null set to empty string

                if (!int.TryParse(input, out int index) ||                                      // Try to parse input to integer, if fails or not within available neighbor count, ask again
                    index < 1 || index > neighbors.Count)
                {
                    Console.WriteLine("Invalid choice.");
                    continue;
                }

                string nextRoom = neighbors[index - 1];                                         // Get chosen neighboring room based on user input, index - 1 for 0-based index
                Console.WriteLine(GameLogic.EnterRoom(player, nextRoom));
                // Go to next room, the while loop will repeat until end conditions are met
            }
        }


        // -----------------------------------------------
        // Custom graph builder (After finishing, it will return CustomGraph, which will be used in Play(BuildCustomGraph()) => check main menu block to see where it's used)
        // -----------------------------------------------
        private static Dictionary<string, List<string>> BuildCustomGraph()
        {
            var CustomGraph = GameLogic.CreateEmptyGraph();

            Console.WriteLine("Custom map builder");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Room types:");
            Console.WriteLine("1) Start  2) Enemy  3) Ammo  4) Heal  5) Boss  6) Goal");
            Console.WriteLine("RULES:");
            Console.WriteLine("You may add a suffix to differentiate rooms (Enemy1, Heal-A, AmmoDeposit1 etc.)");
            Console.WriteLine("First, the Start room will be created automatically.");
            Console.WriteLine("If you use the same name for FROM and TO, it will create a self-loop.");
            Console.WriteLine("If you use the same name for different rooms, they will be treated as the same room.");
            Console.WriteLine("-----------------------------------");

            GameLogic.AddVertex(CustomGraph, "Start");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1) Add edge");
                Console.WriteLine("2) Done");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";
                if (choice == "2")
                    break;

                // FROM
                Console.WriteLine("Choose FROM room type (1-6):");
                string fromType = Console.ReadLine() ?? "";

                Console.Write("FROM suffix (optional): ");
                string fromSuffix = Console.ReadLine() ?? "";

                // TO
                Console.WriteLine("Choose TO room type (1-6):");
                string toType = Console.ReadLine() ?? "";

                Console.Write("TO suffix (optional): ");
                string toSuffix = Console.ReadLine() ?? "";

                string from = "";
                string to = "";

                // Map FROM
                if (fromType == "1") from = "Start";
                else if (fromType == "2") from = "Enemy";
                else if (fromType == "3") from = "Ammo";
                else if (fromType == "4") from = "Heal";
                else if (fromType == "5") from = "Boss";
                else if (fromType == "6") from = "Goal";
                else
                {
                    Console.WriteLine("Invalid FROM room type.");
                    continue;
                }

                // Map TO
                if (toType == "1") to = "Start";
                else if (toType == "2") to = "Enemy";
                else if (toType == "3") to = "Ammo";
                else if (toType == "4") to = "Heal";
                else if (toType == "5") to = "Boss";
                else if (toType == "6") to = "Goal";
                else
                {
                    Console.WriteLine("Invalid TO room type.");
                    continue;
                }

                from += fromSuffix.Trim();
                to += toSuffix.Trim();

                GameLogic.AddEdge(CustomGraph, from, to);
                Console.WriteLine($"Added: {from} -> {to}");
            }

            return CustomGraph;
        }
    }
}
