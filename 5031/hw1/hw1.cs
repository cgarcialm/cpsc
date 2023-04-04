public static class Fibonacci
{
    /// <summary>
    /// Calculates the n-th Fibonacci number recursively using the classic approach.
    /// </summary>
    /// <param name="n">The index of the Fibonacci number to calculate.</param>
    /// <param name="steps">A reference to an integer that will store the number of operations performed.</param>
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
    /// <param name="steps">A reference to an integer that will store the number of operations performed.</param>
    /// <returns>The n-th Fibonacci number.</returns>
    public static int FibIterative(int n, ref int steps)
    {
        steps++;
        if (n == 0 || n == 1)
        {
            return n;
        }
        int a = 0, b = 1, res = 1;
        for (int i = 2; i <= n; i++)
        {
            res = a + b;
            a = b;
            b = res;
            steps++;
        }

        return res;
    }

    /// <summary>
    /// Calculates the n-th Fibonacci number recursively using an accumulation approach.
    /// </summary>
    /// <param name="n">The index of the Fibonacci number to calculate.</param>
    /// <param name="steps">A reference to an integer that will store the number of operations performed.</param>
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

class Program
{
    /// <summary>
    /// The main entry point of the program. Prompts the user for positive integers and calculates the corresponding
    /// Fibonacci numbers using different methods, displaying the number of operations performed for each method.
    /// </summary>
    /// <param name="args">The command line arguments (not used).</param>
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
        int userInput;
        do
        {
            Console.Write("Enter a positive integer or -1 to run: ");
            while (!Int32.TryParse(Console.ReadLine(), out userInput) || userInput < -1 || userInput == 0)
            {
                Console.Write("Invalid input. Please enter a positive number or -1: ");
            }

            if (userInput > 0)
            {
                numbers.Add(userInput);
            }
        } while (userInput != -1);

        Console.WriteLine("Number of addition operations in… ");
        var linePattern = "|{0,10}|{1,10}|{2,20}|{3,20}|{4,20}|";
        Console.WriteLine(String.Format(linePattern, "n", "fib(n)", "Classic Recursive", "Iterative",
            "Recursive w/ Accum"));
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
}
