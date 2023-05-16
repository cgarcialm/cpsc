using System;

class MatrixReader {

    string fileName;
    int[,] array;

    public MatrixReader(string fileName) {
        this.fileName = "input/" + fileName;
        to2DArray();
    }

    private void to2DArray() {
        string input = File.ReadAllText(fileName);
        string[] stringRows = input.Split('\n');
        int rows = stringRows.Length;
        int cols = stringRows[0].Split('\t').Length;

        array = new int[rows, cols];
        for(int i = 0; i < rows; i++) {
            string[] stringCols = stringRows[i].Split('\t');
            for(int j = 0; j < cols; j++) {
                array[i, j] = Int32.Parse(stringCols[j]);
            }
        }
    }
}

class Homework6 {

    static void Main(string[] args) {
        MatrixReader mr = new MatrixReader("adj1.txt");
    }
}