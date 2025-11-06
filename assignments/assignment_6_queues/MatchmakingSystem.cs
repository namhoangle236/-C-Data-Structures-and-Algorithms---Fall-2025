namespace Assignment6
{
    /// <summary>
    /// Main matchmaking system managing queues and matches
    /// Students implement the core methods in this class
    /// </summary>
    public class MatchmakingSystem
    {
        // Data structures for managing the matchmaking system
        private Queue<Player> casualQueue = new Queue<Player>();
        private Queue<Player> rankedQueue = new Queue<Player>();
        private Queue<Player> quickPlayQueue = new Queue<Player>();
        private List<Player> allPlayers = new List<Player>();

        private List<Match> matchHistory = new List<Match>();

        // Statistics tracking
        private int totalMatches = 0;
        private DateTime systemStartTime = DateTime.Now;

        /// <summary>
        /// Create a new player and add to the system
        /// </summary>
        public Player CreatePlayer(string username, int skillRating, GameMode preferredMode = GameMode.Casual)
        {
            // Check for duplicate usernames
            if (allPlayers.Any(p => p.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Player with username '{username}' already exists");
            }

            var player = new Player(username, skillRating, preferredMode);
            allPlayers.Add(player);
            return player;
        }

        /// <summary>
        /// Get all players in the system
        /// </summary>
        public List<Player> GetAllPlayers() => allPlayers.ToList();

        /// <summary>
        /// Get match history
        /// </summary>
        public List<Match> GetMatchHistory() => matchHistory.ToList();

        /// <summary>
        /// Get system statistics
        /// </summary>
        public string GetSystemStats()
        {
            var uptime = DateTime.Now - systemStartTime;
            var avgMatchQuality = matchHistory.Count > 0 
                ? matchHistory.Average(m => m.SkillDifference) 
                : 0;

            return $"""
                🎮 Matchmaking System Statistics
                ================================
                Total Players: {allPlayers.Count}
                Total Matches: {totalMatches}
                System Uptime: {uptime.ToString("hh\\:mm\\:ss")}
                
                Queue Status:
                - Casual: {casualQueue.Count} players
                - Ranked: {rankedQueue.Count} players  
                - QuickPlay: {quickPlayQueue.Count} players
                
                Match Quality:
                - Average Skill Difference: {avgMatchQuality:F1}
                - Recent Matches: {Math.Min(5, matchHistory.Count)}
                """;
        }

        // ============================================
        // STUDENT IMPLEMENTATION METHODS (TO DO)
        // ============================================

        /// <summary>
        /// TODO: Add a player to the appropriate queue based on game mode
        /// 
        /// Requirements:
        /// - Add player to correct queue (casualQueue, rankedQueue, or quickPlayQueue)
        /// - Call player.JoinQueue() to track queue time
        /// - Handle any validation needed
        /// </summary>
        public void AddToQueue(Player player, GameMode mode)
        {
            // TODO: Implement this method
            // Hint: Use switch statement on mode to select correct queue
            // Don't forget to call player.JoinQueue()!

            if (player == null)
            {
                Console.WriteLine("❌ Cannot add null player to queue.");
                return;
            }

            player.JoinQueue();

            switch (mode)
            {
                case GameMode.Casual:
                    casualQueue.Enqueue(player);
                    Console.WriteLine($"🟢 {player.Username} joined the Casual queue.");
                    break;
                case GameMode.Ranked:
                    rankedQueue.Enqueue(player);
                    Console.WriteLine($"🟡 {player.Username} joined the Ranked queue.");
                    break;
                case GameMode.QuickPlay:
                    quickPlayQueue.Enqueue(player);
                    Console.WriteLine($"🔵 {player.Username} joined the QuickPlay queue.");
                    break;
                default:
                    Console.WriteLine("❌ Unknown game mode.");
                    break;
            }
        }


        /// <summary>
        /// TODO: Try to create a match from the specified queue
        /// 
        /// Requirements:
        /// - Return null if not enough players (need at least 2)
        /// - For Casual: Any two players can match (simple FIFO)
        /// - For Ranked: Only players within ±2 skill levels can match
        /// - For QuickPlay: Prefer skill matching, but allow any match if queue > 4 players
        /// - Remove matched players from queue and call LeaveQueue() on them
        /// - Return new Match object if successful
        /// </summary>
        public Match? TryCreateMatch(GameMode mode)
        {
            // TODO: Implement this method
            // Hint: Different logic needed for each mode
            // Remember to check queue count first!

            var queue = GetQueueByMode(mode);

            if (queue.Count < 2)
            {
                Console.WriteLine($"⚠️ Not enough players in {mode} queue to create a match.");
                return null;
            }

            Player? p1 = null;          // Not creating a player, just to hold the dequeued players from the queue
            Player? p2 = null;          // same as above

            switch (mode)
            {
                case GameMode.Casual:
                    p1 = queue.Dequeue();
                    p2 = queue.Dequeue();
                    break;

                case GameMode.Ranked:
                    p1 = queue.Peek();
                    var candidates = queue.Skip(1).ToList();            // skip the first player since it's p1, then convert the rest to a list (of candidates)
                    p2 = candidates.FirstOrDefault(p => CanMatchInRanked(p1, p));       // for each p in candidates, calls 'CanMatchInRanked' helper function, will return null if no 'true' found, thanks to FirstOrDefault C# function, if there's a 'true', return THAT PLAYER as p2 (not the boolean)
                    
                    if (p2 != null)                         // found a match if p2 is not null
                    {
                        queue.Dequeue(); // remove p1 -> no need to store since we already have p1 from Peek()
                        queue = new Queue<Player>(queue.Where(p => p != p2)); // rebuild without p2
                    }
                    else
                    {
                        Console.WriteLine("⚠️ No suitable skill match found in Ranked queue.");
                        return null;
                    }
                    break;

                case GameMode.QuickPlay:
                    if (queue.Count > 4)
                    {
                        p1 = queue.Dequeue();
                        p2 = queue.Dequeue();
                    }
                    else
                    {
                        p1 = queue.Peek();
                        var quickCandidates = queue.Skip(1).ToList();
                        p2 = quickCandidates.FirstOrDefault(p => Math.Abs(p.SkillRating - p1.SkillRating) <= 2);
                        if (p2 != null)
                        {
                            queue.Dequeue();
                            queue = new Queue<Player>(queue.Where(p => p != p2));
                        }
                        else
                        {
                            Console.WriteLine("⚠️ No ideal skill match in QuickPlay yet.");
                            return null;
                        }
                    }
                    break;
            }

            if (p1 == null || p2 == null)
                return null;

            p1.LeaveQueue();
            p2.LeaveQueue();

            var match = new Match(p1, p2, mode);
            return match;
        }

        /// <summary>
        /// TODO: Process a match by simulating outcome and updating statistics
        /// 
        /// Requirements:
        /// - Call match.SimulateOutcome() to determine winner
        /// - Add match to matchHistory
        /// - Increment totalMatches counter
        /// - Display match results to console
        /// </summary>
        public void ProcessMatch(Match match)
        {
            // TODO: Implement this method
            // Hint: Very straightforward - simulate, record, display

            if (match == null)
            {
                Console.WriteLine("❌ Cannot process a null match.");
                return;
            }

            match.SimulateOutcome();
            matchHistory.Add(match);
            totalMatches++;

            Console.WriteLine($"\n🏆 Match Completed! {match.Winner?.Username} defeated {match.Loser?.Username}");
            Console.WriteLine(match.ToDetailedString());
        }

        /// <summary>
        /// TODO: Display current status of all queues with formatting
        /// 
        /// Requirements:
        /// - Show header "Current Queue Status"
        /// - For each queue (Casual, Ranked, QuickPlay):
        ///   - Show queue name and player count
        ///   - List players with position numbers and queue times
        ///   - Handle empty queues gracefully
        /// - Use proper formatting and emojis for readability
        /// </summary>
        public void DisplayQueueStatus()
        {
            // TODO: Implement this method
            // Hint: Loop through each queue and display formatted information

            Console.WriteLine("\n📋 Current Queue Status");
            Console.WriteLine("========================");

            DisplaySingleQueue("🟢 Casual", casualQueue);
            DisplaySingleQueue("🟡 Ranked", rankedQueue);
            DisplaySingleQueue("🔵 QuickPlay", quickPlayQueue);
        }

        private void DisplaySingleQueue(string label, Queue<Player> queue)
        {
            Console.WriteLine($"\n{label} Queue: {queue.Count} players");

            if (queue.Count == 0)
            {
                Console.WriteLine("  (empty)");
                return;
            }

            int position = 1;
            foreach (var p in queue)
            {
                Console.WriteLine($"  {position:D2}. {p.Username} - Wait: {p.GetQueueTime()}");
                position++;
            }
        }
        public void DisplayPlayerStats(Player player)
        {
            // TODO: Implement this method
            // Hint: Combine player info with match history filtering

            Console.WriteLine(player.ToDetailedString());

            bool inQueue = casualQueue.Contains(player) || rankedQueue.Contains(player) || quickPlayQueue.Contains(player);
            if (inQueue)
            {
                Console.WriteLine($"🕒 Currently waiting in queue | Estimated wait: {GetQueueEstimate(player.PreferredMode)}");
            }
            else
            {
                Console.WriteLine("✅ Not currently in a queue.");
            }

            var recentMatches = matchHistory
                .Where(m => m.Winner == player || m.Loser == player)
                .TakeLast(3)
                .ToList();

            if (recentMatches.Count == 0)
            {
                Console.WriteLine("📭 No match history yet.");
            }
            else
            {
                Console.WriteLine("\n📜 Recent Matches:");
                foreach (var match in recentMatches)
                {
                    Console.WriteLine($"  - {match.GetSummary()}");
                }
            }
        }

        /// <summary>
        /// TODO: Calculate estimated wait time for a queue
        /// 
        /// Requirements:
        /// - Return "No wait" if queue has 2+ players
        /// - Return "Short wait" if queue has 1 player
        /// - Return "Long wait" if queue is empty
        /// - For Ranked: Consider skill distribution (harder to match = longer wait)
        /// </summary>
        public string GetQueueEstimate(GameMode mode)
        {
            // TODO: Implement this method
            // Hint: Check queue counts and apply mode-specific logic

            var queue = GetQueueByMode(mode);
            int count = queue.Count;

            if (mode == GameMode.Ranked)
            {
                if (count >= 4) return "No wait";
                if (count >= 2) return "Short wait (depends on skill range)";
                return "Long wait (few ranked players)";
            }

            if (count >= 2) return "No wait";
            if (count == 1) return "Short wait";
            return "Long wait";
        }

        // ============================================
        // HELPER METHODS (PROVIDED)
        // ============================================

        /// <summary>
        /// Helper: Check if two players can match in Ranked mode (±2 skill levels)
        /// </summary>
        private bool CanMatchInRanked(Player player1, Player player2)
        {
            return Math.Abs(player1.SkillRating - player2.SkillRating) <= 2;
        }

        /// <summary>
        /// Helper: Remove player from all queues (useful for cleanup)
        /// </summary>
        private void RemoveFromAllQueues(Player player)
        {
            // Create temporary lists to avoid modifying collections during iteration
            var casualPlayers = casualQueue.ToList();
            var rankedPlayers = rankedQueue.ToList();
            var quickPlayPlayers = quickPlayQueue.ToList();

            // Clear and rebuild queues without the specified player
            casualQueue.Clear();
            foreach (var p in casualPlayers.Where(p => p != player))
                casualQueue.Enqueue(p);

            rankedQueue.Clear();
            foreach (var p in rankedPlayers.Where(p => p != player))
                rankedQueue.Enqueue(p);

            quickPlayQueue.Clear();
            foreach (var p in quickPlayPlayers.Where(p => p != player))
                quickPlayQueue.Enqueue(p);

            player.LeaveQueue();
        }

        /// <summary>
        /// Helper: Get queue by mode (useful for generic operations)
        /// </summary>
        private Queue<Player> GetQueueByMode(GameMode mode)
        {
            return mode switch
            {
                GameMode.Casual => casualQueue,
                GameMode.Ranked => rankedQueue,
                GameMode.QuickPlay => quickPlayQueue,
                _ => throw new ArgumentException($"Unknown game mode: {mode}")
            };
        }
    }
}