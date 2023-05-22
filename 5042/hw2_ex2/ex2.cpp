// C++ program to demonstrate
// multithreading using three
// different callables.
#include <iostream>
#include <thread>
#include <sstream>
using namespace std;

int* inputArray;
int* sortedArray;

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

string printArray(int* array, int size) {
    stringstream ss;
    for(int i = 0; i < size; i++) {
        ss << array[i] << " ";
    }
    return ss.str();
}

// Driver code
int main(int argc, char* argv[])
{
    // Check if enough arguments are provided
    if (argc < 2) {
        std::cout << "Please provide a list of numbers.\n";
        return 0;
    }

    int length = argc - 1;
    inputArray = new int[length]; // TODO: Account for odd numbers

    // Parse the command-line arguments as numbers
    for (int i = 1; i < argc; ++i) {
        inputArray[i - 1] = std::stoi(argv[i]);
    }

    cout << "Input Array: " << printArray(inputArray, length) << endl;
    // Create a threads and pass the arguments
    thread sortingThread1(insertionSort, NumbersToSort{inputArray, 0, length/2});
    thread sortingThread2(insertionSort, NumbersToSort{inputArray, length/2, length});

    // Wait for the threads to finish
    sortingThread1.join();
    sortingThread2.join();
    cout << "Input Array: " << printArray(inputArray, length) << endl;

    thread mergeThread(mergeSortedArrays, NumbersToMerge{length, inputArray});
    mergeThread.join();
    cout << "Sorted Array: " << printArray(sortedArray, length) << endl;

    printArray(sortedArray, length);

    delete[] inputArray;
    delete[] sortedArray;
    return 0;
}
