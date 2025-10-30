# Assignment 5: Browser Navigation System - Implementation Notes

**Name:** Nam Le

## Dual-Stack Pattern Understanding

**How the dual-stack pattern works for browser navigation:**

A stack that keeps track of visited pages (back stack)

A stack that stores pages you’ve moved away from (forward stack).

When you navigate to a new page, that page is pushed onto the back stack and the forward stack is cleared. When you go back, the current page is pushed onto the forward stack, and the previous page is popped from the back stack to become the current page.

## Challenges and Solutions

**Biggest challenge faced:**

Not a lot of big challenge in this assignment aside from fixing bug here and there. But my biggest revelation is understanding why not just use Stack\<T\> for stack-related operation, since it also has all the premade functions needed to do what a stack can, and since you would also get fast index access O(1).

Stack\<T\> provides built-in LIFO behavior, safety, and clarity — ensuring elements are only added or removed from the top, which makes the code more structured and less error-prone.

**How you solved it:**
ChatGPT explaination + Googling for how array stores elements in contiguous memory (side-by-side in one continuous block); Stack\<T\> is built on top of this, but Stack\<T\> hides the internal array, prevents misuse or logic bugs related to LIFO.

**Most confusing concept:**
I'm doing ok with this assignment, the concepts are clear, and the explanation is easy to follow.
It's been a while since I used C# so I got a bit tripped up on public and private variable, and was sratching my head on why the method names in BrowserNavigator was different.

## Time Spent

**Total time:** [about 3 hours]

**Breakdown:**

- Understanding the assignment: [15 mins]
- Implementing the 6 methods: [2 hours]
- Testing and debugging: [1 hours]
- Writing these notes: [15 mins]

