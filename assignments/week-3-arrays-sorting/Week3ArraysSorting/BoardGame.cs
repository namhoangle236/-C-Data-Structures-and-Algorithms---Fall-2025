using System;

class BoardGame
{
    static char[,] board = new char[3, 3];
    static char currentPlayer = 'X';

    public static void Run()        // --> so that Program.cs can call this method to start the game
    {
        do
        {
            ResetBoard();
            PlayGame();
        }
        while (AskPlayAgain());
    }

    // -------------------- GAME LOOP --------------------
    static void PlayGame()
    {
        bool gameOver = false;

        while (!gameOver)
        {
            DrawBoard();
            PlayerMove();

            if (CheckWin())
            {
                DrawBoard();
                Console.WriteLine($"Player {currentPlayer} wins!");
                gameOver = true;
            }
            else if (CheckDraw())
            {
                DrawBoard();
                Console.WriteLine("It's a draw!");
                gameOver = true;
            }
            else
            {
                SwitchPlayer();
            }
        }
    }

    // -------------------- BOARD --------------------
    static void ResetBoard()
    {
        char n = '1';
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                board[i, j] = n++;
    }
    // make it so that the multidimensional array shows numbers 1-9

    static void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine();
        for (int i = 0; i < 3; i++)
        {
            Console.Write($" {board[i, 0]} | {board[i, 1]} | {board[i, 2]} \n");
            if (i < 2) Console.WriteLine("---+---+---");
        }
        Console.WriteLine();
    }
    // draw out the board (take items from the multidimensional array and display them in a tic tac toe format)
    // if don't reset board first time, or after each game, the board will keep previous X and O marks


    // -------------------- PLAYER --------------------
    static void PlayerMove()
    {
        int choice;         // to hold user input for position 1-9
        while (true)
        {
            Console.Write($"Player {currentPlayer}, choose a spot (1-9): ");
            if (int.TryParse(Console.ReadLine(), out choice) &&
                choice >= 1 && choice <= 9)
            {
                int row = (choice - 1) / 3;         // imagine this shit, beeg number when divided return larger number (also this returns integer only!) --> match the row
                int col = (choice - 1) % 3;         // same for this, but remainder should return the correct column
                if (board[row, col] != 'X' && board[row, col] != 'O')
                {
                    board[row, col] = currentPlayer;
                    break;
                }
            }
            Console.WriteLine("Invalid move, try again.");
        }
    }

    static void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
    }

    // -------------------- CHECKS --------------------
    static bool CheckWin()
    {
        // Check rows
        for (int i = 0; i < 3; i++)
            if (board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer)
                return true;

        // Check columns
        for (int i = 0; i < 3; i++)
            if (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer)
                return true;

        // Check diagonals
        if ((board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) ||
            (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer))
            return true;

        return false;
    }

    static bool CheckDraw()
    {
        foreach (char c in board)
            if (c != 'X' && c != 'O')
                return false;
        return true;
    }

    // -------------------- REPLAY --------------------
    static bool AskPlayAgain()
    {
        Console.Write("Play again? (y/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "y";
    }
}
