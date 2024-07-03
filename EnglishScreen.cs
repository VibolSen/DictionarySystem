using DictionarySystem;
using System.Text.Json;
using System.Xml.Linq;

internal class EnglishScreen
{
    private Dictionary<int, Translation> translations = new Dictionary<int, Translation>();
    private const string filePath = "translations.txt";

    public void DisplayMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("English to German");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.WriteLine("1. Add English Word");
            Console.WriteLine("2. Replace a Word");
            Console.WriteLine("3. Delete a Word");
            Console.WriteLine("4. Search for translation of a word");
            Console.WriteLine("5. View all English Words");
            Console.WriteLine("6. Load all data from file");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
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
                case 6:
                    LoadTranslations();
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
        Console.WriteLine();
        Console.WriteLine("1. Add English Word");
        Console.WriteLine("==============================================");
        Console.WriteLine();
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
        Console.WriteLine();
        Console.WriteLine("2. Replace a Word");
        Console.WriteLine("==============================================");
        Console.WriteLine();
        Console.Write("Enter the ID of the word to replace: ");
        Console.WriteLine();
        string? idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int id) && translations.ContainsKey(id))
        {
            var currentTranslation = translations[id];
            Console.WriteLine($"ID       : {id}");
            Console.WriteLine($"English  : {currentTranslation.English}");
            Console.WriteLine($"German   : {currentTranslation.German}");
            Console.WriteLine();

            Console.Write("Are you sure you want to replace this word? (Y/N): ");
            string? confirmation = Console.ReadLine();

            if (confirmation != null && confirmation.ToUpper() == "Y")
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
                Console.WriteLine("Operation canceled.");
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
        Console.WriteLine();
        Console.WriteLine("3. Delete a Word");
        Console.WriteLine("==============================================");
        Console.WriteLine();
        Console.Write("Enter the ID of the word to delete: ");
        string idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int id) && translations.ContainsKey(id))
        {
            var wordToDelete = translations[id];

            Console.WriteLine();
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"English: {wordToDelete.English}");
            Console.WriteLine($"German: {wordToDelete.German}");
            Console.WriteLine();

            Console.WriteLine($"Are you sure you want to delete the word with ID {id}? (Y/N)");
            Console.WriteLine();
            string choice = Console.ReadLine()?.Trim().ToUpper();

            if (choice == "Y")
            {
                translations.Remove(id);
                SaveTranslations();
                Console.WriteLine("Word deleted successfully!");
            }
            else if (choice == "N")
            {
                Console.WriteLine("Deletion canceled.");
            }
            else
            {
                Console.WriteLine("Invalid choice. Deletion canceled.");
            }
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
        Console.WriteLine();
        Console.WriteLine("4. Search for translation of a word");
        Console.WriteLine("==============================================");
        Console.WriteLine();
        Console.Write("Enter English Word to search: ");
        Console.WriteLine();
        string? searchWord = Console.ReadLine();

        bool found = false;
        foreach (var translation in translations.Values)
        {
            if (translation.English.Equals(searchWord, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Translation: {translation.German}"); 
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
        Console.WriteLine();
        Console.WriteLine("5. View all English Words");
        Console.WriteLine();
        Console.WriteLine("{0,-8} {1,-20} {2,-20}", "ID", "English", "German");
        Console.WriteLine("==============================================");

        foreach (var item in translations)
        {
            Console.WriteLine($"{item.Key,-8} {item.Value.English,-20} {item.Value.German,-20}");
        }

        Console.ReadLine();
    }
    private void SaveTranslations()
    {
        var json = JsonSerializer.Serialize(translations, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    private void LoadTranslations() // Open all data from file .text
    {
        Console.Clear();
        Console.WriteLine();
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            translations = JsonSerializer.Deserialize<Dictionary<int, Translation>>(json) ?? new Dictionary<int, Translation>();

            Console.WriteLine("6. Load All Data From File");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.WriteLine("Translations loaded successfully. Here are the current translations:");
            Console.WriteLine();

            // Define column widths
            int idColumnWidth = 5;
            int englishColumnWidth = 20;
            int germanColumnWidth = 20;

            // Print table headers
            Console.WriteLine($"{"ID".PadRight(idColumnWidth)} {"English".PadRight(englishColumnWidth)} {"German".PadRight(germanColumnWidth)}");
            Console.WriteLine(new string('-', idColumnWidth + englishColumnWidth + germanColumnWidth + 2));

            // Print each translation in a formatted manner
            foreach (var translation in translations)
            {
                string id = translation.Key.ToString().PadRight(idColumnWidth);
                string englishWord = translation.Value.English.PadRight(englishColumnWidth);
                string germanWord = translation.Value.German.PadRight(germanColumnWidth);
                Console.WriteLine($"{id} {englishWord} {germanWord}");
            }
            Console.WriteLine();
            Console.Write("Press any key to back to menu. . . . ");
        }
        else
        {
            translations = new Dictionary<int, Translation>();
            Console.WriteLine("No translations file found. Starting with an empty dictionary.");
        }
        Console.ReadLine(); // To pause the screen and let the user read the output
    }

}
