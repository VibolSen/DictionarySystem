using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DictionarySystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int enteredNumber;
            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("DICTIONARY");
                Console.WriteLine("----------");
                Console.WriteLine();
                Console.WriteLine("1.English - German");
                Console.WriteLine("2.German - English");
                Console.WriteLine("0.Exit Application");
                Console.WriteLine();
                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();
                if (!int.TryParse(choice, out enteredNumber))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (enteredNumber)
                {
                    case 0:
                        return;
                    case 1:
                        EnglishScreen englishScreen = new();
                        englishScreen.DisplayMenu();
                        break;
                    case 2:
                        GermanScreen germanScreen = new();
                        germanScreen.DisplayMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (enteredNumber != 0);
        }
    }
}
