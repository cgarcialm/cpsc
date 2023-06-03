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
#include <condition_variable>
using namespace std;

mutex mtx;
condition_variable cv;
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
        // Acquire a unique lock of mutex
        unique_lock<mutex> uniqueLock(mtx);

        // Check if it's the thread's turn
        while (allowedID != ID) {
            cout << waitString;
            cv.wait(uniqueLock, [&] { return allowedID == ID; }); // Wait on allowedID
        }

        // When it's the thread's turn execute
        cout << turnString;
        count++;
        allowedID < 3 ? allowedID++ : allowedID = 1; // Update the shared variable, wrapping around from 3 to 1
        uniqueLock.unlock(); // Release the lock
        cv.notify_all(); // Notify all threads

        // If already execute 3 times, finish
        if(count == 3) {
            cout << completedString;
        }

        // Delay next iteration to allow other threads make progress
        this_thread::sleep_for(chrono::nanoseconds(1));
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
    thread thread2(runner, 2);
    thread thread3(runner, 3);

    // Wait for the threads to finish
    thread1.join();
    thread2.join();
    thread3.join();

    return 0;
}
