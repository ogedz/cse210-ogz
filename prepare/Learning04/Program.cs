using System;

class Program
{
    static void Main(string[] args)
    {   
        // Base Assignment object
        Assignment Assignment = new Assignment("Samuel Bennett", "Multiplication");
        Console.WriteLine(Assignment.GetSummary());

        Console.WriteLine();

        // MathAssignment
        MathAssignment mathAssignment = new MathAssignment("Roberto Rodriguez", "Fractions", "7.3", "8-19");
        Console.WriteLine(mathAssignment.GetSummary());
        Console.WriteLine(mathAssignment.GetHomeworkList());

        Console.WriteLine();

        // WritingAssignment
        WritingAssignment writingAssignment = new WritingAssignment("Mary Waters", "European History", "The Causes of World War II");
        Console.WriteLine(writingAssignment.GetSummary());
        Console.WriteLine(writingAssignment.GetWritingInformation());
    }
}
