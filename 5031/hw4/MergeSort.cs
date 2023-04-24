using System;
using System.Linq;

class MergeSort {
    /// <summary>
    /// Sorts array A[0..n − 1] by recursive mergesort
    /// Input: An array A[0..n − 1] of orderable elements 
    /// Output: Array A[0..n − 1] sorted in nondecreasing order
    /// </summary>
    /// <returns></returns>
    public void sort(ref int[] A) {
        if(A.Length > 1) {
            int[] B = new int[A.Length%2 == 0 ? A.Length/2 : A.Length/2+1];
            Array.Copy(A, B, A.Length%2 == 0 ? A.Length/2 : A.Length/2+1);
            sort(ref B);
            int[] C = new int[A.Length/2];
            Array.Copy(A, A.Length%2 == 0 ? A.Length/2 : A.Length/2+1, C, 0, A.Length/2);
            sort(ref C);
            Merge(B, C, ref A);
        }
    }
    /// <summary>
    /// //Merges two sorted arrays into one sorted array
    /// Input: Arrays B[0..p − 1] and C[0..q − 1] both sorted 
    /// Output: Sorted array A[0..p + q − 1] of the elements of B and C
    /// </summary>
    /// <param name="B">Sorted int array</param>
    /// <param name="C">Sorted int array</param>
    /// <param name="A">Unsorted int array that will store the output</param>
    /// <returns></returns>
    private void Merge(int[] B, int[] C, ref int[] A)
    {
        int i = 0, j = 0, k = 0;
        while(i < B.Length && j < C.Length) {
            if(B[i] <= C[j]) {
                A[k] = B[i]; 
                i++;
            } else {
                A[k] = C[j]; 
                j++;
            }
            k++;
        }
        if(i == B.Length) {
            Array.Copy(C, j, A, k, A.Length - k);
        } else {
            Array.Copy(B, i, A, k, A.Length - k);
        }
    }

    /// <summary>
    /// Prints an array into the console
    /// </summary>
    /// <param name="anArray">Int array</param>
    static string arrayToString(int[] anArray) {
        return "{" + String.Join(",", anArray) + "}";
    }

    /// <summary>
    /// The main entry point of the program.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args) {
        MergeSort sorter = new MergeSort();
        List<int[]> arraysToSort = new List<int[]>();
        arraysToSort.Add(new int[] {});
        arraysToSort.Add(new int[] {0, 1, 2, 3});
        arraysToSort.Add(new int[] {0, 1, 2, 3, 4});
        arraysToSort.Add(new int[] {3, 1, 4, 1, 5, 9, 2, 6, 5});
        arraysToSort.Add(new int[] {9, 8, 7, 6, 5, 4, 3, 2, 1});

        Console.WriteLine("Welcome to the MergeSort.\n");
        const string linePattern = "|{0,20}|{1,20}|{2,20}|";
        Console.WriteLine(String.Format(linePattern, "Test Case #", "A", "Sorted A"));
        Console.WriteLine("+--------------------+--------------------+--------------------+");
        for(int i = 0; i < arraysToSort.Count; i++) {
            int[] sortedArray = new int[arraysToSort[i].Length];
            arraysToSort[i].CopyTo(sortedArray, 0);
            sorter.sort(ref sortedArray);
            Console.WriteLine(String.Format(linePattern, i, arrayToString(arraysToSort[i]), arrayToString(sortedArray)));
        }
        
    }
}