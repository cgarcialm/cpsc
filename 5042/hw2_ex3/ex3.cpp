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
    int count = 0;
    while(count < 2) {
        if (allowedID != ID) {
            cout << "Not thread " << ID << "'s turn" << endl;
            this_thread::sleep_for (chrono::seconds(1));
        } else {
            cout << "Thread " << ID << "'s turn!" << endl;
            count++;
            allowedID < 3 ? allowedID++ : allowedID = 1;
        }
    }
    cout << "Thread " << ID << " completed." << endl;
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
