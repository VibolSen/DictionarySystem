using DictionarySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

internal class GermanScreen
{
    private Dictionary<int, Translation> translations;
    private const string filePath = "translations.txt";

    public void DisplayMenu(Dictionary<int, Translation> translationDictionary)
    {
        translations = translationDictionary;
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("========= German to English ========");
            Console.WriteLine("====================================");
            Console.WriteLine();
            Console.WriteLine("1. Add German Word");
            Console.WriteLine("2. Replace a Word");
            Console.WriteLine("3. Delete a Word");
            Console.WriteLine("4. Search for Translation of a Word");
            Console.WriteLine("5. View all German Words");
            Console.WriteLine("6. Load all data from file");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice(0-6): ");
            string choice = Console.ReadLine();

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
                    AddGermanWord();
                    break;
                case 2:
                    ReplaceWord();
                    break;
                case 3:
                    DeleteWord();
                    break;
                case 4:
                    SearchGermanTranslation();
                    break;
                case 5:
                    ViewAllGermanWords();
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

    public void AddGermanWord()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("-------- 1. Add German Word --------");
        Console.WriteLine("====================================");
        Console.WriteLine();
        Console.Write("Enter German Word: ");
        string germanWord = Console.ReadLine();
        Console.Write("Enter English Word: ");
        string englishWord = Console.ReadLine();

        if (!string.IsNullOrEmpty(germanWord) && !string.IsNullOrEmpty(englishWord))
        {
            int id = translations.Count + 1;
            translations[id] = new Translation(englishWord, germanWord); // Assuming Translation constructor order is English, German
            SaveTranslations();
            Console.WriteLine("Word added successfully!");
        }
        else
        {
            Console.WriteLine("Input cannot be empty.");
        }
        Console.WriteLine();
        Console.Write("Press any key to back to menu. . . . ");
        Console.ReadLine();
    }

    public void ReplaceWord()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("-------- 2. Replace a Word --------");
        Console.WriteLine("===================================");
        Console.WriteLine();
        Console.Write("Enter the ID of the word to replace: ");
        string idInput = Console.ReadLine();

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
                Console.Write("Enter new German Word: ");
                string? newGermanWord = Console.ReadLine();
                Console.Write("Enter new English Translation: ");
                string? newEnglishWord = Console.ReadLine();

                if (!string.IsNullOrEmpty(newGermanWord) && !string.IsNullOrEmpty(newEnglishWord))
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
        Console.WriteLine();
        Console.Write("Press any key to back to menu. . . . ");
        Console.ReadLine();
    }

    public void DeleteWord()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("-------- 3. Delete a Word --------");
        Console.WriteLine("==================================");
        Console.WriteLine();
        Console.Write("Enter the ID of the word to delete: ");
        string idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int id) && translations.ContainsKey(id))
        {
            var wordToDelete = translations[id];

            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"English: {wordToDelete.English}");
            Console.WriteLine($"German: {wordToDelete.German}");
            Console.WriteLine();

            Console.Write($"Are you sure you want to delete the word with ID {id}? (Y/N)");
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
        Console.WriteLine();
        Console.Write("Press any key to back to menu. . . . ");
        Console.ReadLine();
    }

    public void SearchGermanTranslation()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("-------- 4. Search for Translation of a Word --------");
        Console.WriteLine("=====================================================");
        Console.WriteLine();
        Console.Write("Enter German Word to search: ");
        Console.WriteLine();
        string searchWord = Console.ReadLine();

        bool found = false;
        foreach (var translation in translations.Values)
        {
            if (translation.German.Equals(searchWord, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Translation: {translation.English}");
                found = true;
                break;
            }
        }

        if (!found)
        {
            Console.WriteLine("Word not found.");
        }
        Console.WriteLine();
        Console.Write("Press any key to back to menu. . . . ");
        Console.ReadLine();
    }

    public void ViewAllGermanWords()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("---------- 5. View all German Words ----------");
        Console.WriteLine();
        Console.WriteLine("{0,-8} {1,-20} {2,-20}", "ID", "German", "English");
        Console.WriteLine("==============================================");


        foreach (var item in translations)
        {
            Console.WriteLine($"{item.Key,-8} {item.Value.German,-20} {item.Value.English,-20}");
        }
        Console.WriteLine();
        Console.Write("Press any key to back to menu. . . . ");
        Console.ReadLine();
    }

    private void SaveTranslations()
    {
        try
        {
            var json = JsonSerializer.Serialize(translations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving translations: {ex.Message}");
        }
    }

    private void LoadTranslations()
    {
        Console.Clear();
        Console.WriteLine();
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            translations = JsonSerializer.Deserialize<Dictionary<int, Translation>>(json) ?? new Dictionary<int, Translation>();

            Console.WriteLine("-------- 6. Load All Data From File --------");
            Console.WriteLine("============================================");
            Console.WriteLine();
            Console.WriteLine("Translations loaded successfully. Here are the current translations:");
            Console.WriteLine();

            // Define column widths
            int idColumnWidth = 5;
            int germanColumnWidth = 20;
            int englishColumnWidth = 20;

            // Print table headers
            Console.WriteLine($"{"ID".PadRight(idColumnWidth)} {"German".PadRight(germanColumnWidth)} {"English".PadRight(englishColumnWidth)}");
            Console.WriteLine(new string('-', idColumnWidth + germanColumnWidth + englishColumnWidth + 2));

            // Print each translation in a formatted manner
            foreach (var translation in translations)
            {
                string id = translation.Key.ToString().PadRight(idColumnWidth);
                string germanWord = translation.Value.German.PadRight(germanColumnWidth);
                string englishWord = translation.Value.English.PadRight(englishColumnWidth);
                Console.WriteLine($"{id} {germanWord} {englishWord}");
            }
        }
        else
        {
            translations = new Dictionary<int, Translation>();
            Console.WriteLine("No translations file found. Starting with an empty dictionary.");
        }
        Console.WriteLine();
        Console.Write("Press any key to back to menu. . . . ");
        Console.ReadLine(); // To pause the screen and let the user read the output
    }
}
