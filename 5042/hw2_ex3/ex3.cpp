/*
 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: May 22, 2023
 * Platform: macOS Monterrey
 * Version: 1.0
 *
 * The program implements multithreading in C++ to enable three threads to take turns accessing a shared variable.
 * Each thread checks if it is its turn based on the value of the variable and prints a corresponding message.
 * After each thread completes two turns, the program terminates.
 */

#include <iostream>
#include <thread>
#include <mutex>
#include <chrono>
using namespace std;

mutex mtx;
int allowedID = 1;

/// Reads allowedID and check whether it is equal to the passed ID
/// \param ID ID of the thread
void runner(int ID)
{
    // Strings for output messages
    string waitString = "Not thread " + to_string(ID) + "'s turn.\n";
    string turnString = "Thread " + to_string(ID) + "'s turn!\n";
    string completedString = "Thread " + to_string(ID) + " completed.\n";;
    int count = 0;
    while(count < 3) {
        // Check if it's the thread's turn
        if (allowedID != ID) {
            cout << waitString;
        } else {
            cout << turnString;
            mtx.lock(); // Acquire the lock to update the shared variable
            count++;
            allowedID < 3 ? allowedID++ : allowedID = 1; // Update the shared variable, wrapping around from 3 to 1
            mtx.unlock(); // Release the lock
            if(count == 3) {
                cout << completedString;
            }
        }
        this_thread::sleep_for(chrono::milliseconds (2)); // Delay to allow other threads to progress
    }
}

/// Driver program. Creates three threads that run runner function and waits until they are completed.
/// \param argc
/// \param argv
/// \return
int main(int argc, char* argv[])
{
    // Create three threads and pass the ID as argument
    thread thread1(runner, 1);
    this_thread::sleep_for(chrono::milliseconds (1)); // Delay to allow previous thread to do something
    thread thread2(runner, 2);
    this_thread::sleep_for(chrono::milliseconds (1)); // Delay to allow previous thread to do something
    thread thread3(runner, 3);

    // Wait for the threads to finish
    thread1.join();
    thread2.join();
    thread3.join();

    return 0;
}
