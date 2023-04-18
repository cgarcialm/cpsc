/******************************************************************************

Welcome to SubstringSearch.
SubstringSearch is a program that searches for substrings within string all given by the user.

*******************************************************************************/

using System;
using System.Collections.Generic;

/// <summary>
/// SubstringSearch
/// </summary>
class SubstringSearch
{
    /// <summary>
    /// Prompts the user for input for a string.
    /// </summary>
    /// <param name="inputName">Input description</param>
    /// <returns></returns>
    static string PromptUserInput(string inputName)
    {
        string userInput;
        Console.Write(string.Format("Enter a {0}: ", inputName));
        userInput = Console.ReadLine();

        return userInput;
    }

    /// <summary>
    /// Prints into console a table with the results of the subtring search.
    /// </summary>
    /// <param name="totalStrings">The complete string</param>
    /// <param name="lookUpStrings">The subtring to look for</param>
    /// <param name="resultIndexes">The index where it was found</param>
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

    /// <summary>
    /// Searches for the first appearance of a substring in a string. If it's not found or substring is empty returns -1.
    /// </summary>
    /// <param name="totalString">The complete string</param>
    /// <param name="lookUpString">The subtring to look for</param>
    /// <returns></returns>
    public static int BruteForceStringMatch(string totalString, string lookUpString)
    {   
        if(lookUpString.Length > 0) {
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
        }
        return -1;
    }

    /// <summary>
    /// The main entry point of the program. Asks the user for input for a string and a substring to look for until the user wants to stop.
    /// </summary>
    /// <param name="args"></param>
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