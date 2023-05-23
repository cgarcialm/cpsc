/******************************************************************************

 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: May 16 2023
 * Platform: MacOS Monterrey. Version 12.0.1.
 * Version: 1.0
 * Purpose: Implementation of GraphVizGenerator program.

Welcome to GraphVizGenerator. 
Please follow the intructions for setup before running the program:

    ========================== INSTRUCTIONS FOR SETUP ==========================
    1. Create an "input" folder in the same directory level as this program
    2. Place all input files in input folder (both test files and homework 
    examples)
    3. Create an "output" folder in the same directory level as this program
    ============================================================================

GraphVizGenerator is a program that takes a text input with a matrix and uses 
GraphViz toolkit to create a diagram of the graph that it represents.
Main executes GraphVizGenerator both for a set of sample inputs and test inputs.

*******************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// MatrixException implements an Exception that prints an error if the input 
/// matrix does not comply with the requirements of GraphVizGenerator
/// </summary>
class MatrixException : Exception
{
    public MatrixException()
    {
    }
    public MatrixException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// GraphVizGenerator implements a program to take a text input with a square matrix 
/// of 0 and 1 values and generate the corresponding representation of the graph in 
/// a diagram.
/// </summary>
class GraphVizGenerator
{

    string fileName;
    int nodes;
    bool symmetric = true;
    int[,] array;
    string dotGraphString;

    /// <summary>
    /// Constructor of GraphVizGenerator
    /// </summary>
    /// <param name="fileName">Name of text file that must be in input folder</param>
    public GraphVizGenerator(string fileName)
    {
        this.fileName = fileName;
        to2DArray();
        isMatrixSymmetric();
        createdotGraphString();
    }

    /// <summary>
    /// Transforms text input of square matrix into a 2D array and stores it
    /// in member variable array.
    /// </summary>
    private void to2DArray()
    {
        string input = File.ReadAllText("input/" + fileName).Trim('\r', '\n');
        if (input == "")
        {
            throw new MatrixException("Matrix must not be empty.");
        }
        string[] stringRows = input.Split('\n');
        nodes = stringRows.Length;

        array = new int[nodes, nodes];
        int totalEdges = 0;

        for (int i = 0; i < nodes; i++)
        {
            string[] stringCols = stringRows[i].Replace("    ", "\t").Split('\t');
            if (stringCols.Length != nodes)
            {
                throw new MatrixException("Matrix must be square. Row " + i + " has " + stringCols.Length + " columns.");
            }
            for (int j = 0; j < nodes; j++)
            {
                if (stringCols[j] != "0" && stringCols[j] != "1")
                {
                    throw new MatrixException("All values in matrix must be 0 or 1.");
                }
                array[i, j] = Int32.Parse(stringCols[j]);
                totalEdges += array[i, j];
            }
        }
        if (totalEdges == 0)
        {
            throw new MatrixException("Matrix must have at least one edge between nodes.");
        }
    }

    /// <summary>
    /// Checks if matrix stored in array is symmetric and updates member variable 
    /// symmetric.
    /// </summary>
    private void isMatrixSymmetric()
    {
        for (int i = 0; i < nodes; i++)
        {
            for (int j = i + 1; j < nodes; j++)
            {
                if (array[i, j] != array[j, i])
                {
                    symmetric = false;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Creates a string representing the graph diagram with the format 
    /// required by GraphViz toolkit.
    /// Stores the corresponding string in member variable dotGraphString.
    /// </summary>
    private void createdotGraphString()
    {
        dotGraphString = symmetric ? "graph { \n" : "digraph { \n";
        string edge = symmetric ? "--" : "->";
        char[] az = Enumerable.Range('a', array.Length).Select(i => (Char)i).ToArray();

        for (int i = 0; i < nodes; i++)
        {
            if (symmetric)
            {
                for (int j = i; j < nodes; j++)
                {
                    if (array[i, j] == 1)
                    {
                        dotGraphString += az[i] + edge + az[j] + "\n";
                    }
                }
            }
            else
            {
                for (int j = 0; j < nodes; j++)
                {
                    if (array[i, j] == 1)
                    {
                        dotGraphString += az[i] + edge + az[j] + "\n";
                    }
                }
            }
        }
        dotGraphString += " }";
    }

    /// <summary>
    /// Cretes the shell command to be ran in terminal to create the graph diagram.
    /// </summary>
    /// <returns></returns>
    private string createShellCommand()
    {
        string strCmdText = "echo '" + dotGraphString + "' | dot -Tpng > output/" 
                            + fileName.Substring(0, fileName.LastIndexOf(".")) + ".png";

        return strCmdText;
    }

    /// <summary>
    /// Runs GraphVizGenerator: creates shell command and runs it in terminal,
    /// storing png outputs to output folder.
    /// </summary>
    public string run()
    {
        string command = createShellCommand().Replace("\"", "\"\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        proc.WaitForExit();

        return dotGraphString;
    }
}

/// <summary>
/// Homework6 runs GraphVizGenerator on a set of sample inputs and test inputs.
/// </summary>
class Homework6
{

    /// <summary>
    /// Runs GraphVizGenerator on fiven input file.
    /// </summary>
    /// <param name="fileName">Name of text file that must be in input folder</param>
    /// <returns></returns>
    static string runOnInputsList(string fileName)
    {
        GraphVizGenerator mr = new GraphVizGenerator(fileName);

        return mr.run();
    }

    /// <summary>
    /// Runs GraphVizGenerator for sample files.
    /// </summary>
    static void runSampleInputs()
    {
        List<string> sampleInputs = new List<string> { 
            "adj1.txt", "adj2.txt", "adj3.txt", "adj4.txt" 
            };
        foreach (string input in sampleInputs)
        {
            runOnInputsList(input);
        }
    }

    /// <summary>
    /// Runs GraphVizGenerator for test files and prints into console the output of each test.
    /// </summary>
    static void runTests()
    {
        const int NTESTS = 10;
        List<string> testInputs = new List<string>();
        for (int i = 1; i <= NTESTS; i++)
        {
            testInputs.Add("test" + i + ".txt");
        }

        Console.WriteLine("Welcome to the GraphViz Generator.\n");
        const string LINEPATTERN = "|{0,5}|{1,10}|{2,10}|{3,40}|";
        Console.WriteLine(String.Format(LINEPATTERN, "Test", "File Name", "Exception", "Output"));
        Console.WriteLine("+" + new string('-', 5) + "+" + new string('-', 10) + "+" 
                          + new string('-', 10) + "+" + new string('-', 40) + "+");

        List<bool> exceptions = new List<bool>();
        List<string> outputs = new List<string>();
        for (int i = 0; i < testInputs.Count; i++)
        {
            try
            {
                outputs.Add(runOnInputsList(testInputs[i]).Replace("\n", " "));
                exceptions.Add(false);
            }
            catch (MatrixException e)
            {
                outputs.Add(e.ToString().Substring(17, e.ToString().Length - 17).Replace("\n", ""));
                exceptions.Add(true);
            }

            Console.WriteLine(String.Format(
                LINEPATTERN,
                i + 1,
                testInputs[i],
                exceptions[i],
                outputs[i].Length > 40 ? outputs[i].Substring(0, 36) + "..." : outputs[i]
            ));
        }
        Console.WriteLine("\nGoodbye!");
    }

    /// <summary>
    /// Main entry of the program. Runs GraphVizGenerator on sample inputs and test inputs.
    /// its corresponding graph diagrams.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        runSampleInputs();
        runTests();
    }
}