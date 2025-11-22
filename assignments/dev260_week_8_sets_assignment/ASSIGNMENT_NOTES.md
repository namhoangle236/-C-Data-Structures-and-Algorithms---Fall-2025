# Assignment 8: Spell Checker & Vocabulary Explorer - Implementation Notes

**Name:** Nam Le

## HashSet Pattern Understanding

**How HashSet<T> operations work for spell checking:**
I'll try to summarize it:
- load a txt file containing words and palce words in a set
- load some other txt file, tokenize each word, place them in a list, and unique words in a set
- check the words in that set against the dictionary set to see words that are misspelled (words that aren't in the set also count)
- display various results

## Challenges and Solutions

**Biggest challenge faced:**
- Regex
- tokenize logic
- chosing sample text for analyze ( enter the name right after the number)
**How you solved it:**
- I can't remember a lot of regex, it's all premade by someone, I just look up how to filter out stuff with regex and copy what's there
- Same for spliting character by punctuation, look up a lists of what to get rid of using spliting
- chosing text for analyze instruction was a bit confusing due to format

**Most confusing concept:**
-The concept is not at all confusing in this module, most of the struggle is from implementing the try/catch, regex and tokenize split

**Issues you discovered during testing:**
- just some confusion on the UI, especially on analyze text, the descripition is a bit confusing, so I kept hitting 2 and trying to type out the sample txt name, while I should have hit 2, then space, then the txt name with .txt at the end.

## HashSet vs List Understanding

**When to use HashSet:**
- if i want a list of unordered unique item, and fast lookup

**When to use List:**
- when i need order, counting occurence of an element, preserve item sequence

## Time Spent

**Total time:** [1.5 hours]

**Breakdown:**
- Understanding HashSet concepts and assignment requirements: [5 mins]
- Implementing the 6 core methods: [1 hours]
- Testing different text files and scenarios: [10 mins]
- Debugging and fixing issues: [20 mins]
- Writing these notes: [5 mins]
