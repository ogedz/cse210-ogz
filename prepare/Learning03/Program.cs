using System;

class Program
{
    static void Main(string[] args)
    {
        // Showing the fractions
        Fraction fraction1 = new Fraction();        // 1/1
        Console.WriteLine(fraction1.GetFractionString());
        Console.WriteLine(fraction1.GetDecimalValue());

        Fraction fraction2 = new Fraction(5);       // 5/1
        Console.WriteLine(fraction2.GetFractionString());
        Console.WriteLine(fraction2.GetDecimalValue());

        Fraction fraction3 = new Fraction(3, 4);    // 3/4
        Console.WriteLine(fraction3.GetFractionString());
        Console.WriteLine(fraction3.GetDecimalValue());

        Fraction fraction4 = new Fraction(1, 3);    // 1/3
        Console.WriteLine(fraction4.GetFractionString());
        Console.WriteLine(fraction4.GetDecimalValue());
    }
}

class Fraction
{
    private int numerator;
    private int denominator;

    // Constructor with no parameters
    public Fraction()
    {
        numerator = 1;
        denominator = 1;
    }

    // Constructor with one parameter for the denominator,
    public Fraction(int top)
    {
        numerator = top;
        denominator = 1;
    }

    // Constructor with two parameters for denominator and numerator
    public Fraction(int top, int bottom)
    {
        numerator = top;
        // Ensure denominator is not zero
        denominator = (bottom != 0) ? bottom : 1;
    }

    // Getter and Setter for the numerator
    public int Numerator
    {
        get { return numerator; }
        set { numerator = value; }
    }

    // Getter and Setter for the denominator
    public int Denominator
    {
        get { return denominator; }
        set
        {
            // Ensure denominator is not zero
            denominator = (value != 0) ? value : 1;
        }
    }

    // To show fraction in the form 3/4
    public string GetFractionString()
    {
        return $"{numerator}/{denominator}";
    }

    // DIvide- show numerator and denominator
    public double GetDecimalValue()
    {
        return (double)numerator / denominator;
    }
}



