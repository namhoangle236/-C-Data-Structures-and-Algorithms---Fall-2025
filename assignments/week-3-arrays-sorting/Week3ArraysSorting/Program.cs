using System;

// This is the main entry point for the application. User can choose which program to run.
// NOTE: BoardGame and BookCatalog are in the same project and namespace, don’t need to import or include anything to access them.


class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== Main Menu ====");
            Console.WriteLine("1. Play Tic Tac Toe");
            Console.WriteLine("2. Run Book Catalog Sorting and lookup");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    BoardGame.Run(); // call your TicTacToe class
                    break;
                case "2":
                    BookCatalog.Run(); // placeholder for later
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
