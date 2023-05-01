/******************************************************************************

 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: Apr 30 2023
 * Platform: MacOS Monterrey. Version 12.0.1.
 * Version: 1.0
 * Purpose: Implementation of EuclideanDistance calculator program.

Welcome to EuclideanDistance. EuclideanDistance is a program that calculates
the geometric euclidean distance between two points.

*******************************************************************************/

using System;

/// <summary>
/// InvalidPointsOrDimensionsException implements and Exception that inherits
/// from Exception and prints the error of the input points and dimension.
/// </summary>
class InvalidPointsOrDimensionsException : Exception {
    public InvalidPointsOrDimensionsException()
    {
    }
    public InvalidPointsOrDimensionsException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// EuclideanDistance implements a distance between to points that is calculated
/// using the Pythagorean Theorem:
///     distance = √((xn-yn)^2+(x(n-1)-y(n-1))^2+...+(x0-y0)^2)
///     n: number of dimensions of points X and Y
/// </summary>
class EuclideanDistance {

    /// <summary>
    /// Creates a string representation of point p
    /// </summary>
    /// <param name="p">Point p</param>
    /// <returns></returns>
    public string printPoint(int[] p)
    {
        string s = "{";
        foreach (int dim in p)
        {
            s += dim + ",";
        }
        s = s.Remove(s.Length - 1, 1); 
        s += "}";
        return s;
    }

    /// <summary>
    /// Checks that the set of points and the number of dimensions are valid 
    /// by checking that:
    ///     - p1 has length numDimensions
    ///     - p2 has length numDimensions
    /// Hence, p1 and p2 have the same length.
    /// </summary>
    /// <param name="p1">Point p1</param>
    /// <param name="p2">Point p2</param>
    /// <param name="numDimensions">Number of dimensions</param>
    private void checkPointDims(int[] p1, int[] p2, int numDimensions) {
        if(p1.Length != numDimensions) {
            throw new InvalidPointsOrDimensionsException(
                String.Format("Invalid dimensions ({0}) for point: {1}.", 
                numDimensions, 
                printPoint(p1))
                );
        }
        if(p2.Length != numDimensions) {
            throw new InvalidPointsOrDimensionsException(
                String.Format("Invalid dimensions ({0}) for point: {1}.", 
                numDimensions, 
                printPoint(p2))
                );
        }
    }

    /// <summary>
    /// Calculates the distance between two points using iteration
    /// </summary>
    /// <param name="p1">Point p1</param>
    /// <param name="p2">Point p2</param>
    /// <param name="numDimensions">Number of dimensions</param>
    /// <returns></returns>
    public double calculateIteratively(int[] p1, int[] p2, int numDimensions) {
        checkPointDims(p1, p2, numDimensions);
        double distance = 0;
        for(int dim = 0; dim < numDimensions; dim++) {
            distance += Math.Pow(p1[dim] - p2[dim], 2);
        }

        return Math.Round(Math.Sqrt(distance),3);
    }

    /// <summary>
    /// Calculates the distance^2 between two points using recursion
    /// </summary>
    /// <param name="p1">Point p1</param>
    /// <param name="p2">Point p2</param>
    /// <param name="numDimensions">Number of dimensions</param>
    /// <returns></returns>
    private double _calculateRecursively(int[] p1, int[] p2, int numDimensions) {
        if(numDimensions == 1) {
            return Math.Pow(p1[0] - p2[0], 2);
        }
        return Math.Pow(p1[numDimensions-1] - p2[numDimensions-1], 2) 
            + _calculateRecursively(p1, p2, numDimensions-1);
    }

    /// <summary>
    /// Helper function to compute the square root of _calculateRecursively output
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="numDimensions"></param>
    /// <returns></returns>
    public double calculateRecursively(int[] p1, int[] p2, int numDimensions) {
        return Math.Round(Math.Sqrt(_calculateRecursively(p1, p2, numDimensions)), 3);
    }

    /// <summary>
    /// The main entry point of the program. Runs 10 tests.
    /// 1. In the first test creates two points of randomly generated number of dimensions 
    ///     with the same randomly generated values 
    ///     In the other tests creates two points of randomly generated number of dimensions with
    ///     randomly generated values
    /// 2. Calculates the distance between the points iteratively and recursively
    /// 3. Prints results into console
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        const int NTESTS = 10;

        const int MINDIMENSIONS = 1;
        const int MAXDIMENSIONS = 6;

        const int MINVALUE = -10;
        const int MAXVALUE = 11;
        
        Console.WriteLine("Welcome to the EuclideanDistance calculator.\n");
        const string linePattern = "|{0,20}|{1,20}|{2,20}|{3,20}|";
        Console.WriteLine(String.Format(linePattern, "P1", "P2", "Iterative Dist.", "Recursive Dist."));
        Console.WriteLine("+--------------------+--------------------+--------------------+--------------------+");

        Random randNum = new Random();
        EuclideanDistance euDist = new EuclideanDistance();
        for (int test = 1; test <=NTESTS; test++) {
            int numDimensions = randNum.Next(MINDIMENSIONS, MAXDIMENSIONS);;
            int[] p1 = new int[numDimensions];
            int[] p2 = new int[numDimensions];

            for (int dim = 0; dim < numDimensions; dim++) {
                if(test==1) {
                    int val = randNum.Next(MINVALUE, MAXVALUE);
                    p1[dim] = val;
                    p2[dim] = val;
                } else {
                    p1[dim] = randNum.Next(MINVALUE, MAXVALUE);
                    p2[dim] = randNum.Next(MINVALUE, MAXVALUE);
                }
            }
            
            Console.WriteLine(String.Format(
                linePattern, 
                euDist.printPoint(p1), 
                euDist.printPoint(p2), 
                euDist.calculateIteratively(p1, p2, numDimensions), 
                euDist.calculateRecursively(p1, p2, numDimensions)
                ));
        }
        Console.WriteLine("\nGoodbye!");
    }
}

