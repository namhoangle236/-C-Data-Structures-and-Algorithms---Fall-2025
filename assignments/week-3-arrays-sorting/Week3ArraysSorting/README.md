# Tic Tac Toe Board Game

A simple console-based Tic Tac Toe game implemented in C# using a multi-dimensional array.

## Game Rules
- Two players take turns: **X** and **O**.
- Players select a spot on the 3x3 board by entering a number from **1 to 9**.
- First player to get **3 in a row** (horizontally, vertically, or diagonally) wins.
- If all spots are filled and no player wins, the game ends in a **draw**.

## Controls
- Enter a number **1–9** corresponding to the board position to place your mark.
- After the game ends, you can choose to **play again** or **exit**.

## Implementation Details
- The game board is represented using a **multi-dimensional array** (3x3) internally.
- Game logic includes:
  - Drawing the board
  - Switching players
  - Checking for win/draw conditions
  - Handling player input and validation
- Modular design allows running the game via `BoardGame.Run()` from a main menu in `Program.cs`.

## How to Run
1. Open the project in a C# IDE or terminal.
2. Run the project using:
   ```bash
   dotnet run
