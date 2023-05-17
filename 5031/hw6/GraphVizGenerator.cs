using System;
using System.Diagnostics;

class MatrixException : Exception {
    public MatrixException()
    {
    }
    public MatrixException(string message)
        : base(message)
    {
    }
}

class GraphVizGenerator {

    string fileName;
    int nodes;
    bool symmetric = true;
    int[,] array;
    string dotString;

    public GraphVizGenerator(string fileName) {
        this.fileName = fileName;
        to2DArray();
        isMatrixSymmetric();
        createDotString();
    }

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
                try {
                    array[i, j] = Int32.Parse(stringCols[j]);
                } catch (Exception e) {
                    throw new MatrixException("Value \"" + stringCols[j] + "\" is not an integer.");
                }
            }
        }
    }

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

    private void createDotString() {
        dotString = symmetric ? "graph { \n" : "digraph { \n";
        string edge = symmetric ? "--" : "->";
        char[] az = Enumerable.Range('a', array.Length).Select(i => (Char)i).ToArray();

        for (int i = 0; i < nodes; i++) {
            if (symmetric) {
                for (int j = i; j < nodes; j++) {
                    if (array[i, j] == 1) {
                        dotString += az[i] + edge + az[j] + "\n";
                    }
                }
            } else {
                for (int j = 0; j < nodes; j++) {
                    if (array[i, j] == 1) {
                        dotString += az[i] + edge + az[j] + "\n";
                    }
                }
            }
        }
        dotString += " }";
    }

    private string createShellCommand() {
        string strCmdText = "echo '" + dotString + "' | dot -Tpng > output/" + fileName.Substring(0, fileName.LastIndexOf(".")) + ".png";
        File.WriteAllText("output/" + fileName , strCmdText);

        return strCmdText;
    }

    public string generate()
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

        return proc.StandardOutput.ReadToEnd();
    }
    
}

class Homework6 {

    static void Main(string[] args) {

        List<string> testInputs = new List<string> {"adj1.txt", "adj2.txt", "adj3.txt", "adj4.txt"};

        foreach(string input in testInputs) {
            GraphVizGenerator mr = new GraphVizGenerator(input);
            mr.generate();
        }
    }
}