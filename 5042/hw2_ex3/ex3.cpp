// C++ program to demonstrate
// multithreading using three
// different callables.
#include <iostream>
#include <thread>
#include <chrono>
using namespace std;

int allowedID = 1;

// Function to calculate the average
void checkID(int ID)
{
    string waitString = "Not thread " + to_string(ID) + "'s turn.\n";
    string turnString = "Thread " + to_string(ID) + "'s turn!\n";
    string completedString = "Thread " + to_string(ID) + " completed.\n";;
    int count = 0;
    while(count < 2) {
        if (allowedID != ID) {
            cout << waitString;
            this_thread::sleep_for(chrono::milliseconds (100));
        } else {
            cout << turnString;
            count++;
            allowedID < 3 ? allowedID++ : allowedID = 1;
        }
    }
    cout << completedString;
}

// Driver code
int main(int argc, char* argv[])
{
    // Create a threads and pass the arguments
    thread thread1(checkID, 1);
    thread thread2(checkID, 2);
    thread thread3(checkID, 3);

    // Wait for the threads to finish
    thread1.join();
    thread2.join();
    thread3.join();

    return 0;
}
