using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class JournalEntry
{
    public string Prompt { get; set; }
    public string Response { get; set; }
    public string Date { get; set; }

    public JournalEntry(string prompt, string response)
    {
        Prompt = prompt;
        Response = response;
        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public override string ToString()
    {
        return $"{Date}\nPrompt: {Prompt}\nResponse: {Response}\n";
    }
}

public class Journal
{
    private List<JournalEntry> entries;
    private List<string> prompts;

public Journal()
{
    entries = new List<JournalEntry>();
    prompts = new List<string>
    {
        "Share a surprising discovery you made today.",
        "Reflect on a moment that brought you pure joy.",
        "Describe a situation where you found inner strength.",
        "Explore the most powerful emotion you felt during the day.",
        "If you could rewind time, what moment would you revisit and why?",
        "Highlight a personal success or accomplishment from today.",
        "Discuss a setback or challenge you faced and how you handled it."
    };
}

    public void WriteNewEntry()
    {
    string randomPrompt = GetRandomPrompt();
    Console.WriteLine("" + randomPrompt);
    
    Console.Write("> ");
    string response = Console.ReadLine();

    JournalEntry entry = new JournalEntry(randomPrompt, response);
    entries.Add(entry);
    }


    public void DisplayJournal()
    {
        foreach (var entry in entries)
        {
            Console.WriteLine(entry);
        }
    }

    public void SaveJournalToFile()
    {
        Console.Write("Enter the filename to save the journal (do not add extension): ");
        string fileName = Console.ReadLine() + ".csv";

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var entry in entries)
            {
                writer.WriteLine($"{entry.Date},{entry.Prompt},{entry.Response}");
            }
        }

        Console.WriteLine("Journal saved to CSV file successfully!");
    }

    public void LoadJournalFromFile()
    {
        Console.Write("Enter the filename to load the journal (do not add extension): ");
        string fileName = Console.ReadLine() + ".csv";

        entries.Clear(); // This help me to clear existing entries before loading new ones

        try
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string[] parts = reader.ReadLine().Split(',');
                    if (parts.Length == 3)
                    {
                        string date = parts[0];
                        string prompt = parts[1];
                        string response = parts[2];

                        JournalEntry entry = new JournalEntry(prompt, response);
                        entry.Date = date;
                        entries.Add(entry);
                    }
                }
            }

            Console.WriteLine("Journal loaded from CSV file successfully!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. Creating a new journal.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the journal: {ex.Message}");
        }
    }

    public void SaveJournalToJson()
    {
        Console.Write("Enter the filename to save the journal (do not add extension): ");
        string fileName = Console.ReadLine() + ".json";

        string jsonContent = JsonConvert.SerializeObject(entries, Formatting.Indented);

        File.WriteAllText(fileName, jsonContent);

        Console.WriteLine("Journal saved to JSON file successfully!");
    }

    public void LoadJournalFromJson()
    {
        Console.Write("Enter the filename to load the journal (do not add extension): ");
        string fileName = Console.ReadLine() + ".json";

        entries.Clear(); 

        try
        {
            string jsonContent = File.ReadAllText(fileName);

            List<JournalEntry> loadedEntries = JsonConvert.DeserializeObject<List<JournalEntry>>(jsonContent);

            if (loadedEntries != null)
            {
                entries.AddRange(loadedEntries);
                Console.WriteLine("Journal loaded from JSON file successfully!");
            }
            else
            {
                Console.WriteLine("Invalid JSON format. Creating a new journal.");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. Creating a new journal.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the journal: {ex.Message}");
        }
    }

    private string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(prompts.Count);
        return prompts[index];
    }
}

public class Program
{
    static void Main()
    {
        Journal myJournal = new Journal();

        while (true)
        {
            Console.WriteLine("Please select one of the following choices:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a CSV file");
            Console.WriteLine("4. Load the journal from a CSV file");
            Console.WriteLine("5. Save the journal to a JSON file");
            Console.WriteLine("6. Load the journal from a JSON file");
            Console.WriteLine("7. Quit");

            Console.Write("What would you like to do? (1-7): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    myJournal.WriteNewEntry();
                    break;

                case "2":
                    myJournal.DisplayJournal();
                    break;

                case "3":
                    myJournal.SaveJournalToFile();
                    break;

                case "4":
                    myJournal.LoadJournalFromFile();
                    break;

                case "5":
                    myJournal.SaveJournalToJson();
                    break;

                case "6":
                    myJournal.LoadJournalFromJson();
                    break;

                case "7":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                    break;
            }
        }
    }
}
