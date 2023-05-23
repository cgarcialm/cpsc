/*
 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: May 22, 2023
 * Platform: macOS Monterrey
 * Version: 1.0
 *
 * The code takes a list of numbers as command-line arguments and calculates the average, minimum, and maximum values
 * using separate threads in C++. The numbers are stored in a struct, and each calculation is performed by a dedicated
 * function. The results are then displayed on the console.
 */
#include <iostream>
#include <thread>
using namespace std;

int average; // Global variable to store the average
int minimum; // Global variable to store the minimum
int maximum; // Global variable to store the maximum

/// Struct to hold an array and its length for merging
struct Numbers {
    int length;
    int* array;
};

/// Calculates the average of the values stored in the array of a Numbers object.
/// \param numbers
void calculateAverage(Numbers numbers)
{
    int total = 0;
    for (int i = 0; i < numbers.length; i++)
    {
        total += numbers.array[i];
    }
    average = total / numbers.length;
}

/// Calculates the minimum of the values stored in the array of a Numbers object.
/// \param numbers
void calculateMin(Numbers numbers)
{
    minimum = numbers.array[0];
    for (int i = 1; i < numbers.length; i++)
    {
        if(numbers.array[i] < minimum) {
            minimum = numbers.array[i];
        }
    }
}

/// Calculates the maximum of the values stored in the array of a Numbers object.
/// \param numbers
void calculateMax(Numbers numbers)
{
    maximum = numbers.array[0];
    for (int i = 1; i < numbers.length; i++)
    {
        if(numbers.array[i] > maximum) {
            maximum = numbers.array[i];
        }
    }
}

/// Driver program. Takes values from command line input and stores them in a Numbers object.
/// In three different threads calculates the average, the minimum and the maximum.
/// Waits for every thread to finish and prints the results.
/// \param argc
/// \param argv
/// \return
int main(int argc, char* argv[])
{
    // Check if enough arguments are provided
    if (argc < 2) {
        std::cout << "Please provide a list of numbers.\n";
        return 0;
    }

    int length = argc - 1;
    int* array = new int[length];

    // Parse the command-line arguments as numbers
    for (int i = 1; i < argc; ++i) {
        array[i - 1] = std::stoi(argv[i]);
    }

    Numbers numbers = Numbers{ length, array };

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

    /// Clean up memory
    delete[] array;

    return 0;
}
