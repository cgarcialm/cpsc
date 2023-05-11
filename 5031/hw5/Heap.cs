/******************************************************************************

 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: May 10 2023
 * Platform: MacOS Monterrey. Version 12.0.1.
 * Version: 1.0
 * Purpose: Implementation of HeapSort.

Implementation of HeapSort per the algorithm described in the book "Introduction
to The Design and Analysis of Algorithms" by Anany Levitin, 3rd edition, to sort 
an array in ascending order.

*******************************************************************************/

using System;

/// <summary>
/// Class Heap implements a binary tree that always mantains the following 
/// properties:
///     1. It is complete
///     2. It satisfies heap-order property: Data in each node >= data in 
///     children
/// </summary>
class Heap{

    private int[] H;

    /// <summary>
    /// Constructs a heap from elements of a given array using bottom-up heap 
    /// construction
    /// </summary>
    /// <param name="anArray">Array of ints to create heap</param>
    public Heap(int[] anArray) {
        H = new int[anArray.Length];
        for(int i = 0; i < anArray.Length; i++) {
            H[i] = anArray[i];
        }
        heapify();
    }

    /// <summary>
    /// Checks if length of heap is 0
    /// </summary>
    /// <returns>Boolean indicating if heap is empty</returns>
    public bool empty() {
        return H.Length == 0;
    }

    /// <summary>
    /// Generates string represatation of heap
    /// </summary>
    /// <returns></returns>
    public string toString()
    {
        string s = "{";
        for (int i = 0; i < H.Length; i++)
        {
            s += H[i] + ",";
        }
        s = s.Remove(s.Length - 1, 1);
        s += "}";
        return s;
    }

    /// <summary>
    /// Retrieves the max item
    /// </summary>
    /// <returns>Max item</returns>
    public int getMax() {
        return H[0];
    }

    /// <summary>
    /// Deletes the max item
    /// </summary>
    /// <returns>Max item</returns>
    public int delete() {
        int max = H[0];
        H[0] = H[H.Length - 1];
        Array.Resize(ref H, H.Length - 1);
        heapify();
        return max;
    }

    /// <summary>
    /// Insert value to heap
    /// </summary>
    /// <param name="v">Value to insert</param>
    public void insert(int v) {
        Array.Resize(ref H, H.Length + 1);
        H[H.Length - 1] = v;
        percolateUp(H.Length - 1);
    }

    /// <summary>
    /// Generates sorted array from heap
    /// </summary>
    /// <returns>Sorted array of ints</returns>
    public int[] sort() {
        int initLength = H.Length;
        int[] sortedArray = new int[initLength];
        for(int i = 0; i < initLength; i++) {
            sortedArray[i] = delete();
        }
        return sortedArray;
    }

    /// <summary>
    /// Swaps node k with the biggest of its children until array is in heap 
    /// order
    /// </summary>
    /// <param name="k">Index of element to percolate</param>
    private void percolateDown(int k) {
        int v = H[k];
        bool heap = false;
        while(!heap && 2 * k + 1 <= H.Length - 1) { // there are any children
            int j = 2 * k + 1;
            if(j < H.Length - 1) { // there are two children
                if(H[j] < H[j+1]) {
                    j++;
                }
            }
            if(v >= H[j]) { // already heap
                heap = true;
            } else {
                H[k] = H[j];
                k = j;
            }
        }
        H[k] = v;
    }

    /// <summary>
    /// Swaps node k with its parent until array is in heap order
    /// </summary>
    /// <param name="k">Index of element to percolate</param>
    private void percolateUp(int k) {
        int v = H[k];
        bool heap = false;
        while(!heap || k != 0) { 
            int j = (k - 1) / 2;
            if(v <= H[j]) { // already heap
                heap = true;
            } else {
                H[k] = H[j];
                H[j] = v;
                k = j;
            }
        }
    }

    /// <summary>
    /// Rearranges array H to put it in heap order
    /// </summary>
    private void heapify() {
        if(H.Length > 1) {
            int i = (H.Length - 1)/2;
            while(i >= 0) {
                percolateDown(i);
                i--;
            }
        }
    }
}

/// <summary>
/// Class Homework 5 tests the implementation of Heap.
/// </summary>
class Homework5 {
    /// <summary>
    /// The main entry point of the program.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args) {
        Heap h = new Heap(new int[] {2, 9, 7, 6, 5, 8});
        Console.WriteLine("Heap: " + h.toString());
        Console.WriteLine("Deleted: " + h.delete());
        Console.WriteLine("Heap: " + h.toString());
        Console.WriteLine("Inserted: 10");
        h.insert(10);
        Console.WriteLine("Heap: " + h.toString());
        int[] sorted = h.sort();
    }
}