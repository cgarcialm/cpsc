using System;
using System.Diagnostics;

class MatrixReader {

    string fileName;
    int nodes;
    int[,] array;
    string dotString;

    public MatrixReader(string fileName) {
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
}

class Homework6 {

    static void Main(string[] args) {
        MatrixReader mr = new MatrixReader("adj3.txt");
    }
}