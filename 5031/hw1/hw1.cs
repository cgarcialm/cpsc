namespace hw1 
{
    /// <summary>
    /// Fibonacci class implements different techniques to calculate the n-th Fibonacci number of the sequence.
    /// </summary>
    public static class Fibonacci
    {
        /// <summary>
        /// Calculates the n-th Fibonacci number recursively using the classic approach.
        /// </summary>
        /// <param name="n">The index of the Fibonacci number to calculate.</param>
        /// <param name="steps">A reference to a counter of the number of addition operations.</param>
        /// <returns>The n-th Fibonacci number.</returns>
        public static int FibRecursive(int n, ref int steps)
        {
            steps++;
            switch (n)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return FibRecursive(n - 1, ref steps) + FibRecursive(n - 2, ref steps);
            }
        }

        /// <summary>
        /// Calculates the n-th Fibonacci number iteratively using a for loop.
        /// </summary>
        /// <param name="n">The index of the Fibonacci number to calculate.</param>
        /// <param name="steps">A reference to a counter of the number of addition operations.</param>
        /// <returns>The n-th Fibonacci number.</returns>
        public static int FibIterative(int n, ref int steps)
        {
            if (n == 0 || n == 1)
            {
                steps++;
                return n;
            }
            int a = 0, b = 1, res = 1;
            for (int i = 2; i <= n; i++)
            {
                steps++;
                res = a + b;
                a = b;
                b = res;
            }

            return res;
        }

        /// <summary>
        /// Calculates the n-th Fibonacci number recursively using an accumulation approach.
        /// </summary>
        /// <param name="n">The index of the Fibonacci number to calculate.</param>
        /// <param name="steps">A reference to a counter of the number of addition operations.</param>
        /// <param name="a">The accumulator for the previous Fibonacci number.</param>
        /// <param name="b">The accumulator for the current Fibonacci number.</param>
        /// <returns>The n-th Fibonacci number.</returns>
        public static int FibRecursiveAccum(int n, ref int steps, int a = 0, int b = 1)
        {
            steps++;
            switch (n)
            {
                case 0:
                    return a;
                default:
                    return FibRecursiveAccum(n - 1, ref steps, b, a + b);
            }
        }
    }
    /// <summary>
    /// Program provides an interface to executre Fibonacci techniques for values that the user inputs using the console.
    /// </summary>
    class Program
    {
        static List<int> PromptUserInput() {
            List<int> numbers = new List<int>();
            int userInput;
            do
            {
                Console.Write("Enter a positive integer or -1 to run: ");
                while (!Int32.TryParse(Console.ReadLine(), out userInput) || userInput < -1 || userInput == 0) // TODO: Add Int32 validation
                {
                    Console.Write("Invalid input. Please enter a positive integer or -1: ");
                }

                if (userInput > 0)
                {
                    numbers.Add(userInput);
                }
            } while (userInput != -1);

            return numbers;
        }

        static void PrintFibResults(List<int> numbers) {
            Console.WriteLine("Number of addition operations in… ");
            var linePattern = "|{0,10}|{1,10}|{2,20}|{3,20}|{4,20}|";
            Console.WriteLine(String.Format(linePattern, "n", "fib(n)", "Classic Recursive", "Iterative", "Recursive w/ Accum"));
            foreach (int num in numbers)
            {
                int stepsR = 0, stepsI = 0, stepsRA = 0;
                int fibR = Fibonacci.FibRecursive(num, ref stepsR);
                int fibI = Fibonacci.FibIterative(num, ref stepsI);
                int fibRA = Fibonacci.FibRecursiveAccum(num, ref stepsRA);

                if (fibR != fibI || fibR != fibRA)
                {
                    Console.WriteLine("There was an error in the calculation of Fibonacci for " + num + ".");
                    break;
                }
                Console.WriteLine(String.Format(linePattern, num, fibR, stepsR, stepsI, stepsRA));
            }
        }

        /// <summary>
        /// The main entry point of the program. Prompts the user for positive integers and calculates the corresponding
        /// Fibonacci numbers using different methods, displaying the number of addition operations performed for each method.
        /// </summary>
        /// <param name="args">The command line arguments (not used).</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Fibonacci Sequence Technique Comparator.");
            bool runFib = true;
            string? answer;
            do {
                List<int> numbers = PromptUserInput();
                PrintFibResults(numbers);
                
                Console.Write("\nDo you want to calculate and compare more Fibonaccis? y/n: ");
                answer = Console.ReadLine();
                while (answer != "y" && answer != "n") {
                    Console.Write("\nInvalid input. Please enter \"y\" or \"n\": ");
                }
                runFib = answer == "y";
            } while (runFib);
            Console.WriteLine("Goodbye!");
        }
    }
}