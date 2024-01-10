using System;

class Program
{
    static void Main(string[] args)
    {
        // user first name
        Console.Write("What is your first name? ");
        string firstName = Console.ReadLine();

        // user last name
        Console.Write("What is your last name? ");
        string lastName = Console.ReadLine();

        // the result
        Console.WriteLine($"Your name is {lastName}, {firstName} {lastName}.");
    }
}