/******************************************************************************

 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: May 16 2023
 * Platform: MacOS Monterrey. Version 12.0.1.
 * Version: 1.0
 * Purpose: Implementation of GraphVizGenerator program.

Welcome to GraphVizGenerator. GraphVizGenerator is a program that takes a text 
input with a matrix and uses GraphViz toolkit to create a diagram of the graph 
that it represents.

*******************************************************************************/

using System;
using System.Diagnostics;

/// <summary>
/// MatrixException implements an Exception that prints an error if the input 
/// matrix does not comply with the requirements of GraphVizGenerator
/// </summary>
class MatrixException : Exception {
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
class GraphVizGenerator {

    string fileName;
    int nodes;
    bool symmetric = true;
    int[,] array;
    string dotGraphString;

    public GraphVizGenerator(string fileName) {
        this.fileName = fileName;
        to2DArray();
        isMatrixSymmetric();
        createdotGraphString();
    }

    /// <summary>
    /// Transforms text input of square matrix into a 2D array and stores it
    /// in member variable array.
    /// </summary>
    private void to2DArray() {
        string input = File.ReadAllText("input/" + fileName).Trim('\r', '\n');
        string[] stringRows = input.Split('\n'); 
        nodes = stringRows.Length;
        if(nodes<2) {
            throw new MatrixException("Matrix must have at least 2 nodes");
        }

        array = new int[nodes, nodes];
        int totalEdgesBtwNodes = 0;
        
        for(int i = 0; i < nodes; i++) {
            string[] stringCols = stringRows[i].Replace("    ", "\t").Split('\t');
            if(stringCols.Length != nodes) {
                throw new MatrixException("Matrix must be square.");
            }
            for(int j = 0; j < nodes; j++) {
                if(stringCols[j] != "0" && stringCols[j] != "1") {
                    throw new MatrixException("All values in matrix must be 0 or 1.");
                }
                array[i, j] = Int32.Parse(stringCols[j]);
                if(i != j) {
                    totalEdgesBtwNodes += array[i, j];
                }
            }
        }
        if(totalEdgesBtwNodes == 0) {
            throw new MatrixException("Matrix must have at least one edge between different nodes.");
        }
    }

    /// <summary>
    /// Checks if matrix stored in array is symmetric and updates member variable 
    /// symmetric.
    /// </summary>
    private void isMatrixSymmetric() {
        for(int i = 0; i < nodes; i++) {
            for(int j = i + 1; j < nodes; j++) {
                if(array[i, j] != array[j, i]) {
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
    private void createdotGraphString() {
        dotGraphString = symmetric ? "graph { \n" : "digraph { \n";
        string edge = symmetric ? "--" : "->";
        char[] az = Enumerable.Range('a', array.Length).Select(i => (Char)i).ToArray();

        for (int i = 0; i < nodes; i++) {
            if (symmetric) {
                for (int j = i; j < nodes; j++) {
                    if (array[i, j] == 1) {
                        dotGraphString += az[i] + edge + az[j] + "\n";
                    }
                }
            } else {
                for (int j = 0; j < nodes; j++) {
                    if (array[i, j] == 1) {
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
    private string createShellCommand() {
        string strCmdText = "echo '" + dotGraphString + "' | dot -Tpng > output/" + fileName.Substring(0, fileName.LastIndexOf(".")) + ".png";

        return strCmdText;
    }

    /// <summary>
    /// Runs GraphVizGenerator: creates shell command and runs it in terminal,
    /// storing png outputs to output folder.
    /// </summary>
    public string run()
    {
        string command = createShellCommand().Replace("\"","\"\"");

        var proc = new Process {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \""+ command + "\"",
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
/// Homework6 runs GraphVizGenerator on a set of inputs stored in List testInputs.
/// </summary>
class Homework6 {

    static string runOnInputsList(string fileName) {
        GraphVizGenerator mr = new GraphVizGenerator(fileName);
        
        return mr.run();
    }

    static void runTests() {
        const int NTESTS = 8;
        List<string> testInputs = new List<string>();
        for(int i = 1; i <= NTESTS; i++) {
            testInputs.Add("test" + i + ".txt");
        }
        List<bool> expectedToBreak  = new List<bool> {false, false, false, true, true, true, true, false};

        Console.WriteLine("Welcome to the GraphViz Generator.\n");
        const string LINEPATTERN = "|{0,5}|{1,10}|{2,10}|{3,40}|";
        Console.WriteLine(String.Format(LINEPATTERN, "Test", "File Name", "Exception", "Output"));
        Console.WriteLine("+"+new string('-', 5)+"+"+new string('-', 10)+"+"+new string('-', 10)+"+"+new string('-', 40)+"+");

        List<string> outputs = new List<string>();
        for(int i = 0; i < testInputs.Count; i++) {
            if(expectedToBreak[i]) {
                try{
                    runOnInputsList(testInputs[i]);
                } catch (MatrixException e) {
                    outputs.Add(e.ToString().Substring(17, e.ToString().Length-17).Replace("\n", ""));
                }
            } else {
                outputs.Add(runOnInputsList(testInputs[i]).Replace("\n", " "));
            }
            Console.WriteLine(String.Format(
                LINEPATTERN,
                i+1,
                testInputs[i],
                expectedToBreak[i],
                outputs[i].Length > 40 ? outputs[i].Substring(0, 36) + "..." : outputs[i]
            ));
        }
        Console.WriteLine("\nGoodbye!");
    }

    /// <summary>
    /// Main entry of the program. Creates a List of input file names and generates 
    /// its corresponding graph diagrams.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args) {
        List<string> sampleInputs = new List<string> {"adj1.txt", "adj2.txt", "adj3.txt", "adj4.txt"};
        foreach(string input in sampleInputs) {
            runOnInputsList(input);
        }

        runTests();
    }
}