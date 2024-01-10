using System;

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();

        Console.WriteLine("Enter a list of numbers, type 0 when finished.");

        int input;
        do
        {
            Console.Write("Enter number: ");
            input = Convert.ToInt32(Console.ReadLine());
            
            if (input != 0)
            {
                numbers.Add(input);
            }

        } while (input != 0);

        // Core Requirements
        int sum = CalculateSum(numbers);
        double average = CalculateAverage(numbers);
        int maxNumber = FindMaxNumber(numbers);

        Console.WriteLine($"The sum is: {sum}");
        Console.WriteLine($"The average is: {average}");
        Console.WriteLine($"The largest number is: {maxNumber}");

        // To find the smallest number
        if (numbers.Count > 0)
        {
            int smallestPositive = FindSmallestPositive(numbers);
            List<int> sortedList = SortList(numbers);

            Console.WriteLine($"The smallest positive number is: {smallestPositive}");
            Console.WriteLine("The sorted list is:");
            foreach (int num in sortedList)
            {
                Console.WriteLine(num);
            }
        }
    }

    static int CalculateSum(List<int> numbers)
    {
        int sum = 0;
        foreach (int num in numbers)
        {
            sum += num;
        }
        return sum;
    }

    static double CalculateAverage(List<int> numbers)
    {
        if (numbers.Count == 0)
        {
            return 0;
        }

        int sum = CalculateSum(numbers);
        return (double)sum / numbers.Count;
    }

    static int FindMaxNumber(List<int> numbers)
    {
        if (numbers.Count == 0)
        {
            return 0;
        }

        int max = numbers[0];
        foreach (int num in numbers)
        {
            if (num > max)
            {
                max = num;
            }
        }
        return max;
    }

    static int FindSmallestPositive(List<int> numbers)
    {
        int smallestPositive = int.MaxValue;
        foreach (int num in numbers)
        {
            if (num > 0 && num < smallestPositive)
            {
                smallestPositive = num;
            }
        }
        return smallestPositive == int.MaxValue ? 0 : smallestPositive;
    }

    static List<int> SortList(List<int> numbers)
    {
        List<int> sortedList = new List<int>(numbers);
        sortedList.Sort();
        return sortedList;
    }
}