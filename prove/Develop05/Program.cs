using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class Goal
{
    protected internal string name;
    protected internal int points;

    public Goal(string name)
    {
        this.name = name;
        this.points = 0;
    }

    public abstract void RecordEvent();

    public virtual void DisplayStatus()
    {
        Console.WriteLine($"[{(IsComplete() ? 'X' : ' ')}] {name}");
    }

    public int GetPoints()
    {
        return points;
    }

    public abstract bool IsComplete();
}

public class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name)
    {
        this.points = points;
    }

    public override void RecordEvent()
    {
        points += 1000; 
    }

    public override bool IsComplete()
    {
        return true; // Simple goals are always complete
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name)
    {
        this.points = points;
    }

    public override void RecordEvent()
    {
        points += 100; 
    }

    public override bool IsComplete()
    {
        return false; // Eternal goals are never complete
    }
}

public class ChecklistGoal : Goal
{
    private int targetCount;
    private int completedCount;

    public ChecklistGoal(string name, int points, int targetCount) : base(name)
    {
        this.points = points;
        this.targetCount = targetCount;
        this.completedCount = 0;
    }

    public override void RecordEvent()
    {
        points += 50; 
        completedCount++;

        if (completedCount == targetCount)
        {
            points += 500; // Bonus points
        }
    }

    public override bool IsComplete()
    {
        return completedCount == targetCount;
    }

    public override void DisplayStatus()
    {
        Console.WriteLine($"[{(IsComplete() ? 'X' : ' ')}] {name} (Completed {completedCount}/{targetCount} times)");
    }
}

public static class GoalManager
{
    private const string FileName = "goals.txt";

    public static void SaveGoals(List<Goal> goals)
    {
        try
        {
            using (StreamWriter writer = new(FileName))
            {
                foreach (var goal in goals)
                {
                    writer.WriteLine($"{goal.GetType().Name}|{goal.name}|{goal.points}");
                }
            }

            Console.WriteLine("Goals saved successfully.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error saving goals: {ex.Message}");
        }
    }

    public static List<Goal> LoadGoals()
    {
        List<Goal> loadedGoals = new List<Goal>();

        try
        {
            if (File.Exists(FileName))
            {
                using (StreamReader reader = new StreamReader(FileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] goalInfo = reader.ReadLine().Split('|');
                        if (goalInfo.Length == 3)
                        {
                            string goalType = goalInfo[0];
                            string goalName = goalInfo[1];
                            int goalPoints;
                            if (int.TryParse(goalInfo[2], out goalPoints))
                            {
                                Goal goal = CreateGoalInstance(goalType, goalName, goalPoints);
                                loadedGoals.Add(goal);
                            }
                            else
                            {
                                Console.WriteLine($"Error loading goals: Invalid points for goal '{goalName}'. Skipping...");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error loading goals: Invalid format. Skipping...");
                        }
                    }
                }

                Console.WriteLine("Goals loaded successfully.");
            }
            else
            {
                Console.WriteLine("No saved goals found.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error loading goals: {ex.Message}");
        }

        return loadedGoals;
    }

    private static Goal CreateGoalInstance(string goalType, string name, int points)
    {
        switch (goalType)
        {
            case nameof(SimpleGoal):
                return new SimpleGoal(name, points);
            case nameof(EternalGoal):
                return new EternalGoal(name, points);
            case nameof(ChecklistGoal):
                return new ChecklistGoal(name, points, 10); 
            case nameof(PenaltyGoal): 
                return new PenaltyGoal(name, points);
            default:
                throw new ArgumentException($"Unknown goal type: {goalType}");
        }
    }
}

public class Quest
{
    private string v1;
    private int v2;

    public string Name { get; }
    public string Description { get; }
    public int RewardPoints { get; }
    public bool IsCompleted { get; private set; }

    public Quest(string name, string description, int rewardPoints)
    {
        Name = name;
        Description = description;
        RewardPoints = rewardPoints;
        IsCompleted = false;
    }

    public Quest(string v1, int v2)
    {
        this.v1 = v1;
        this.v2 = v2;
    }

    public void CompleteQuest()
    {
        IsCompleted = true;
        Console.WriteLine($"Congratulations! You completed the quest: {Name}");
    }
}

public class User
{
    public string Username { get; set; }
    public int Level { get; set; }
    public int ExperiencePoints { get; set; }
    public List<string> AchievementBadges { get; set; }
    public Avatar Avatar { get; set; }
    public List<Quest> ActiveQuests { get; set; }

    public User(string username)
    {
        Username = username;
        Level = 1;
        ExperiencePoints = 0;
        AchievementBadges = new List<string>();
        Avatar = new Avatar();
        ActiveQuests = new List<Quest>();
    }

    public void StartQuest(Quest quest)
    {
        ActiveQuests.Add(quest);
        Console.WriteLine($"Quest started: {quest.Name}");
    }

    public void CompleteQuest(Quest quest)
    {
        if (ActiveQuests.Contains(quest))
        {
            quest.CompleteQuest();
            ActiveQuests.Remove(quest);
            EarnExperiencePoints(quest.RewardPoints);
        }
    }

    public void LevelUp()
    {
        Level++;
        Console.WriteLine($"Congratulations, {Username}! You've leveled up to level {Level}!");
    }

    public void EarnExperiencePoints(int points)
    {
        ExperiencePoints += points;
        Console.WriteLine($"You've earned {points} experience points!");
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // To check if user should level up based on experience points
        if (ExperiencePoints >= 1000)
        {
            int levelUps = ExperiencePoints / 1000;
            for (int i = 0; i < levelUps; i++)
            {
                LevelUp();
                ExperiencePoints -= 1000;
            }
        }
    }
}


public class Avatar
{
    public string Appearance { get; set; }
    public List<string> Accessories { get; set; }

    public Avatar()
    {
        Appearance = "Default";
        Accessories = new List<string>();
    }

    public void CustomizeAppearance(string appearance)
    {
        Appearance = appearance;
        Console.WriteLine("Avatar appearance updated!");
    }

    public void AddAccessory(string accessory)
    {
        Accessories.Add(accessory);
        Console.WriteLine("New accessory added to avatar!");
    }
}

public class PenaltyGoal : Goal
{
    public PenaltyGoal(string name, int points) : base(name)
    {
        this.points = points;
    }

    public override void RecordEvent()
    {
        points -= 50; 
    }

    public override bool IsComplete()
    {
        return false; // Penalty goals are never complete
    }
}

class Program
{
    static void Main()
    {
        List<Goal> goals = GoalManager.LoadGoals();
        User user = InitializeUser();
        List<Quest> quests = InitializeQuests(); // Initialize some Quests

        while (true)
        {
            DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal(goals);
                    break;
                case "2":
                    RecordEvent(goals);
                    break;
                case "3":
                    DisplayGoals(goals);
                    break;
                case "4":
                    GoalManager.SaveGoals(goals);
                    break;
                case "5":
                    DisplayTotalScore(goals);
                    break;
                case "6":
                    DisplayUserDetails(user);
                    break;
                case "7":
                    CustomizeAvatar(user.Avatar);
                    break;
                case "8":
                    StartQuest(user, quests);
                    break;
                case "9":
                    CompleteQuest(user);
                    break;
                case "10":
                    AddPenaltyGoal(goals);
                    break;
                case "11":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static User InitializeUser()
    {
        Console.Write("Enter your username: ");
        string username = Console.ReadLine();
        return new User(username);
    }

static List<Quest> InitializeQuests()
{
    List<Quest> quests = new List<Quest>();

    // Quest
    quests.Add(new Quest("Learn a New Skill.", 200));
    quests.Add(new Quest("Complete a Fitness Challenge", 500));
    quests.Add(new Quest("Volunteer for a Cause", 1000));
    quests.Add(new Quest("Read a Book", "Read a book on a topic of interest or personal development.", 300));
    quests.Add(new Quest("Start a Savings Plan", "Begin a savings plan to achieve a financial goal.", 800));
    quests.Add(new Quest("Improve Time Management", "Implement strategies to improve time management.", 400));
    quests.Add(new Quest("Complete a DIY Project", "Undertake a do-it-yourself project.", 600));
    quests.Add(new Quest("Attend a Workshop or Seminar", "Participate in a workshop or seminar.", 700));
    quests.Add(new Quest("Practice Mindfulness", "Incorporate mindfulness practices into your daily routine.", 300));
    quests.Add(new Quest("Learn a New Recipe", "Discover and prepare a new recipe from a cuisine you're not familiar with.", 250));
    quests.Add(new Quest("Complete a Home Organization Project", "Tackle a home organization project.", 400));
    quests.Add(new Quest("Explore Nature", "Spend time outdoors exploring nature.", 350));
    quests.Add(new Quest("Start a Journal", "Begin journaling to reflect on your thoughts, feelings, and experiences.", 200));
    quests.Add(new Quest("Attend a Networking Event", "Attend a networking event or professional meetup.", 500));
    quests.Add(new Quest("Learn a New Instrument", "Challenge yourself to learn to play a musical instrument.", 600));
    quests.Add(new Quest("Complete a Physical Challenge", "Set and achieve a physical challenge.", 1000));
    quests.Add(new Quest("Start a Garden", "Start a garden at home and nurture it over time.", 450));
    quests.Add(new Quest("Improve Communication Skills", "Work on improving your communication skills.", 400));

    return quests;
}



    static void AddPenaltyGoal(List<Goal> goals)
    {
        Console.Write("Enter penalty goal name: ");
        string name = Console.ReadLine();

        Goal penaltyGoal = new PenaltyGoal(name, 0);
        goals.Add(penaltyGoal);
        Console.WriteLine($"New penalty goal '{name}' created successfully.");
    }

    static void StartQuest(User user, List<Quest> quests)
    {
        Console.WriteLine("\n===== Available Quests =====");
        for (int i = 0; i < quests.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {quests[i].Name}: {quests[i].Description}");
        }

        Console.Write("\nEnter the number of the quest you want to start: ");
        if (int.TryParse(Console.ReadLine(), out int questIndex) && questIndex > 0 && questIndex <= quests.Count)
        {
            user.StartQuest(quests[questIndex - 1]);
        }
        else
        {
            Console.WriteLine("Invalid quest number. Please try again.");
        }
    }

    static void CompleteQuest(User user)
    {
        Console.WriteLine("\n===== Active Quests =====");
        for (int i = 0; i < user.ActiveQuests.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {user.ActiveQuests[i].Name}");
        }

        Console.Write("\nEnter the number of the quest you want to complete: ");
        if (int.TryParse(Console.ReadLine(), out int questIndex) && questIndex > 0 && questIndex <= user.ActiveQuests.Count)
        {
            user.CompleteQuest(user.ActiveQuests[questIndex - 1]);
        }
        else
        {
            Console.WriteLine("Invalid quest number. Please try again.");
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n===== Ogeds Eternal Quest Program =====");
        Console.WriteLine("1. Create New Goal");
        Console.WriteLine("2. Record Event");
        Console.WriteLine("3. Display Goals");
        Console.WriteLine("4. Save Goals");
        Console.WriteLine("5. Display Total Score");
        Console.WriteLine("6. Display User Details");
        Console.WriteLine("7. Customize Avatar");
        Console.WriteLine("8. Start Quest");
        Console.WriteLine("9. Complete Quest");
        Console.WriteLine("10. Add Penalty Goal");
        Console.WriteLine("11. Exit");
        Console.Write("Enter your choice: ");
    }

    static void CreateGoal(List<Goal> goals)
    {
        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();

        Console.Write("Select goal type (1: Simple, 2: Eternal, 3: Checklist): ");
        string typeChoice = Console.ReadLine();

        Goal goal;

        switch (typeChoice)
        {
            case "1":
                goal = new SimpleGoal(name, 0);
                break;
            case "2":
                goal = new EternalGoal(name, 0);
                break;
            case "3":
                goal = new ChecklistGoal(name, 0, 10); 
                break;
            default:
                Console.WriteLine("Invalid goal type. Creating a simple goal by default.");
                goal = new SimpleGoal(name, 0);
                break;
        }

        goals.Add(goal);
        Console.WriteLine($"New goal '{name}' created successfully.");
    }

    static void RecordEvent(List<Goal> goals)
    {
        Console.WriteLine("\n===== Record Event =====");
        DisplayGoals(goals);

        Console.Write("\nEnter the number of the goal you want to record an event for: ");
        if (int.TryParse(Console.ReadLine(), out int goalIndex) && goalIndex > 0 && goalIndex <= goals.Count)
        {
            goals[goalIndex - 1].RecordEvent();
            Console.WriteLine("Event recorded successfully.");
        }
        else
        {
            Console.WriteLine("Invalid goal number. Please try again.");
        }
    }

    static void DisplayGoals(List<Goal> goals)
    {
        Console.WriteLine("\n===== Your Goals =====");
        for (int i = 0; i < goals.Count; i++)
        {
            Console.Write($"{i + 1}. ");
            goals[i].DisplayStatus();
        }
    }

    static void DisplayTotalScore(List<Goal> goals)
    {
        int totalScore = goals.Sum(g => g.GetPoints());
        Console.WriteLine($"\nTotal Score: {totalScore}");
    }

    static void DisplayUserDetails(User user)
    {
        Console.WriteLine("\n===== User Details =====");
        Console.WriteLine($"Username: {user.Username}");
        Console.WriteLine($"Level: {user.Level}");
        Console.WriteLine($"Experience Points: {user.ExperiencePoints}");

        Console.WriteLine("\nAchievement Badges:");
        foreach (var badge in user.AchievementBadges)
        {
            Console.WriteLine(badge);
        }
    }

    static void CustomizeAvatar(Avatar avatar)
    {
        Console.WriteLine("\n===== Customize Avatar =====");
        Console.WriteLine("1. Change Avatar Appearance");
        Console.WriteLine("2. Add Accessory");
        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter new appearance: ");
                string appearance = Console.ReadLine();
                avatar.CustomizeAppearance(appearance);
                break;
            case "2":
                Console.Write("Enter accessory name: ");
                string accessory = Console.ReadLine();
                avatar.AddAccessory(accessory);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
