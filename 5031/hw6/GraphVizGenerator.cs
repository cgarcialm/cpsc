using System;
using System.Diagnostics;

class GraphVizGenerator {

    string fileName;
    int nodes;
    int[,] array;
    string dotString;

    public GraphVizGenerator(string fileName) {
        this.fileName = fileName; // TODO: check if txt
        to2DArray();
        createDotString();
    }

    private void to2DArray() {
        string input = File.ReadAllText("input/" + fileName);
        string[] stringRows = input.Split('\n');
        nodes = stringRows.Length - 1;

        array = new int[nodes, nodes];
        for(int i = 0; i < nodes; i++) {
            string[] stringCols = stringRows[i].Split('\t');
            for(int j = 0; j < nodes; j++) {
                array[i, j] = Int32.Parse(stringCols[j]); // TODO: check if int
            }
        }
    }

    private void createDotString() {
        dotString = "digraph { \n";
        char[] az = Enumerable.Range('a', array.Length).Select(i => (Char)i).ToArray();
        for (int i = 0; i < nodes; i++) {
            for (int j = 0; j < nodes; j++) {
                if (array[i, j] == 1) {
                    dotString += az[i] + "->" + az[j] + "\n";
                }
            }
        }
        dotString += " }";
    }

    private string createShellCommand() {
        string strCmdText = "echo '" + dotString + "' | dot -Tpng > output/" + fileName.Substring(0, fileName.LastIndexOf(".txt")) + ".png";
        File.WriteAllText("output/" + fileName , strCmdText);

        return strCmdText;
    }

    public string generate()
    {
        // according to: https://stackoverflow.com/a/15262019/637142
        // thans to this we will pass everything as one command
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

        foreach(string testInput in testInputs) {
            GraphVizGenerator mr = new GraphVizGenerator(testInput);
            mr.generate();
        }
    }
}