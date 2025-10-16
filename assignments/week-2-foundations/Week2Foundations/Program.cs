
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


// Micro-task

static void RunArrayDemo()
{
    int[] numbers = new int[10];
    numbers[0] = 111;
    numbers[1] = 222;
    numbers[2] = 333;

    Console.WriteLine("----------------- A. Array -----------------");

    // Print index 2
    Console.WriteLine($"The number at index 2 is: {numbers[2]}");

    // Linearly search for a value
    int searchValue = 222;
    bool found = false;
    for (int i = 0; i < numbers.Length; i++)
    {
        if (numbers[i] == searchValue)
        {
            found = true;
            break;
        }
    }

    // Print whether found
    Console.WriteLine($"Checking if {searchValue} exists --> {found}.");
}

//-----------------------------------------------------------------------------

static void RunListDemo()
{
    // Start with 1–5
    List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

    Console.WriteLine("----------------- B. List<T> -----------------");

    // print original list
    Console.WriteLine($"Original list: {string.Join(", ", numbers)}");
    // Insert 99 at index 2
    numbers.Insert(2, 99);
    Console.WriteLine($"List after inserting 99 at index 2: {string.Join(", ", numbers)}");
    // Then remove it, print final Count
    numbers.Remove(99);
    Console.WriteLine($"After removing 99, final Count: {numbers.Count}");
}

//-----------------------------------------------------------------------------

static void RunStackDemo()
{
    Stack<string> history = new Stack<string>();

    Console.WriteLine("----------------- C. Stack<T> -----------------");

    // Push three page URLs
    history.Push("google.com");
    history.Push("csharp.net");
    history.Push("currentpage.com");

    // Peek the current page
    string currentPage = history.Peek();
    Console.WriteLine($"Current page (Peek): {currentPage}");

    // Pop all to simulate “Back” navigation; print order visited
    Console.Write("Pages visited (Popped): ");
    while (history.Count > 0)
    {
        Console.Write($"{history.Pop()}" + (history.Count > 0 ? " -> " : ""));
    }
    Console.WriteLine();
}

//-----------------------------------------------------------------------------

static void RunQueueDemo()
{
    Queue<string> printJobs = new Queue<string>();

    Console.WriteLine("----------------- D. Queue<T> -----------------");

    // Enqueue three “print jobs”
    printJobs.Enqueue("DocumentA.pdf");
    printJobs.Enqueue("ReportB.docx");
    printJobs.Enqueue("ImageC.jpg");

    // Peek the next job
    string nextJob = printJobs.Peek();
    Console.WriteLine($"Next job to process (Peek): {nextJob}");

    // Dequeue all; print processing order (FIFO)
    Console.Write("Jobs processed (Dequeued): ");
    while (printJobs.Count > 0)
    {
        Console.Write($"{printJobs.Dequeue()}" + (printJobs.Count > 0 ? " -> " : ""));
    }
    Console.WriteLine();
}

//-----------------------------------------------------------------------------

static void RunDictionaryDemo()
{
    // Map three SKUs to quantities
    Dictionary<string, int> inventory = new Dictionary<string, int>
    {
        { "SKU100", 50 },
        { "SKU200", 25 },
        { "SKU300", 75 }
    };

    Console.WriteLine("----------------- E. Dictionary<TKey,TValue> -----------------");

    // Update one quantity
    string updateSku = "SKU100";
    inventory[updateSku] += 10;
    Console.WriteLine($"Updated {updateSku} quantity to: {inventory[updateSku]}");

    // Show TryGetValue("missing") result
    string missingSku = "SKU999";
    int quantity;
    bool found = inventory.TryGetValue(missingSku, out quantity);
    Console.WriteLine($"TryGetValue('{missingSku}') result: {found}");
}

//-----------------------------------------------------------------------------

static void RunHashSetDemo()
{
    HashSet<int> set = new HashSet<int>();

    Console.WriteLine("----------------- F. HashSet<T> -----------------");

    // Add a few integers with duplicates; show whether adding a duplicate returns false.
    Console.WriteLine("Adding 1, 2, 3:");
    set.Add(1);
    set.Add(2);
    set.Add(3);
    
    // Attempt to add a duplicate (3)
    bool addedDuplicate = set.Add(3);
    Console.WriteLine($"Attempting to add 3 again: Success={addedDuplicate} (should be False)");
    Console.WriteLine($"Current set: {string.Join(", ", set)}");

    // Perform a UnionWith on {3, 4, 5}
    List<int> unionValues = new List<int> { 3, 4, 5 };
    set.UnionWith(unionValues);

    // Print final Count.
    Console.WriteLine($"Performing UnionWith {{3, 4, 5}}.");
    Console.WriteLine($"Final set: {string.Join(", ", set)}");
    Console.WriteLine($"Final Count: {set.Count}");
}


//-----------------------------------------------------------------------------

static void RunBenchmarks()
{
    Console.WriteLine("----------------- G. Benchmarks -----------------");
    Console.WriteLine("Comparing O(n) List search vs O(1) HashSet/Dictionary membership checks.");

    // Ns to test
    // Added 250,000 for a better view of scaling.
    int[] Ns = { 1000, 10000, 100000, 250000 };
    const int Repetitions = 5; // Run each check this many times
    const int SearchValuePresent = -2; // Placeholder for the value N-1
    const int SearchValueMissing = -1; // The missing element

    Stopwatch sw = new Stopwatch();

    foreach (int N in Ns)
    {
        // 1. Prepare Data Sets (0 to N-1)
        List<int> list = Enumerable.Range(0, N).ToList();
        HashSet<int> hashSet = new HashSet<int>(list);
        Dictionary<int, bool> dictionary = list.ToDictionary(k => k, v => true); // Value doesn't matter

        // Set the actual present search value
        int presentValue = N - 1;

        Console.WriteLine($"\nN={N}");

        // Array to store times for median calculation
        List<double> times = new List<double>(Repetitions);

        // --- Helper function to run and time a check multiple times ---
        Func<Action, double> RunTimedCheck = (action) =>
        {
            times.Clear();
            // Simple "warm-up" run (ignore time)
            action();

            for (int i = 0; i < Repetitions; i++)
            {
                sw.Restart();
                action();
                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds);
            }

            // NOTE: We'll report the best (minimum) time for simplicity
            return times.Min();
        };

        // --- 1. Present Value (N-1) ---

        // List.Contains(N-1) -> O(n) worst case
        double listPresentTime = RunTimedCheck(() => list.Contains(presentValue));
        Console.WriteLine($"List.Contains({presentValue}): {listPresentTime:F3} ms");

        // HashSet.Contains(N-1) -> O(1) average
        double hashSetPresentTime = RunTimedCheck(() => hashSet.Contains(presentValue));
        Console.WriteLine($"HashSet.Contains:     {hashSetPresentTime:F3} ms");

        // Dict.ContainsKey(N-1) -> O(1) average
        double dictPresentTime = RunTimedCheck(() => dictionary.ContainsKey(presentValue));
        Console.WriteLine($"Dict.ContainsKey:     {dictPresentTime:F3} ms");

        // --- 2. Missing Value (-1) ---

        // List.Contains(-1) -> O(n) (must check every element)
        double listMissingTime = RunTimedCheck(() => list.Contains(SearchValueMissing));
        Console.WriteLine($"List.Contains({SearchValueMissing}):    {listMissingTime:F3} ms");

        // HashSet.Contains(-1) -> O(1) average
        double hashSetMissingTime = RunTimedCheck(() => hashSet.Contains(SearchValueMissing));
        Console.WriteLine($"HashSet.Contains(-1): {hashSetMissingTime:F3} ms");

        // Dict.ContainsKey(-1) -> O(1) average
        double dictMissingTime = RunTimedCheck(() => dictionary.ContainsKey(SearchValueMissing));
        Console.WriteLine($"Dict.ContainsKey(-1): {dictMissingTime:F3} ms");
    }
}

//-----------------------------------------------------------------------------
// Run demos
RunArrayDemo();
RunListDemo();
RunStackDemo();
RunQueueDemo();
RunDictionaryDemo();
RunHashSetDemo();
RunBenchmarks();