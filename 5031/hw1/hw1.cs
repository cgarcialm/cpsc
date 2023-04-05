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
        /// <param name="addOps">A reference to a counter of the number of addition operations.</param>
        /// <returns>The n-th Fibonacci number.</returns>
        public static int FibRecursive(int n, ref int addOps)
        {
            switch (n)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    addOps++;
                    return FibRecursive(n - 1, ref addOps) + FibRecursive(n - 2, ref addOps);
            }
        }

        /// <summary>
        /// Calculates the n-th Fibonacci number iteratively using a for loop.
        /// </summary>
        /// <param name="n">The index of the Fibonacci number to calculate.</param>
        /// <param name="addOps">A reference to a counter of the number of addition operations.</param>
        /// <returns>The n-th Fibonacci number.</returns>
        public static int FibIterative(int n, ref int addOps)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            int a = 0, b = 1, res = 1;
            for (int i = 2; i <= n; i++)
            {
                addOps++;
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
        /// <param name="addOps">A reference to a counter of the number of addition operations.</param>
        /// <param name="a">The accumulator for the previous Fibonacci number.</param>
        /// <param name="b">The accumulator for the current Fibonacci number.</param>
        /// <returns>The n-th Fibonacci number.</returns>
        public static int FibRecursiveAccum(int n, ref int addOps, int a = 0, int b = 1)
        {
            switch (n)
            {
                case 0:
                    return a;
                default:
                    addOps++;
                    return FibRecursiveAccum(n - 1, ref addOps, b, a + b);
            }
        }
    }
    /// <summary>
    /// Program provides an interface to execute and compare Fibonacci techniques for values that the user inputs using the console.
    /// </summary>
    class Program
    {
        static List<int> PromptUserInput() {
            List<int> numbers = new List<int>();
            int userInput;
            do
            {
                Console.Write("Enter a positive integer or -1 to run: ");
                while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < -1)
                {
                    Console.Write("Invalid input. Please enter a positive integer or -1: ");
                }

                if (userInput != -1)
                {
                    numbers.Add(userInput);
                }
            } while (userInput != -1);

            return numbers;
        }

        static void PrintFibResults(List<int> numbers) {
            Console.WriteLine("Number of addition operations in… ");
            var linePattern = "|{0,8}|{1,8}|{2,18}|{3,18}|{4,18}|";
            Console.WriteLine(String.Format(linePattern, "n", "fib(n)", "Classic Recursive", "Iterative", "Recursive w/ Accum"));
            foreach (int num in numbers)
            {
                int addOpsR = 0, addOpsI = 0, addOpsRA = 0;
                int fibR = Fibonacci.FibRecursive(num, ref addOpsR);
                int fibI = Fibonacci.FibIterative(num, ref addOpsI);
                int fibRA = Fibonacci.FibRecursiveAccum(num, ref addOpsRA);

                if (fibR != fibI || fibR != fibRA)
                {
                    Console.WriteLine("There was an error in the calculation of Fibonacci for " + num + ".");
                    break;
                }
                Console.WriteLine(String.Format(linePattern, num, fibR, addOpsR, addOpsI, addOpsRA));
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
            string? runFib = "y";
            do {
                List<int> numbers = PromptUserInput();
                PrintFibResults(numbers);
                
                Console.Write("\nDo you want to calculate and compare more Fibonaccis? y/n: ");
                runFib = Console.ReadLine();
                while (runFib != "y" && runFib != "n") {
                    Console.Write("\nInvalid input. Please enter \"y\" or \"n\": ");
                }
            } while (runFib == "y");
            Console.WriteLine("Goodbye!");
        }
    }
}