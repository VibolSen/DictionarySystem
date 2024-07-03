using ExamTemplate;
using System.Text.Json;

internal class EnglishScreen
{
    private Dictionary<int, Translation> translations = new Dictionary<int, Translation>();
    private const string filePath = "translations.txt";

    public EnglishScreen()
    {
        LoadTranslations();
    }

    public void DisplayMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Add English Word");
            Console.WriteLine("2. Replace a Word");
            Console.WriteLine("3. Delete a Word");
            Console.WriteLine("4. Search for translation of a word");
            Console.WriteLine("5. View all English Words");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            if (!int.TryParse(choice, out int enteredNumber))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (enteredNumber)
            {
                case 0:
                    return;
                case 1:
                    AddEnglishWord();
                    break;
                case 2:
                    ReplaceWord();
                    break;
                case 3:
                    DeleteWord();
                    break;
                case 4:
                    SearchTranslation();
                    break;
                case 5:
                    ViewAllEnglishWords();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void AddEnglishWord()
    {
        Console.Clear();
        Console.WriteLine("1. Add English Word");
        Console.Write("Enter English Word: ");
        string? englishWord = Console.ReadLine();
        Console.Write("Enter German Translation: ");
        string? germanWord = Console.ReadLine();

        if (!string.IsNullOrEmpty(englishWord) && !string.IsNullOrEmpty(germanWord))
        {
            int id = translations.Count + 1;
            translations[id] = new Translation(englishWord, germanWord);
            SaveTranslations();
            Console.WriteLine("Word added successfully!");
        }
        else
        {
            Console.WriteLine("Input cannot be empty.");
        }
        Console.ReadLine();
    }

    public void ReplaceWord()
    {
        Console.Clear();
        Console.WriteLine("2. Replace a Word");
        Console.Write("Enter the ID of the word to replace: ");
        string? idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int id) && translations.ContainsKey(id))
        {
            Console.Write("Enter new English Word: ");
            string? newEnglishWord = Console.ReadLine();
            Console.Write("Enter new German Translation: ");
            string? newGermanWord = Console.ReadLine();

            if (!string.IsNullOrEmpty(newEnglishWord) && !string.IsNullOrEmpty(newGermanWord))
            {
                translations[id] = new Translation(newEnglishWord, newGermanWord);
                SaveTranslations();
                Console.WriteLine("Word replaced successfully!");
            }
            else
            {
                Console.WriteLine("Input cannot be empty.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID or word not found.");
        }
        Console.ReadLine();
    }

    public void DeleteWord()
    {
        Console.Clear();
        Console.WriteLine("3. Delete a Word");
        Console.Write("Enter the ID of the word to delete: ");
        string? idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int id) && translations.ContainsKey(id))
        {
            translations.Remove(id);
            SaveTranslations();
            Console.WriteLine("Word deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid ID or word not found.");
        }
        Console.ReadLine();
    }

    public void SearchTranslation()
    {
        Console.Clear();
        Console.WriteLine("4. Search for translation of a word");
        Console.Write("Enter English Word to search: ");
        string? searchWord = Console.ReadLine();

        bool found = false;
        foreach (var translation in translations.Values)
        {
            if (translation.English.Equals(searchWord, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Translation: {translation.German}"); // Corrected here
                found = true;
                break;
            }
        }

        if (!found)
        {
            Console.WriteLine("Word not found.");
        }
        Console.ReadLine();
    }

    public void ViewAllEnglishWords()
    {
        Console.Clear();
        Console.WriteLine("5. View all English Words");

        if (translations.Count == 0)
        {
            Console.WriteLine("No words available.");
            Console.ReadLine();
            return;
        }

        // Determine maximum lengths for English and German words
        int maxEnglishLength = Math.Max(translations.Max(t => t.Value.English.Length), "English".Length);
        int maxGermanLength = Math.Max(translations.Max(t => t.Value.German.Length), "German".Length);

        // Construct table borders dynamically
        string topBorder = $"┌─────┬{new string('─', maxEnglishLength)}┬{new string('─', maxGermanLength)}┐";
        string header = $"│ ID  │ {"English".PadRight(maxEnglishLength)} │ {"German".PadRight(maxGermanLength)} │";
        string separator = $"├─────┼{new string('─', maxEnglishLength)}┼{new string('─', maxGermanLength)}┤";
        string bottomBorder = $"└─────┴{new string('─', maxEnglishLength)}┴{new string('─', maxGermanLength)}┘";

        // Print the table with dynamic column widths
        Console.WriteLine(topBorder);
        Console.WriteLine(header);
        Console.WriteLine(separator);

        foreach (var item in translations)
        {
            Console.WriteLine($"│ {item.Key,-4} │ {item.Value.English,-maxEnglishLength} │ {item.Value.German,-maxGermanLength} │");
        }

        Console.WriteLine(bottomBorder);
        Console.ReadLine();
    }

    private void SaveTranslations()
    {
        var json = JsonSerializer.Serialize(translations, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    private void LoadTranslations()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            translations = JsonSerializer.Deserialize<Dictionary<int, Translation>>(json) ?? new Dictionary<int, Translation>();
        }
        else
        {
            translations = new Dictionary<int, Translation>();
        }
    }
}
