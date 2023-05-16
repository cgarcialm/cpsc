using System;

class MatrixReader {

    string fileName;
    int[,] array;
    string dotString;

    public MatrixReader(string fileName) {
        this.fileName = "input/" + fileName;
        to2DArray();
        createDotString();
    }

    private void to2DArray() {
        string input = File.ReadAllText(fileName);
        string[] stringRows = input.Split('\n');
        int nodes = stringRows.Length;

        array = new int[nodes, nodes];
        for(int i = 0; i < nodes; i++) {
            string[] stringCols = stringRows[i].Split('\t');
            for(int j = 0; j < nodes; j++) {
                array[i, j] = Int32.Parse(stringCols[j]);
            }
        }
    }

    private void createDotString() {
        dotString = "diagraph D { \n";
        char[] az = Enumerable.Range('a', array.Length).Select(i => (Char)i).ToArray();
        for (int i = 0; i < array.Length; i++) {
            for (int j = 0; j < array.Length; j++) {
                dotString += az[i] + "->" + az[j] + "\n";
            }
        }
        dotString += "}";
    }
}

class Homework6 {

    static void Main(string[] args) {
        MatrixReader mr = new MatrixReader("adj1.txt");
    }
}