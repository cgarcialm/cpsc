// C++ program to demonstrate
// multithreading using three
// different callables.
#include <iostream>
#include <thread>
#include <sstream>
using namespace std;

int* inputArray;
int* sortedArray;

string printArray(int* array, int start, int end) {
    stringstream ss;
    for(int i = start; i < end; i++) {
        ss << array[i] << " ";
    }
    return ss.str();
}

struct NumbersToSort {
    int* array;
    int startIndex;
    int endIndex;
};

// Function to calculate the minimum
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

struct NumbersToMerge {
    int length;
    int* array;
};

void mergeSortedArrays(NumbersToMerge numbers) {
    sortedArray = new int[numbers.length]; // TODO: Account for odd numbers
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

// Driver code
int main(int argc, char* argv[])
{
    // Check if enough arguments are provided
    if (argc < 2) {
        cout << "Please provide a list of numbers.\n";
        return 0;
    }
    int length = argc - 1;
    inputArray = new int[length]; // TODO: Account for odd numbers

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

    thread mergeThread(mergeSortedArrays, NumbersToMerge{length, inputArray});
    mergeThread.join();
    cout << "Merged Array: " << printArray(sortedArray, 0, length) << endl;

    delete[] inputArray;
    delete[] sortedArray;
    return 0;
}
