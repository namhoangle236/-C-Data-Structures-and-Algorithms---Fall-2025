using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assignment8
{
    /// <summary>
    /// Core spell checker class that uses HashSet<string> for efficient word lookups and text analysis.
    /// This class demonstrates key HashSet concepts including fast Contains() operations,
    /// automatic uniqueness enforcement, and set-based text processing.
    /// </summary>
    public class SpellChecker
    {
        // Core HashSet for dictionary storage - provides O(1) word lookups
        private HashSet<string> dictionary;
        
        // Text analysis results - populated after analyzing a file
        private List<string> allWordsInText;
        private HashSet<string> uniqueWordsInText;
        private HashSet<string> correctlySpelledWords;
        private HashSet<string> misspelledWords;
        private string currentFileName;
        
        public SpellChecker()
        {
            dictionary = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            allWordsInText = new List<string>();
            uniqueWordsInText = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            correctlySpelledWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            misspelledWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            currentFileName = "";
        }
        
        /// <summary>
        /// Gets the number of words in the loaded dictionary.
        /// </summary>
        public int DictionarySize => dictionary.Count;
        
        /// <summary>
        /// Gets whether a text file has been analyzed.
        /// </summary>
        public bool HasAnalyzedText => !string.IsNullOrEmpty(currentFileName);
        
        /// <summary>
        /// Gets the name of the currently analyzed file.
        /// </summary>
        public string CurrentFileName => currentFileName;
        
        /// <summary>
        /// Gets basic statistics about the analyzed text.
        /// </summary>
        public (int totalWords, int uniqueWords, int correctWords, int misspelledWords) GetTextStats()
        {
            return (
                allWordsInText.Count,
                uniqueWordsInText.Count,
                correctlySpelledWords.Count,
                misspelledWords.Count
            );
        }
        
        /// <summary>
        /// TODO #1: Load Dictionary into HashSet
        /// 
        /// Load words from the specified file into the dictionary HashSet.
        /// Requirements:
        /// - Read all lines from the file
        /// - Normalize each word (trim whitespace, convert to lowercase)
        /// - Add normalized words to the dictionary HashSet
        /// - Handle file not found gracefully
        /// - Return true if successful, false if file cannot be loaded
        /// 
        /// Key Concepts:
        /// - HashSet automatically handles duplicates
        /// - StringComparer.OrdinalIgnoreCase for case-insensitive operations
        /// - File I/O with proper error handling
        /// </summary>
        public bool LoadDictionary(string filename)
        {
            // TODO: Implement dictionary loading
            try
            {
                string[] lines = File.ReadAllLines(filename);

                // Clear old dictionary if any
                dictionary.Clear();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string word = line.Trim().ToLowerInvariant();
                    dictionary.Add(word);
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }

        
        /// <summary>
        /// TODO #2: Analyze Text File
        /// 
        /// Load and analyze a text file, populating all internal collections.
        /// Requirements:
        /// - Read the entire file content
        /// - Tokenize into words (split on whitespace and punctuation)
        /// - Normalize each token consistently
        /// - Populate allWordsInText with all tokens (preserving duplicates)
        /// - Populate uniqueWordsInText with unique tokens
        /// - Return true if successful, false if file cannot be loaded
        /// 
        /// Key Concepts:
        /// - Text tokenization and normalization
        /// - List<T> for preserving order and duplicates
        /// - HashSet<T> for automatic uniqueness
        /// - Regex for advanced text processing (stretch goal)
        /// </summary>
        public bool AnalyzeTextFile(string filename)
        {
            // TODO: Implement text file analysis
            try
            {
                string content = File.ReadAllText(filename);

                // Clear previous data
                allWordsInText.Clear();
                uniqueWordsInText.Clear();
                correctlySpelledWords.Clear();
                misspelledWords.Clear();

                // Remove punctuation
                string cleaned = Regex.Replace(content, @"[^\w\s]", "");

                // Split into tokens
                string[] tokens = cleaned.Split(new char[] { ' ', '\t', '\n', '\r' },
                                                StringSplitOptions.RemoveEmptyEntries);

                foreach (string token in tokens)
                {
                    string word = NormalizeWord(token);
                    if (string.IsNullOrWhiteSpace(word))
                        continue;

                    allWordsInText.Add(word);
                    uniqueWordsInText.Add(word);
                }

                currentFileName = filename;
                return true;
            }
            catch
            {
                return false;
            }
        }

        
        /// <summary>
        /// TODO #3: Categorize Words by Spelling
        /// 
        /// After analyzing text, categorize unique words into correct and misspelled.
        /// Requirements:
        /// - Iterate through uniqueWordsInText
        /// - Use dictionary.Contains() to check each word
        /// - Add words to correctlySpelledWords or misspelledWords accordingly
        /// - Clear previous categorization before processing
        /// 
        /// Key Concepts:
        /// - HashSet.Contains() provides O(1) membership testing
        /// - Set partitioning based on criteria
        /// - Defensive programming (clear previous results)
        /// </summary>
        public void CategorizeWords()
        {
            // TODO: Implement word categorization
            // Hint: Clear both correctlySpelledWords and misspelledWords first
            // Hint: Use a foreach loop over uniqueWordsInText
            // Hint: Use dictionary.Contains(word) for fast lookup
            
            correctlySpelledWords.Clear();
            misspelledWords.Clear();

            foreach (string word in uniqueWordsInText)
            {
                if (dictionary.Contains(word))
                {
                    correctlySpelledWords.Add(word);
                }
                else
                {
                    misspelledWords.Add(word);
                }
            }
        }
        
        /// <summary>
        /// TODO #4: Check Individual Word
        /// 
        /// Check if a specific word is in the dictionary and/or appears in analyzed text.
        /// Requirements:
        /// - Normalize the input word consistently
        /// - Check if word exists in dictionary
        /// - If text has been analyzed, check if word appears in text
        /// - If word appears in text, count occurrences in allWordsInText
        /// - Return comprehensive information about the word
        /// 
        /// Key Concepts:
        /// - Consistent normalization across all operations
        /// - Multiple HashSet lookups for comprehensive analysis
        /// - LINQ or manual counting for frequency analysis
        /// </summary>
        public (bool inDictionary, bool inText, int occurrences) CheckWord(string word)
        {
            // TODO: Implement individual word checking
            // Hint: Normalize the word using the same method as other operations
            // Hint: Use dictionary.Contains() and uniqueWordsInText.Contains()
            // Hint: Use allWordsInText.Count(w => w.Equals(normalizedWord, StringComparison.OrdinalIgnoreCase))
            
            string normalized = NormalizeWord(word);

            bool inDictionary = dictionary.Contains(normalized);
            bool inText = uniqueWordsInText.Contains(normalized);

            int count = 0;
            foreach (string w in allWordsInText)
            {
                if (w.Equals(normalized, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return (inDictionary, inText, count);
        }
        
        /// <summary>
        /// TODO #5: Get Misspelled Words
        /// 
        /// Return a sorted list of all misspelled words from the analyzed text.
        /// Requirements:
        /// - Return words from misspelledWords HashSet
        /// - Sort alphabetically for consistent display
        /// - Limit results if there are too many (optional)
        /// - Return empty collection if no text analyzed
        /// 
        /// Key Concepts:
        /// - Converting HashSet to sorted List
        /// - LINQ for sorting and limiting results
        /// - Defensive programming for uninitialized state
        /// </summary>
        public List<string> GetMisspelledWords(int maxResults = 50)
        {
            // TODO: Implement misspelled words retrieval
            // Hint: Convert misspelledWords to List, then use OrderBy()
            // Hint: Use Take(maxResults) to limit results if needed
            // Hint: Return empty list if no text has been analyzed
            
            if (!HasAnalyzedText)
                return new List<string>();

            List<string> result = misspelledWords.ToList();
            result.Sort();

            if (result.Count > maxResults)
            {
                return result.Take(maxResults).ToList();
            }

            return result;
        }
        
        /// <summary>
        /// TODO #6: Get Unique Words Sample
        /// 
        /// Return a sample of unique words found in the analyzed text.
        /// Requirements:
        /// - Return words from uniqueWordsInText HashSet
        /// - Sort alphabetically for consistent display
        /// - Limit to specified number of results
        /// - Return empty collection if no text analyzed
        /// 
        /// Key Concepts:
        /// - HashSet enumeration and conversion
        /// - LINQ for data manipulation
        /// - Sampling large datasets
        /// </summary>
        public List<string> GetUniqueWordsSample(int maxResults = 20)
        {
            // TODO: Implement unique words sample retrieval
            // Hint: Similar to GetMisspelledWords but use uniqueWordsInText
            // Hint: Consider showing a mix of correct and misspelled words
            
            if (!HasAnalyzedText)
                return new List<string>();

            List<string> sample = uniqueWordsInText.ToList();
            sample.Sort();

            if (sample.Count > maxResults)
            {
                return sample.Take(maxResults).ToList();
            }

            return sample;
        }
        
        // Helper method for consistent word normalization
        private string NormalizeWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return "";
                
            // Remove punctuation and convert to lowercase
            word = Regex.Replace(word.Trim(), @"[^\w]", "");
            return word.ToLowerInvariant();
        }
    }
}