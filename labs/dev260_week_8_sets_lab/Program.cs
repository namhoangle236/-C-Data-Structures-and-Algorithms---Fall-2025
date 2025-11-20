using System;
using System.Collections.Generic;
using System.Linq;

// ============================================
// 📚 QUICK REFERENCE GUIDE
// ============================================

/*
🎯 HASHSET CHEAT SHEET:

Creation:
var set = new HashSet<string>();
var caseInsensitive = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

Adding Data:
set.Add(item);                     // Returns true if added, false if already exists

Safe Checking:
if (set.Contains(item))
{
    // item exists in set
}

Removing:
set.Remove(item);                  // Returns true if removed, false if not found

Getting All Data:
set.Count                          // Number of unique items
foreach (var item in set) { }      // Iterate through items

Set Operations:
set1.UnionWith(set2);             // Add all items from set2 to set1
set1.IntersectWith(set2);         // Keep only items in both sets
set1.ExceptWith(set2);            // Remove items that are in set2
set1.IsSubsetOf(set2);            // Check if all items in set1 are also in set2

🌟 WHY HASHSETS ROCK:
- O(1) Contains() - instant membership testing!
- Automatic duplicate prevention
- Perfect for permissions and authorization
- Ideal for data deduplication and uniqueness
- Set operations for logical comparisons

🌐 REAL-WORLD USES:
- Email deduplication systems
- User permission and role management
- Social media (mutual friends, common interests)
- Analytics (unique visitors, event tracking)
- Content filtering and categorization
- Data processing pipelines
*/

namespace Lab8_Sets
{
    /// <summary>
    /// Lab 8: Set Operations - Student Version
    /// 
    /// This lab demonstrates HashSet<T> fundamentals through real-world scenarios!
    /// You'll build systems that work like real deduplication and permission APIs.
    /// 
    /// 🌟 Real-World Connection:
    /// Every time you see "Remove Duplicates" or check permissions in an app,
    /// you're seeing HashSet operations in action!
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🎯 Welcome to Lab 8: Set Operations in C#");
            Console.WriteLine("==========================================");
            Console.WriteLine("Today we'll explore HashSet<T> through real-world scenarios!\n");

            var lab = new SetOperationsLab();
            lab.RunInteractiveMenu();
        }
    }

    public class SetOperationsLab
    {
        // 🎯 THE CORE: HashSets for O(1) operations and automatic uniqueness
        // This is exactly how deduplication and permission systems work!
        private HashSet<string> uniqueEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, HashSet<string>> userPermissions = new Dictionary<string, HashSet<string>>();
        private HashSet<string> enrolledNow = new HashSet<string>();
        private HashSet<string> enrolledLastQuarter = new HashSet<string>();

        // 📊 System tracking (real systems do this too!)
        private int totalOperations = 0;
        private DateTime systemStartTime = DateTime.Now;

        public SetOperationsLab()
        {
            // 🎭 Start with some demo data - like a real system with test data
            LabSupport.LoadDemoData(this);
            Console.WriteLine("🎯 Set Operations Lab Initialized!");
            Console.WriteLine($"📊 System ready with demo data loaded.");
            Console.WriteLine("\n💡 Fun Fact: This system uses HashSet<T> for O(1) operations!");
            Console.WriteLine("   Real deduplication and permission systems use the same patterns.\n");
        }

        // ============================================
        // 🚀 YOUR MISSION: IMPLEMENT THESE METHODS
        // ============================================

        /// <summary>
        /// TODO #1: Deduplicate an email list
        /// 
        /// Real-World Connection: This is like cleaning a mailing list or user database
        /// 
        /// Requirements:
        /// - Take a list of emails that may have duplicates
        /// - Create case-insensitive HashSet to remove duplicates
        /// - Return the count of duplicates removed
        /// - Display before/after statistics
        /// 
        /// 🔑 Key Learning: HashSet automatically prevents duplicates!
        /// </summary>
        public int DeduplicateEmails(List<string> emailList)
        {
            totalOperations++;

            // Create a case-insensitive HashSet
            HashSet<string> set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Add emails into the set
            foreach (string email in emailList)
            {
                set.Add(email);
            }

            // Find how many duplicates were removed
            int duplicatesRemoved = emailList.Count - set.Count;

            // Replace original list content with unique values
            emailList.Clear();
            foreach (string email in set)
            {
                emailList.Add(email);
            }

            return duplicatesRemoved;
        }


        /// <summary>
        /// TODO #2: Check if user has specific permission
        /// 
        /// Real-World Connection: This is like checking if a user can access a feature
        /// 
        /// Requirements:
        /// - Check if user exists in permission system
        /// - Use Contains() to check for specific permission
        /// - Return true if user has permission, false otherwise
        /// - Handle case where user doesn't exist
        /// 
        /// 🚀 Key Learning: O(1) permission checking with Contains()!
        /// </summary>
        public bool HasPermission(string userId, string permission)
        {
            totalOperations++;

            // Check if user exists
            if (!userPermissions.ContainsKey(userId))
            {
                return false;
            }

            // Check if user has the permission
            HashSet<string> perms = userPermissions[userId];
            return perms.Contains(permission);
        }


        /// <summary>
        /// TODO #3: Add permissions to a user
        /// 
        /// Real-World Connection: This is like granting new permissions to a user role
        /// 
        /// Requirements:
        /// - Create user permission set if it doesn't exist
        /// - Use UnionWith() to add new permissions
        /// - Return the number of NEW permissions added
        /// - Handle both new and existing users
        /// 
        /// 💡 Key Learning: UnionWith() merges sets without duplicates!
        /// </summary>
        public int AddPermissions(string userId, HashSet<string> newPermissions)
        {
            totalOperations++;

            // Create user entry if not found
            if (!userPermissions.ContainsKey(userId))
            {
                userPermissions[userId] = new HashSet<string>();
            }

            HashSet<string> current = userPermissions[userId];
            int beforeCount = current.Count;

            // Add permissions without duplicates
            current.UnionWith(newPermissions);

            int afterCount = current.Count;
            int added = afterCount - beforeCount;

            return added;
        }


        /// <summary>
        /// TODO #4: Check if user has all required permissions
        /// 
        /// Real-World Connection: This is like validating access to a secure feature
        /// 
        /// Requirements:
        /// - Check if user exists
        /// - Use IsSubsetOf() to verify all required permissions
        /// - Return missing permissions if not authorized
        /// - Return empty set if fully authorized
        /// 
        /// 🎯 Key Learning: IsSubsetOf() checks if one set is contained in another!
        /// </summary>
        public HashSet<string> GetMissingPermissions(string userId, HashSet<string> requiredPermissions)
        {
            totalOperations++;

            HashSet<string> missing = new HashSet<string>();

            // If user doesn't exist, all required permissions are missing
            if (!userPermissions.ContainsKey(userId))
            {
                foreach (string perm in requiredPermissions)
                {
                    missing.Add(perm);
                }
                return missing;
            }

            HashSet<string> userPerms = userPermissions[userId];

            // Find permissions required but not in user's set
            foreach (string perm in requiredPermissions)
            {
                if (!userPerms.Contains(perm))
                {
                    missing.Add(perm);
                }
            }

            return missing;
        }


        /// <summary>
        /// TODO #5: Find new students (enrolled now but not last quarter)
        /// 
        /// Real-World Connection: This is like finding new sign-ups between time periods
        /// 
        /// Requirements:
        /// - Use ExceptWith() to find students in current but not previous enrollment
        /// - Return set of new student emails
        /// - Don't modify the original enrollment sets
        /// 
        /// 📊 Key Learning: ExceptWith() finds differences between sets!
        /// </summary>
        public HashSet<string> FindNewStudents()
        {
            totalOperations++;

            // Copy current quarter
            HashSet<string> result = new HashSet<string>(enrolledNow);

            // Remove students who were also in last quarter
            result.ExceptWith(enrolledLastQuarter);

            return result;
        }


        /// <summary>
        /// TODO #6: Find dropped students (enrolled last quarter but not now)
        /// 
        /// Real-World Connection: This is like finding users who cancelled subscriptions
        /// 
        /// Requirements:
        /// - Use ExceptWith() to find students in previous but not current enrollment  
        /// - Return set of dropped student emails
        /// - Don't modify the original enrollment sets
        /// 
        /// 🚪 Key Learning: Set operations work both ways for different insights!
        /// </summary>
        public HashSet<string> FindDroppedStudents()
        {
            totalOperations++;

            // Copy last quarter
            HashSet<string> result = new HashSet<string>(enrolledLastQuarter);

            // Remove students who are in current quarter
            result.ExceptWith(enrolledNow);

            return result;
        }


        /// <summary>
        /// TODO #7: Find continuing students (enrolled both quarters)
        /// 
        /// Real-World Connection: This is like finding loyal customers or active users
        /// 
        /// Requirements:
        /// - Use IntersectWith() to find students in both enrollments
        /// - Return set of continuing student emails
        /// - Don't modify the original enrollment sets
        /// 
        /// 🔄 Key Learning: IntersectWith() finds common elements between sets!
        /// </summary>
        public HashSet<string> FindContinuingStudents()
        {
            totalOperations++;

            // Copy current quarter
            HashSet<string> result = new HashSet<string>(enrolledNow);

            // Keep only those also in last quarter
            result.IntersectWith(enrolledLastQuarter);

            return result;
        }


        /// <summary>
        /// TODO #8: Calculate retention rate
        /// 
        /// Real-World Connection: This is like calculating customer retention in analytics
        /// 
        /// Requirements:
        /// - Find continuing students (intersection)
        /// - Calculate percentage: (continuing / lastQuarter) * 100
        /// - Return as double percentage (0.0 to 100.0)
        /// - Handle edge case where lastQuarter is empty
        /// 
        /// 📈 Key Learning: Set operations enable powerful analytics calculations!
        /// </summary>
        public double CalculateRetentionRate()
        {
            totalOperations++;

            // Avoid division by zero
            if (enrolledLastQuarter.Count == 0)
            {
                return 0.0;
            }

            HashSet<string> continuing = FindContinuingStudents();

            double continuingCount = continuing.Count;
            double lastCount = enrolledLastQuarter.Count;

            double rate = (continuingCount / lastCount) * 100.0;

            return rate;
        }


        public void RunInteractiveMenu()
        {
            LabSupport.RunInteractiveMenu(this);
        }

        // ============================================
        // 🔧 HELPER METHODS FOR EXTERNAL HANDLERS
        // ============================================

        public Dictionary<string, HashSet<string>> GetUserPermissions()
        {
            return userPermissions;
        }

        public (HashSet<string> enrolledNow, HashSet<string> enrolledLastQuarter) GetEnrollmentData()
        {
            return (enrolledNow, enrolledLastQuarter);
        }

        public SystemStats GetSystemStats()
        {
            var uptime = DateTime.Now - systemStartTime;
            return new SystemStats
            {
                TotalOperations = totalOperations,
                Uptime = uptime,
                UniqueEmailsCount = uniqueEmails.Count,
                UsersWithPermissionsCount = userPermissions.Count,
                ThisQuarterEnrollment = enrolledNow.Count,
                LastQuarterEnrollment = enrolledLastQuarter.Count
            };
        }

        public void InitializeUniqueEmails(List<string> emails)
        {
            foreach (var email in emails)
            {
                uniqueEmails.Add(email); // Will automatically deduplicate
            }
        }

        public void InitializeUserPermissions(Dictionary<string, HashSet<string>> permissions)
        {
            foreach (var kvp in permissions)
            {
                userPermissions[kvp.Key] = kvp.Value;
            }
        }

        public void InitializeEnrollmentData(HashSet<string> thisQuarter, HashSet<string> lastQuarter)
        {
            enrolledNow.UnionWith(thisQuarter);
            enrolledLastQuarter.UnionWith(lastQuarter);
        }
    }
}