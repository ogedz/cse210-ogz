using System;

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)
        {
            // Ask the user for the range of numbers
            Console.Write("Enter the minimum value for the magic number: ");
            int minRange = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the maximum value for the magic number: ");
            int maxRange = Convert.ToInt32(Console.ReadLine());

            // Validate the range
            if (minRange >= maxRange)
            {
                Console.WriteLine("Invalid range. Please ensure that the minimum value is less than the maximum value.");
                continue;
            }

            // create a random number within the specified range
            Random random = new Random();
            int magicNumber = random.Next(minRange, maxRange + 1);

            // start the game
            Console.WriteLine($"Welcome to Guess My Number! The magic number is between {minRange} and {maxRange}.");
            int userGuess;
            int attempts = 0;

            do
            {
                Console.Write("What is your guess? ");
                userGuess = Convert.ToInt32(Console.ReadLine());
                attempts++;

                if (userGuess < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (userGuess > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine("You guessed it!");
                }

            } while (userGuess != magicNumber);

            // tell the user of the number of guesses
            Console.WriteLine($"It took you {attempts} guesses.");

            // Ask if the user wants to play again
            Console.Write("Do you want to play again? (yes/no): ");
            string playAgainResponse = Console.ReadLine().ToLower();

            playAgain = playAgainResponse == "yes";
        }

        Console.WriteLine("Thanks for playing Guess My Number!");
    }
}