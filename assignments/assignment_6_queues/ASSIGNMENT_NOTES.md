# Assignment 6: Game Matchmaking System - Implementation Notes

**Name:** Nam Le

## Multi-Queue Pattern Understanding

**How the multi-queue pattern works for game matchmaking:**

Casual Queue: FIFO — match the first two players in line, no skill check.

Ranked Queue: Match only players within +/- 2 skill levels.

QuickPlay Queue: Try skill-based matching first, but if the queue grows more than a certain amount, match anyone for speed.

## Challenges and Solutions

**Biggest challenge faced:**

TryCreateMatch function - the bread and butter of different dequeue logic
GetQueueEstimate(GameMode mode) get a param of type GameMode, Casual, Ranked or QuickPlay then we use this param for GetQueueByMode, based on the param, we get the queue, etc... Navigating the mess
I understand the logic for matching, it's the easiest part, the difficult part is always about looking at the premade code and trying to make sense of the structure and how data flows, it's really difficult to complete on my own. 
Rebuilding the queue to not include p2

**How you solved it:**

Needed to read up to the helper method first, implement GetQueueByMode
Linking GameMode in Player.cs; GetQueueByMode, and figuring out the switch
AI help

**Most confusing concept:**

Not about queues, but mostly going through the premade codes and understanding the setup as well as why they are nececssary.

Some example include but not limited to:
- Creating a function to pull the lists of player from allPlayers instead of directly accessing them
- Looking through TODOs (especially in lab 6) to figure out the naming of tracking variables
- Some C# syntax that I've never seen before such as OrdinalIgnoreCase
- Knowing when and where to use which premade functions in Players, such as JoinQueue() or LeaveQueue() (there's guided comments, but I still need to know why it's used there, bad at keeping track of which thing do what)


## Real-World Applications

**How this relates to actual game matchmaking:**

Elo point is like skill level, a number used for match making

## Stretch Features

No

## Time Spent

**Total time:** [5 hours]

**Breakdown:**

- Understanding the assignment and queue concepts + structure: [30 mins]
- Implementing the 6 core methods: [3.5 hours]
- Testing different game modes and scenarios: [10 mins]
- Debugging and fixing issues: [30 mins]
- Writing these notes: [15 mins]

**Most time-consuming part:** 

HandleCreateMatches, knowing what function inPlayer.cs to use

