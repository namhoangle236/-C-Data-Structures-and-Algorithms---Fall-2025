# Assignment 9: BST File System Navigator - Implementation Notes

**Name:** Nam Le

## Binary Search Tree Pattern Understanding

**How BST operations work for file system navigation:**
[Explain your understanding of how O(log n) searches, automatic sorting through in-order traversal, and hierarchical file organization work together for efficient file management]

Answer: This assignment is strange, the directory vs directory/file and file vs file/directory insert rule are weird, I understand that these are just rule to put stuff based on what file type they are. 

**But it's impossible to add a file in a directory, if inside that directory exist another directory already**
this alone makes it very confusing on what I needed to do while trying to code and understanding what this tree's trying to do. It's more filter layer, but the filter doesn't make sense (I know it sounds confusing, but I can't find a better way to explain it)
