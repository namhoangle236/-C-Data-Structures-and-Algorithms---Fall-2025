using System;
using System.Collections.Generic;
using System.IO;

// ----------------------------
// Data Model
// ----------------------------
class Book
{
    public string Original { get; set; }      // original text for display
    public string Normalized { get; set; }    // normalized for sorting/comparison

    public Book(string title)
    {
        Original = title;
        Normalized = title.Trim().ToUpperInvariant();
    }
}

// ----------------------------
// Book Catalog
// ----------------------------
class BookCatalog
{
    private static int[,] startIndex;   // index for start positions, containing more 2nd array
    private static int[,] endIndex;    // index for end positions, containing books for each 2nd array
    private static List<Book> books;

    // ----------------------------
    // Entry point
    // ----------------------------
    public static void Run()
    {
        // Load books
        books = Load("books.txt");
        if (books.Count == 0)
        {
            Console.WriteLine("No books found.");
            return;
        }

        // Sort using QuickSort
        QuickSort(books, 0, books.Count - 1);

        // Build 2D prefix index
        BuildIndex(books, out startIndex, out endIndex);

        // Interactive prompt
        while (true)
        {
            Console.Write("\nEnter a book title (or 'exit'): ");
            string input = Console.ReadLine();
            if (input == null || input.Trim().ToLowerInvariant() == "exit")
                break;

            string queryNorm = input.Trim().ToUpperInvariant();

            var (s, e) = Lookup(queryNorm);

            if (s == -1 || e == -1)
            {
                Console.WriteLine("No match. Suggestions:");
                ShowGlobalSuggestions(queryNorm, 5);
            }
            else
            {
                int idx = BinarySearchSlice(s, e, queryNorm);
                if (idx != -1)
                {
                    Console.WriteLine($"Exact match found: {books[idx].Original}");
                }
                else
                {
                    Console.WriteLine("No exact match. Suggestions:");
                    ShowSliceSuggestions(s, e, queryNorm, 5);
                }
            }
        }
    }

    // ----------------------------
    // Load books from file
    // ----------------------------
    static List<Book> Load(string filePath)
    {
        var list = new List<Book>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found: " + filePath);
            return list;
        }

        string[] lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            list.Add(new Book(line));
        }

        return list;
    }

    // ----------------------------
    // QuickSort Section
    // ----------------------------
    static void QuickSort(List<Book> books, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(books, left, right);
            QuickSort(books, left, pivotIndex - 1);
            QuickSort(books, pivotIndex + 1, right);
        }
    }

    static int Partition(List<Book> books, int left, int right)
    {
        var pivot = books[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (books[j].Normalized.CompareTo(pivot.Normalized) < 0)
            {
                i++;
                Swap(books, i, j);
            }
        }

        Swap(books, i + 1, right);
        return i + 1;
    }

    static void Swap(List<Book> books, int i, int j)
    {
        var temp = books[i];
        books[i] = books[j];
        books[j] = temp;
    }

    // ----------------------------
    // Letter to index helper
    // ----------------------------
    static int LetterIndex(char c)
    {
        char upper = char.ToUpperInvariant(c);
        if (upper >= 'A' && upper <= 'Z')
            return upper - 'A';
        return 0;
    }

    // ----------------------------
    // Build 2D prefix index
    // ----------------------------
    static void BuildIndex(List<Book> books, out int[,] start, out int[,] end)
    {
        start = new int[26, 26];
        end = new int[26, 26];

        for (int i = 0; i < 26; i++)
            for (int j = 0; j < 26; j++)
                start[i, j] = end[i, j] = -1;

        for (int i = 0; i < books.Count; i++)
        {
            string norm = books[i].Normalized;
            int first = LetterIndex(norm.Length > 0 ? norm[0] : 'A');
            int second = LetterIndex(norm.Length > 1 ? norm[1] : 'A');

            if (start[first, second] == -1)
                start[first, second] = i;
            end[first, second] = i + 1; // exclusive
        }
    }

    // ----------------------------
    // Lookup by prefix using index
    // ----------------------------
    static (int, int) Lookup(string prefix)
    {
        int first = LetterIndex(prefix.Length > 0 ? prefix[0] : 'A');
        int second = LetterIndex(prefix.Length > 1 ? prefix[1] : 'A');

        int s = startIndex[first, second];
        int e = endIndex[first, second];

        if (s == -1 || e == -1)
            return (-1, -1);

        return (s, e);
    }

    // ----------------------------
    // Binary search within slice for exact match
    // ----------------------------
    static int BinarySearchSlice(int s, int e, string queryNorm)
    {
        int left = s;
        int right = e - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int cmp = books[mid].Normalized.CompareTo(queryNorm);
            if (cmp == 0) return mid;
            else if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }

        return -1;
    }

    // ----------------------------
    // Suggestions from slice
    // ----------------------------
    static void ShowSliceSuggestions(int s, int e, string queryNorm, int maxSuggestions)
    {
        List<string> suggestions = new List<string>();

        // find insertion point
        int pos = s;
        while (pos < e && books[pos].Normalized.CompareTo(queryNorm) < 0) pos++;

        // Add neighbors around insertion point
        int before = pos - 1;
        int after = pos;
        while (suggestions.Count < maxSuggestions && (before >= s || after < e))
        {
            if (before >= s)
            {
                suggestions.Add(books[before].Original);
                before--;
            }
            if (suggestions.Count >= maxSuggestions) break;
            if (after < e)
            {
                suggestions.Add(books[after].Original);
                after++;
            }
        }

        foreach (var title in suggestions)
            Console.WriteLine(title);
    }

    // ----------------------------
    // Global suggestions (if slice empty)
    // ----------------------------
    static void ShowGlobalSuggestions(string queryNorm, int maxSuggestions)
    {
        List<string> suggestions = new List<string>();

        int idx = 0;
        while (idx < books.Count && suggestions.Count < maxSuggestions)
        {
            suggestions.Add(books[idx].Original);
            idx++;
        }

        foreach (var title in suggestions)
            Console.WriteLine(title);
    }
}
