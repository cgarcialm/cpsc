using System;

class InvalidPointsOrDimensionsException : Exception {
    public InvalidPointsOrDimensionsException()
    {
    }
    public InvalidPointsOrDimensionsException(string message)
        : base(message)
    {
    }
}

class EuclideanDistance {

    private string printPoint(int[] p)
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


    private void checkPointDims(int[] p1, int[] p2, int numDimensions) {
        if(p1.Length != numDimensions) {
            throw new InvalidPointsOrDimensionsException(String.Format("Invalid dimensions ({0}) for point: {1}.", numDimensions, printPoint(p1)));
        }
        if(p2.Length != numDimensions) {
            throw new InvalidPointsOrDimensionsException(String.Format("Invalid dimensions ({0}) for point: {1}.", numDimensions, printPoint(p2)));
        }
    }

    public double calculateIteratively(int[] p1, int[] p2, int numDimensions) {
        checkPointDims(p1, p2, numDimensions);
        double distance = 0;
        for(int dim = 0; dim < numDimensions; dim++) {
            distance += Math.Pow(p1[dim] - p2[dim], 2);
        }
        distance = Math.Sqrt(distance);

        return distance;
    }

    private double _calculateRecursively(int[] p1, int[] p2, int numDimensions) {
        if(numDimensions == 0) {
            return Math.Pow(p1[0] - p2[0], 2);
        }
        return Math.Pow(p1[numDimensions-1] - p2[numDimensions-1], 2) + _calculateRecursively(p1, p2, numDimensions-1);
    }

    public double calculateRecursively(int[] p1, int[] p2, int numDimensions) {
        return Math.Sqrt(_calculateRecursively(p1, p2, numDimensions-1));
    }

    static void Main(string[] args)
    {
        int[] p1 = {1, 1, 1, 1};
        int[] p2 = {0, 0, 0};
        int numDimensions = 4;

        EuclideanDistance euDist = new EuclideanDistance();
        Console.WriteLine("Iteratively calculated distance: {0:0.00}", euDist.calculateIteratively(p1, p2, numDimensions));
        Console.WriteLine("Recursively calculated distance: {0:0.00}", euDist.calculateRecursively(p1, p2, numDimensions));
    }
}

