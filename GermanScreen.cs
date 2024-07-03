using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

internal class GermanScreen
{
    private Dictionary<int, Translation> translations = new Dictionary<int, Translation>();
    private const string filePath = "translations.txt";

    public GermanScreen()
    {
        LoadTranslations();
    }

    public void DisplayMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Add German Word");
            Console.WriteLine("2. Replace a Word");
            Console.WriteLine("3. Delete a Word");
            Console.WriteLine("4. Search for translation of a word");
            Console.WriteLine("5. View all German Words");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");
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
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void AddGermanWord()
    {
        Console.Clear();
        Console.WriteLine("1. Add German Word");
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
        Console.ReadLine();
    }

    public void ReplaceWord()
    {
        Console.Clear();
        Console.WriteLine("2. Replace a Word");
        Console.Write("Enter the ID of the word to replace: ");
        string idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int id) && translations.ContainsKey(id))
        {
            Console.Write("Enter new German Word: ");
            string newGermanWord = Console.ReadLine();

            if (!string.IsNullOrEmpty(newGermanWord))
            {
                translations[id] = new Translation(translations[id].English, newGermanWord);
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
        string idInput = Console.ReadLine();

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

    public void SearchGermanTranslation()
    {
        Console.Clear();
        Console.WriteLine("4. Search for translation of a word");
        Console.Write("Enter German Word to search: ");
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
        Console.ReadLine();
    }

    public void ViewAllGermanWords()
    {
        Console.Clear();
        Console.WriteLine("5. View all German Words");

        foreach (var item in translations)
        {
            Console.WriteLine($"ID: {item.Key}, English Word: {item.Value.English}, German Translation: {item.Value.German}");
        }
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
        try
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                translations = JsonSerializer.Deserialize<Dictionary<int, Translation>>(json) ?? new Dictionary<int, Translation>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading translations: {ex.Message}");
        }
    }
}

public class Translation
{
    public string English { get; set; }
    public string German { get; set; }

    public Translation(string english, string german)
    {
        English = english;
        German = german;
    }
}
