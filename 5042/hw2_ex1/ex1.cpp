// C++ program to demonstrate
// multithreading using three
// different callables.
#include <iostream>
#include <thread>
using namespace std;

int average;
int minimum;
int maximum;

struct Numbers {
    int count;
    int* array;
};

// Function to calculate the average
void calculateAverage(Numbers numbers)
{
    int total = 0;
    for (int i = 0; i < numbers.count; i++)
    {
        total += numbers.array[i];
    }
    average = total / numbers.count;
}

// Function to calculate the minimum
void calculateMin(Numbers numbers)
{
    minimum = numbers.array[0];
    for (int i = 1; i < numbers.count; i++)
    {
        if(numbers.array[i] < minimum) {
            minimum = numbers.array[i];
        }
    }
}

// Function to calculate the minimum
void calculateMax(Numbers numbers)
{
    maximum = numbers.array[0];
    for (int i = 1; i < numbers.count; i++)
    {
        if(numbers.array[i] > maximum) {
            maximum = numbers.array[i];
        }
    }
}

// Driver code
int main(int argc, char* argv[])
{
    // Check if enough arguments are provided
    if (argc < 2) {
        std::cout << "Please provide a list of numbers.\n";
        return 0;
    }

    int count = argc - 1;
    int* array = new int[count];

    // Parse the command-line arguments as numbers
    for (int i = 1; i < argc; ++i) {
        array[i - 1] = std::stoi(argv[i]);
    }

    Numbers numbers = Numbers{ count, array };

    // Create a threads and pass the arguments
    thread threadAverage(calculateAverage, numbers);
    thread threadMin(calculateMin, numbers);
    thread threadMax(calculateMax, numbers);

    // Wait for the threads to finish
    threadAverage.join();
    threadMin.join();
    threadMax.join();

    std::cout << "The average value is " << average << std::endl;
    std::cout << "The minimum value is " << minimum << std::endl;
    std::cout << "The maximum value is " << maximum << std::endl;

    delete[] array;
    return 0;
}
