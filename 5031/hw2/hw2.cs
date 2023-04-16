using System;

namespace hw2;
class Program
{
    public static int BruteForceStringMatch(string totalString, string lookUpString) {
        for (int i = 0; i < totalString.Length - lookUpString.Length; i++) {
            int j = 0;
            while(j<lookUpString.Length && lookUpString[j] == totalString[j]) {
                j++;
            }
            if (j == totalString.Length) {
                return i;
            }
        }
        return -1;
    }

    static void Main(string[] args)
    {
        string totalString = "hola";
        string lookUpString = "la";

        Console.WriteLine(BruteForceStringMatch(totalString, lookUpString));
    }
}