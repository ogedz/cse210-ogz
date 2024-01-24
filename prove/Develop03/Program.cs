using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("Week 03 Develop: Ogedz Scripture Memorizer");

        List<Scripture> scriptureLibrary = LoadScripturesFromFile("scriptures.txt");

        if (scriptureLibrary.Count == 0)
        {
            Console.WriteLine("No scriptures found in the library. Exiting.");
            return;
        }

        UserProfile userProfile = new UserProfile();
        DifficultyLevel difficulty = GetUserDifficulty();
        Game game = new Game(scriptureLibrary, userProfile, difficulty);

        try
        {
            game.Start();
        }
        catch (Exception ex)
        {
            LogError($"An error occurred: {ex.Message}");
        }
    }

    private static List<Scripture> LoadScripturesFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                List<Scripture> scriptures = lines
                    .Select(line => line.Split('|'))
                    .Where(parts => parts.Length == 2)
                    .Select(parts => new Scripture(parts[0].Trim(), parts[1].Trim()))
                    .ToList();

                return scriptures;
            }
            else
            {
                LogError($"Scripture file not found at: {filePath}");
            }
        }
        catch (Exception ex)
        {
            LogError($"Error loading scriptures from file: {ex.Message}");
        }

        return new List<Scripture>();
    }

    private static void LogError(string message)
    {
        Console.WriteLine($"ERROR: {message}");
    }

    private static DifficultyLevel GetUserDifficulty()
    {
        Console.WriteLine("Select difficulty level:");
        Console.WriteLine("1. Easy");
        Console.WriteLine("2. Medium");
        Console.WriteLine("3. Hard");

        while (true)
        {
            Console.Write("Enter the number of your choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && Enum.IsDefined(typeof(DifficultyLevel), choice))
            {
                return (DifficultyLevel)choice;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}

enum DifficultyLevel
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}

class UserProfile
{
    public int Score { get; private set; }

    public void IncreaseScore(int points)
    {
        Score += points;
    }
}

class Game
{
    private const string QuitCommand = "quit";
    private const int WordsToHidePerIteration = 2;
    private const int TimeLimitInSecondsEasy = 60;
    private const int TimeLimitInSecondsMedium = 45;
    private const int TimeLimitInSecondsHard = 30;

    private readonly Memorizer memorizer;
    private readonly List<Scripture> scriptureLibrary;
    private readonly UserProfile userProfile;
    private readonly DifficultyLevel difficulty;

    public Game(List<Scripture> library, UserProfile profile, DifficultyLevel difficultyLevel)
    {
        scriptureLibrary = library;
        userProfile = profile;
        difficulty = difficultyLevel;
        memorizer = new Memorizer(scriptureLibrary.GetRandomElement());
    }

    public void Start()
    {
        int timeLimitInSeconds = GetTimeLimitInSeconds();

        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(timeLimitInSeconds);

        do
        {
            Console.Clear();
            memorizer.DisplayScripture();

            Console.WriteLine($"\nPress Enter to continue or type '{QuitCommand}' to exit.");

            TimeSpan remainingTime = endTime - DateTime.Now;

            if (remainingTime.TotalSeconds <= 0)
            {
                Console.WriteLine("Time's up! Exiting.");
                break;
            }

            Console.WriteLine($"Time remaining: {remainingTime:mm\\:ss}");

            string input = Console.ReadLine();

            if (input.ToLower() == QuitCommand)
                break;

            memorizer.HideRandomWords(WordsToHidePerIteration);
        } while (!memorizer.AllWordsHidden);

        int pointsEarned = CalculateScore(timeLimitInSeconds);
        userProfile.IncreaseScore(pointsEarned);

        Console.WriteLine($"Session complete! You earned {pointsEarned} points. Total score: {userProfile.Score}");
        Thread.Sleep(2000); // Pausing for 2 seconds before exiting
    }

    private int GetTimeLimitInSeconds()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                return TimeLimitInSecondsEasy;
            case DifficultyLevel.Medium:
                return TimeLimitInSecondsMedium;
            case DifficultyLevel.Hard:
                return TimeLimitInSecondsHard;
            default:
                throw new ArgumentOutOfRangeException(nameof(difficulty), "Invalid difficulty level.");
        }
    }

    private int CalculateScore(int totalTimeInSeconds)
    {
        int baseScore = 1000;
        int timeBonus = (int)(baseScore * (1 - (TimeLimitInSecondsMedium - totalTimeInSeconds) / (double)TimeLimitInSecondsMedium));
        return baseScore + timeBonus;
    }
}

class Scripture
{
    public string Reference { get; private set; }
    public string Text { get; private set; }

    public Scripture(string reference, string text)
    {
        Reference = reference;
        Text = text;
    }
}

class Memorizer
{
    private readonly Scripture _scripture;
    private readonly List<Word> wordsToMemorize;

    public Memorizer(Scripture scripture)
    {
        _scripture = scripture;
        wordsToMemorize = scripture.Text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public bool AllWordsHidden => wordsToMemorize.All(word => word.IsHidden);

    public void DisplayScripture()
    {
        Console.WriteLine($"Reference: {_scripture.Reference}");
        string hiddenText = GetHiddenText();
        Console.WriteLine(hiddenText);
    }

    public void HideRandomWords(int numberOfWordsToHide)
    {
        var wordsToHide = wordsToMemorize.Where(word => !word.IsHidden).OrderBy(w => Guid.NewGuid()).Take(numberOfWordsToHide);
        foreach (var word in wordsToHide)
        {
            word.Hide();
        }
    }

    private string GetHiddenText()
    {
        return string.Join(" ", wordsToMemorize.Select(word => word.IsHidden ? new string('_', word.Text.Length) : word.Text));
    }
}

class Word
{
    public string Text { get; }
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }
}

public static class ListExtensions
{
    private static Random random = new Random();

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[random.Next(list.Count)];
    }
}
