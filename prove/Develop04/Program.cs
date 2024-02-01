using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        MindfulnessApp app = new MindfulnessApp();
        app.Start();
    }
}

class MindfulnessApp
{
    private List<Activity> activities;

    public MindfulnessApp()
    {
        activities = new List<Activity>
        {
            new BreathingActivity(),
            new ReflectionActivity(),
            new ListingActivity(),
            new PersonalizedActivity () 
        };
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("0. Exit");

            for (int i = 0; i < activities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {activities[i].GetType().Name}");
            }

            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= activities.Count)
            {
                if (choice == 0)
                {
                    Console.WriteLine("Exiting the program.");
                    return;
                }

                RunActivity(activities[choice - 1]);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 4.");
            }
        }
    }

    private void RunActivity(Activity activity)
    {
        activity.Start();
        Console.WriteLine("Activity completed!");
        Console.WriteLine();

        // Keeping a log 
        activity.LogActivity();
    }
}

abstract class Activity
{
    protected int Duration;

    public virtual void Start()
    {
        Console.Write("Enter the duration in seconds: ");
        Duration = int.Parse(Console.ReadLine());

        Console.WriteLine($"Starting {GetType().Name} activity...");
        Console.WriteLine("Get ready!");
        Pause(3);
    }

    public void Pause(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.WriteLine($"Pausing... {i}");
            Thread.Sleep(1000);
        }
    }

    public virtual void End()
    {
        Console.WriteLine($"Good job! You've completed the {GetType().Name} activity for {Duration} seconds.");
        Pause(3);
    }

    // Keeping a log
    public virtual void LogActivity()
    {
        Console.WriteLine($"Logging activity: {GetType().Name}");
    }
}

class BreathingActivity : Activity
{
    public override void Start()
    {
        base.Start();
        Console.WriteLine("This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.");

        for (int i = 0; i < Duration; i++)
        {
            Console.WriteLine("Breathe in...");
            Pause(2);
            Console.WriteLine("Breathe out...");
            Pause(2);
        }

        End();
    }
}

class ReflectionActivity : Activity
{
    private readonly string[] prompts = {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private readonly string[] questions = {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    private List<string> usedPrompts;

    public ReflectionActivity()
    {
        usedPrompts = new List<string>();
    }

    public override void Start()
    {
        base.Start();
        Console.WriteLine("This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.");

        string randomPrompt = GetRandomPrompt();
        Console.WriteLine(randomPrompt);

        foreach (string question in questions)
        {
            Console.WriteLine(question);
            Pause(3);
        }

        End();
    }

    private string GetRandomPrompt()
    {
        if (!prompts.Except(usedPrompts).Any())
        {
            usedPrompts.Clear(); // Reset used prompts
        }

        string randomPrompt;
        do
        {
            randomPrompt = prompts[new Random().Next(prompts.Length)];
        } while (usedPrompts.Contains(randomPrompt));

        usedPrompts.Add(randomPrompt);
        return randomPrompt;
    }

    public override void LogActivity()
    {
        base.LogActivity();
        Console.WriteLine($"Used prompts: {string.Join(", ", usedPrompts)}");
    }
}

class ListingActivity : Activity
{
    private readonly string[] listPrompts = {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public override void Start()
    {
        base.Start();
        Console.WriteLine("This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.");

        string randomListPrompt = listPrompts[new Random().Next(listPrompts.Length)];
        Console.WriteLine(randomListPrompt);

        Console.WriteLine("Get ready to start listing...");
        Pause(3);

        Console.WriteLine("Start listing...");

        for (int i = 1; i <= Duration; i++)
        {
            Console.WriteLine($"Item {i}: ");
            Thread.Sleep(1000);
        }

        Console.WriteLine($"You listed {Duration} items.");

        End();
    }
}

class PersonalizedActivity : Activity
{
    public override void Start()
    {
        base.Start();
        Console.WriteLine("This is a personalized activity designed to promote relaxation and boost creativity.");

        Console.WriteLine("Follow the steps below:");

        // Steps
        PerformStep("Close your eyes and take three deep breaths.");
        PerformStep("Imagine a peaceful natural setting, like a serene beach or a quiet forest.");
        PerformStep("Focus on positive affirmations related to creativity and relaxation.");
        PerformStep("Engage in a brief creative exercise, such as drawing or writing.");
        PerformStep("Savor a calming tea or beverage of your choice.");
        PerformStep("Listen to soothing music or sounds.");
        PerformStep("Express gratitude for something positive in your life.");
        PerformStep("Take a mindful walk, paying attention to each step and your surroundings.");
        PerformStep("Disconnect from electronic devices for a few minutes.");
        PerformStep("Stretch and release tension from your body.");
        PerformStep("Write down any creative ideas that come to mind.");

        End();
    }

    private void PerformStep(string step)
    {
        Console.WriteLine($"Step: {step}");
        Pause(4);
    }
}


