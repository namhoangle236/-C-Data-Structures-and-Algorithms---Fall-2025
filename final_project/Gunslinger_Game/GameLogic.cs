using System;
using System.Collections.Generic;
using System.Linq;

namespace Gunslinger_Game
{
    // Simple bullet types, need more? Add them here. Then do logic later (also only allow certain names when creating bullets)
    public enum BulletType
    {
        Normal,
        Heavy,
    }

    // Bullet data type class
    public class Bullet
    {
        public BulletType Type { get; }         // Don't use 'set' here, so other funcs can only read it, not change it
        public int Damage { get; }              // same as above

        public Bullet(BulletType type, int damage)
        {
            Type = type;
            Damage = damage;
        }

        public override string ToString() => $"{Type}({Damage})";       // For displaying bullets instead of default class name when print
    }


    // Player state class
    public class PlayerState
    {
        // Health Points
        public int HP { get; set; } = 12;       // both get and set, so other funcs can read and change HP when needed


        // FIFO -> fires in the same order you gained bullets

        public Queue<Bullet> Chamber { get; } = new Queue<Bullet>(          // only get, so other funcs can read Chamber, but not replace it with a new queue. Can still add or dequeue bullets
            new[]
            {
                new Bullet(BulletType.Normal, 2),
                new Bullet(BulletType.Normal, 2),
                new Bullet(BulletType.Normal, 2),
                new Bullet(BulletType.Normal, 2),
                new Bullet(BulletType.Normal, 3),                           // random crit for fun
                new Bullet(BulletType.Normal, 2),
            }
        );
              

        // Current room name (current vertex in graph)
        public string CurrentRoom { get; set; } = "Start";

        // when isAlive is accessed by a func; IsAlive is evaluated, which run HP > 0 to see if IsAlive is true or false
        public bool IsAlive => HP > 0;
    }

    // ====================================================================================================================


    // Game logic
    public static class GameLogic
    {
        // -------------------------
        // Graph helpers
        // -------------------------

        // Create an empty graph of type Dictionary<string, List<string>>
        // the key is the vertex name, the value is a list of neighboring vertex names
        public static Dictionary<string, List<string>> CreateEmptyGraph()
        {
            return new Dictionary<string, List<string>>();
        }


        // Add vertex to graph
        public static void AddVertex(Dictionary<string, List<string>> graph, string vertex)
        {
            if (!graph.ContainsKey(vertex))                 // only add if no duplicate
                graph[vertex] = new List<string>();         // empty neighbor list (value) for new vertex (key)
        }


        // Add edge to graph, where each vertex leads to (basically add 'to' to the neighbor list of 'from')
        // this also adds the vertices if they don't exist yet
        public static void AddEdge(Dictionary<string, List<string>> graph, string from, string to)
        {
            AddVertex(graph, from);                         // if vertex exists, do nothing since AddVertex checks for duplicates
            AddVertex(graph, to);
            graph[from].Add(to);
        }


        // Retrieve neighbors of a vertex
        public static List<string> GetNeighbors(Dictionary<string, List<string>> graph, string vertex)
        {
            return graph.ContainsKey(vertex) ? graph[vertex] : new List<string>();      // return empty list if vertex not found
        }

        // -------------------------
        // Premade graph
        // -------------------------
        // Visualization of the premade graph:
        /*
                Start
                  |
               Enemy1
              /   |    \
          Heal1  Ammo1  Enemy2
             \    |      |
              Enemy3    Heal2
               \  |     /
                 Ammo2
                   |
                 Boss1
                   |
                 Goal1
        */
        public static Dictionary<string, List<string>> BuildPremadeGraph()
        {
            var g = CreateEmptyGraph();

            // first, fight a dude
            AddEdge(g, "Start", "Enemy1");

            // 3 choices: heal, ammo, or fight another dude
            AddEdge(g, "Enemy1", "Heal1");  // path 1
            AddEdge(g, "Enemy1", "Ammo1");  // path 2
            AddEdge(g, "Enemy1", "Enemy2"); // path 3

            // path 1, 2 gets another fight. Path 3 gets heal
            AddEdge(g, "Heal1", "Enemy3");  // path 1
            AddEdge(g, "Ammo1", "Enemy3");  // path 2
            AddEdge(g, "Enemy2", "Heal2");  // path 3

            // All paths converge to get ammo before final boss
            AddEdge(g, "Enemy3", "Ammo2");  // path 1 & 2 converge here
            AddEdge(g, "Heal2", "Ammo2");

            // Then final boss fight
            AddEdge(g, "Ammo2", "Boss1");

            // Finally, goal room
            AddEdge(g, "Boss1", "Goal1");

            return g;
        }
        

        // -------------------------
        // Player setup
        // -------------------------
        public static PlayerState CreateNewPlayer()
        {
            PlayerState player = new PlayerState();

            // Start with 1 normal bullet
            player.Chamber.Enqueue(new Bullet(BulletType.Normal, 2));

            return player;
        }

        // -------------------------
        // Room logic   (GameUI will handle moving between rooms, this just handles room effects)
        // -------------------------
        public static string EnterRoom(PlayerState player, string roomName)
        {
            player.CurrentRoom = roomName;

            if (roomName == "Start")
                return "You are at the start.";

            if (roomName.StartsWith("Heal"))
            {
                player.HP += 5;
                return "You healed +5 HP.";
            }

            if (roomName.StartsWith("Ammo"))
            {
                player.Chamber.Enqueue(new Bullet(BulletType.Heavy, 4));
                player.Chamber.Enqueue(new Bullet(BulletType.Heavy, 4));
                return "You found ammo: +2 Heavy bullets(4 damage ea).";
            }

            if (roomName.StartsWith("Enemy"))
                return RunFight(player, false);         // false for normal fight

            if (roomName.StartsWith("Boss"))
                return RunFight(player, true);          // true for boss fight

            if (roomName.StartsWith("Goal"))
                return "You reached the goal! You win!";

            return "Nothing happens.";
        }

        // -------------------------
        // Fight logic (player goes first)
        // -------------------------
        private static string RunFight(PlayerState player, bool isBoss)
        {
            int enemyHP;
            int enemyDamage;

            if (isBoss)
            {
                enemyHP = 23;                                       // Boss HP
                enemyDamage = 3;                                    // Boss Damage 
            }
            else
            {
                enemyHP = 5;                                        // Normal Enemy HP
                enemyDamage = 2;                                    // Normal Enemy Damage
            }

            Console.WriteLine(isBoss ? "Boss fight!" : "Enemy fight!");     // announce fight depending on enemy type

            while (enemyHP > 0 && player.IsAlive)
            {
                // Display status and options
                Console.WriteLine();
                Console.WriteLine($"Enemy HP: {enemyHP} | Your HP: {player.HP} | Bullets: {player.Chamber.Count}");
                Console.WriteLine("1) 🔫Shoot  2) ♻️Reload(+2 Normal bullets)  3) Rapid Fire(💀let it rip!💀)");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                // Player turn
                if (choice == "1")
                {
                    if (player.Chamber.Count == 0)
                    {
                        Console.WriteLine("❌Click... no bullets!");
                    }
                    else
                    {
                        Bullet bullet = player.Chamber.Dequeue();
                        enemyHP -= bullet.Damage;
                        Console.WriteLine($"💥{bullet} bullet fired! Enemy takes {bullet.Damage} damage.");
                    }
                }
                else if (choice == "2")
                {
                    player.Chamber.Enqueue(new Bullet(BulletType.Normal, 2));
                    player.Chamber.Enqueue(new Bullet(BulletType.Normal, 2));
                    Console.WriteLine("Reloaded: +2 Normal bullets.");
                }
                else if (choice == "3")
                {
                    if (player.Chamber.Count == 0)
                    {
                        Console.WriteLine("No bullets to rapid fire!");
                    }
                    else
                    {
                        int totalDamage = 0;

                        while (player.Chamber.Count > 0)
                        {
                            Bullet b = player.Chamber.Dequeue();
                            totalDamage += b.Damage;
                        }

                        enemyHP -= totalDamage;
                        Console.WriteLine($"💥Rapid fire!💥💥 Total damage: {totalDamage}.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Try 1, 2, or 3.");
                    continue; // don't let enemy attack if player entered invalid input         << chatGPT my GOAT! a simple line to fix the bug
                }

                // If enemy is dead, stop before enemy attacks
                if (enemyHP <= 0)
                {
                    Console.WriteLine("--------------------");
                    return isBoss ? "You defeated the boss!" : "You defeated the enemy!";
                }
                // Enemy turn
                player.HP -= enemyDamage;
                Console.WriteLine($"Enemy hits you for {enemyDamage} damage! (Your HP: {player.HP})");

                Console.WriteLine("-----------------------------------");
            }

            if (!player.IsAlive)
            {
                Console.WriteLine("-----------------------------------");
                return "You were defeated.";
            }
            return isBoss ? "You defeated the boss!" : "You defeated the enemy!";
        }

    }
}
