/******************************************************************************

 * Author: Cecilia Garcia Lopez de Munain <cgarcialopezdemunain@seattleu.edu>
 * Date: Apr 30 2023
 * Platform: MacOS Monterrey. Version 12.0.1.
 * Version: 1.0
 * Purpose: Implementation of DistanceCalculator program.

Welcome to DistanceCalculator. DistanceCalculator is a program that calculates
the geometric euclidean distance between two points using the Pythagorean Theorem:
    distance = √((xn-yn)^2+(x(n-1)-y(n-1))^2+...+(x0-y0)^2)
    n: number of dimensions of points X and Y

*******************************************************************************/

using System;

/// <summary>
/// InvalidPointsOrDimensionsException implements and Exception that inherits 
/// from Exception and prints the error of the input points and dimension.
/// </summary>
class InvalidDimensionsException : Exception
{
    public InvalidDimensionsException()
    {
    }
    public InvalidDimensionsException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Point represents a location in space in Euclidean geometry.
/// </summary>
class Point
{
    private int[] array;

    public Point(int[] anArray)
    {
        array = anArray;
    }

    /// <summary>
    /// Creates a string representation of the point
    /// </summary>
    /// <returns>String like {Xn, Xn-1, ..., X0}</returns>
    public string toString()
    {
        string s = "{";
        for (int dim = 0; dim < array.Length; dim++)
        {
            s += dim + ",";
        }
        s = s.Remove(s.Length - 1, 1);
        s += "}";
        return s;
    }

    /// <summary>
    /// Checks that a given point has the same number of dimensions than this point
    /// </summary>
    /// <param name="p2">Point p2</param>
    private void checkSameDims(Point p2)
    {
        if (array.Length != p2.array.Length)
        {
            throw new InvalidDimensionsException(
                String.Format("Points must have same dimensions: {0} p1 and {1} p2",
                toString(),
                p2.toString()
                ));
        }
    }

    /// <summary>
    /// Calculates the distance between this point an another given one using 
    /// iteration. Validates dimensions of point p2 first.
    /// </summary>
    /// <param name="p2">Point p2</param>
    /// <returns>distance</returns>
    public double calculateDistIter(Point p2)
    {
        checkSameDims(p2);
        double distance = 0;
        for (int dim = 0; dim < array.Length; dim++)
        {
            distance += Math.Pow(array[dim] - p2.array[dim], 2);
        }

        return Math.Sqrt(distance);
    }

    /// <summary>
    /// Helper function to calculate distance^2 between this point an 
    /// another given one using recursion.
    /// </summary>
    /// <param name="p2">Point p2</param>
    /// <returns>distance^2</returns>
    private double _calculateDistRec(Point p2, int dimension)
    {
        if (dimension == 1)
        {
            return Math.Pow(array[0] - p2.array[0], 2);
        }
        return Math.Pow(array[dimension - 1] - p2.array[dimension - 1], 2)
            + _calculateDistRec(p2, dimension - 1);
    }

    /// <summary>
    /// Calculates distance with point p2 using recursion.
    /// Validates dimensions of point p2 first.
    /// </summary>
    /// <param name="p2">Point p2</param>
    /// <returns></returns>
    public double calculateDistRec(Point p2)
    {
        checkSameDims(p2);
        return Math.Sqrt(_calculateDistRec(p2, array.Length));
    }
}

/// <summary>
/// DistanceCalculator implements a program to compute distances between points
/// with different techniques and test its results by comparing the outputs in 
/// console.
/// </summary>
class DistanceCalculator
{

    /// <summary>
    /// The main entry point of the program. Runs 10 tests.
    /// 1. In the first test creates two points of randomly generated number of
    ///     dimensions with the same randomly generated values 
    ///     In the other tests creates two points of randomly generated number 
    ///     of dimensions with randomly generated values
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
        const string LINEPATTERN = "|{0,20}|{1,20}|{2,20}|{3,20}|";

        Console.WriteLine("Welcome to the Distance Calculator.\n");
        Console.WriteLine(String.Format(LINEPATTERN, "P1", "P2", "Iterative Dist.", "Recursive Dist."));
        Console.WriteLine("+--------------------+--------------------+--------------------+--------------------+");

        Random randNum = new Random();
        for (int test = 1; test <= NTESTS; test++)
        {
            int numDimensions = randNum.Next(MINDIMENSIONS, MAXDIMENSIONS); ;
            int[] array1 = new int[numDimensions];
            int[] array2 = new int[numDimensions];

            for (int dim = 0; dim < numDimensions; dim++)
            {
                if (test == 1)
                {
                    int val = randNum.Next(MINVALUE, MAXVALUE);
                    array1[dim] = val;
                    array2[dim] = val;
                }
                else
                {
                    array1[dim] = randNum.Next(MINVALUE, MAXVALUE);
                    array2[dim] = randNum.Next(MINVALUE, MAXVALUE);
                }
            }
            Point p1 = new Point(array1);
            Point p2 = new Point(array2);

            Console.WriteLine(String.Format(
                LINEPATTERN,
                p1.toString(),
                p2.toString(),
                Math.Round(p1.calculateDistIter(p2), 3),
                Math.Round(p1.calculateDistRec(p2), 3)
                ));
        }
        Console.WriteLine("\nGoodbye!");
    }
}