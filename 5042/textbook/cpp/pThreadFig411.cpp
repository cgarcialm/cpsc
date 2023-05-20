#include <pthread.h>
#include <iostream>
#include <cstdlib>

int sum; // This data is shared by the thread(s)

void *runner(void *param); // Threads call this function

int main(int argc, char *argv[])
{
    if (argc < 2) {
        std::cerr << "Usage: " << argv[0] << " <upper_bound>" << std::endl;
        return 1;
    }

    pthread_t tid; // The thread identifier
    pthread_attr_t attr; // Set of thread attributes

    // Set the default attributes of the thread
    pthread_attr_init(&attr);
    // Create the thread
    pthread_create(&tid, &attr, runner, argv[1]);
    // Wait for the thread to exit
    pthread_join(tid, nullptr);

    std::cout << "sum = " << sum << std::endl;

    return 0;
}

// The thread will execute in this function
void *runner(void *param)
{
    int i, upper = std::atoi(static_cast<char *>(param));
    sum = 0;
    for (i = 1; i <= upper; i++)
        sum += i;
    pthread_exit(nullptr);
}
