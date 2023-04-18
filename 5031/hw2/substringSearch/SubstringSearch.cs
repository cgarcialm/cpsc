using System;
using System.Collections.Generic;

class SubstringSearch
{
    static string PromptUserInput(string inputName)
    {
        string userInput;
        Console.Write(string.Format("Enter a {0}: ", inputName));
        userInput = Console.ReadLine();

        return userInput;
    }

    static void PrintStringMatchResults(List<string> totalStrings, List<string> lookUpStrings, List<int> resultIndexes)
    {
        Console.WriteLine("Number of addition operations in… ");
        var linePattern = "|{0,-20}|{1,-20}|{2,20}|";
        Console.WriteLine(String.Format(linePattern, "S", "U", "Result"));
        for (int i = 0; i < totalStrings.Count; i++)
        {
            Console.WriteLine(String.Format(linePattern, totalStrings[i], lookUpStrings[i], resultIndexes[i]));
        }
    }

    public static int BruteForceStringMatch(string totalString, string lookUpString)
    {
        for (int i = 0; i <= totalString.Length - lookUpString.Length; i++)
        {
            int j = 0;
            while (j < lookUpString.Length && lookUpString[j] == totalString[i + j])
            {
                j++;
            }
            if (j == lookUpString.Length)
            {
                return i;
            }
        }
        return -1;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Substring Search.");
        Console.WriteLine("You'll be asked to input string and then the substring to look up:\n");
        List<string> totalStrings = new List<string>();
        List<string> lookUpStrings = new List<string>();
        List<int> resultIndexes = new List<int>();
        int stringMatchNumber = 0;

        string run = "y";
        do
        {
            totalStrings.Add(PromptUserInput("string"));
            lookUpStrings.Add(PromptUserInput("lookup substring"));
            resultIndexes.Add(BruteForceStringMatch(totalStrings[stringMatchNumber], lookUpStrings[stringMatchNumber]));
            stringMatchNumber++;

            Console.Write("\nDo you want to match more strings? y/n: ");
            run = Console.ReadLine();
            while (run != "y" && run != "n")
            {
                Console.Write("Invalid input. Please enter \"y\" or \"n\": ");
                run = Console.ReadLine();
            }
        } while (run == "y");
        PrintStringMatchResults(totalStrings, lookUpStrings, resultIndexes);
        Console.WriteLine("Goodbye!");
    }
}