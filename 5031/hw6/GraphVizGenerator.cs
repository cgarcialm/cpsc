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

        array = new int[nodes, nodes];
        
        for(int i = 0; i < nodes; i++) {
            string[] stringCols = stringRows[i].Split('\t');
            if(stringCols.Length != nodes) {
                throw new MatrixException("Matrix in input " + fileName + " should be square.");
            }
            for(int j = 0; j < nodes; j++) {
                if(stringCols[j] != "0" && stringCols[j] != "1") {
                    throw new MatrixException("Value \"" + stringCols[j] + "\" in matrix must be 0 or 1.");
                }
                array[i, j] = Int32.Parse(stringCols[j]);
            }
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
        File.WriteAllText("output/" + fileName , strCmdText); // TODO: Drop this export

        return strCmdText;
    }

    /// <summary>
    /// Runs GraphVizGenerator: creates shell command and runs it in terminal,
    /// storing png outputs to output folder.
    /// </summary>
    public void run()
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
    }
}

/// <summary>
/// Homework6 runs GraphVizGenerator on a set of inputs stored in List testInputs.
/// </summary>
class Homework6 {
    /// <summary>
    /// Main entry of the program. Creates a List of input file names and generates 
    /// its corresponding graph diagrams.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args) {
        List<string> testInputs = new List<string> {"adj1.txt", "adj2.txt", "adj3.txt", "adj4.txt"};

        foreach(string input in testInputs) {
            GraphVizGenerator mr = new GraphVizGenerator(input);
            mr.run();
        }
    }
}