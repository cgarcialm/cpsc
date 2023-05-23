/*
 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: May 22, 2023
 * Platform: macOS Monterrey
 * Version: 1.0
 *
 * The program takes a list of numbers as command-line arguments and performs sorting and merging operations using
 * multithreading in C++. It divides the input array into two halves and uses separate threads to sort each half
 * independently. After sorting, it merges the two sorted halves into a single sorted array using another thread.
 * The program then outputs the sorted and merged array.
 */
#include <iostream>
#include <thread>
#include <sstream>
using namespace std;

int* inputArray; // Global variable to store the input values in an array
int* sortedArray; // Global variable to store the sorted array

/// Function to print an array as a string
/// \param array Array to print
/// \param start Index from where to start printing
/// \param end Index to where to stop printing
/// \return String representation of the array
string printArray(int* array, int start, int end) {
    stringstream ss;
    for(int i = start; i < end; i++) {
        ss << array[i] << " ";
    }
    return ss.str();
}

/// Struct to hold input array and range for sorting
struct NumbersToSort {
    int* array;
    int startIndex;
    int endIndex;
};

/// Performs insertion sort on a portion of the array
/// \param numbers NumbersToSort object to be sorted
void insertionSort(NumbersToSort numbers)
{
    int v, j;
    for (int i = numbers.startIndex + 1; i < numbers.endIndex; i++)
    {
        v = numbers.array[i];
        j = i - 1;
        while(j >= numbers.startIndex && numbers.array[j] > v) {
            numbers.array[j + 1] = numbers.array[j];
            j--;
        }
        numbers.array[j + 1] = v;
    }
    cout << "Sorted Array: " << printArray(inputArray, numbers.startIndex, numbers.endIndex) << endl;
}

/// Struct to hold an array and its length for merging
struct NumbersToMerge {
    int length;
    int* array;
};

/// Takes an array that has each half already sorted and merges the them into global array sortedArray in a sorted way.
/// \param numbers
void mergeSortedArrays(NumbersToMerge numbers) {
    sortedArray = new int[numbers.length];
    int i = 0, j = numbers.length/2, k = 0;
    while(i < numbers.length/2 && j < numbers.length) {
        if(numbers.array[i] <= numbers.array[j]) {
            sortedArray[k] = numbers.array[i];
            i++;
        } else {
            sortedArray[k] = numbers.array[j];
            j++;
        }
        k++;
    }
    if(i == numbers.length/2) {
        while(j < numbers.length) {
            sortedArray[k] = numbers.array[j];
            j++; k++;
        }
    } else {
        while(i < numbers.length/2) {
            sortedArray[k] = numbers.array[i];
            i++; k++;
        }
    }
}

/// Driver program. Takes values from command line input and stores them in global variable inputArray.
/// Creates two threads to sort each half of the array and waits for them. Creates one last thread to merge
/// them in a sorted way.
/// \param argc
/// \param argv
/// \return
int main(int argc, char* argv[])
{
    // Check if enough arguments are provided
    if (argc < 2) {
        cout << "Please provide a list of numbers.\n";
        return 0;
    }
    int length = argc - 1;
    inputArray = new int[length];

    // Parse the command-line arguments as numbers
    for (int i = 1; i < argc; ++i) {
        inputArray[i - 1] = std::stoi(argv[i]);
    }

    cout << "Input Array: " << printArray(inputArray, 0, length) << endl;

    // Create a threads and pass the arguments
    thread sortingThread1(insertionSort, NumbersToSort{inputArray, 0, length/2});
    thread sortingThread2(insertionSort, NumbersToSort{inputArray, length/2, length});

    // Wait for the threads to finish
    sortingThread1.join();
    sortingThread2.join();

    cout << "Input Array: " << printArray(inputArray, 0, length) << endl;

    // Create a thread to merge the sorted arrays
    thread mergeThread(mergeSortedArrays, NumbersToMerge{length, inputArray});
    mergeThread.join();

    cout << "Merged Array: " << printArray(sortedArray, 0, length) << endl;

    // Clean up memory
    delete[] inputArray;
    delete[] sortedArray;

    return 0;
}
