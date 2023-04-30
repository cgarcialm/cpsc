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

        return Math.Round(Math.Sqrt(distance),3);
    }

    private double _calculateRecursively(int[] p1, int[] p2, int numDimensions) {
        if(numDimensions == 1) {
            return Math.Pow(p1[0] - p2[0], 2);
        }
        return Math.Pow(p1[numDimensions-1] - p2[numDimensions-1], 2) + _calculateRecursively(p1, p2, numDimensions-1);
    }

    public double calculateRecursively(int[] p1, int[] p2, int numDimensions) {
        return Math.Round(Math.Sqrt(_calculateRecursively(p1, p2, numDimensions)), 3);
    }

    static void Main(string[] args)
    {
        const int MINDIMENSIONS = 1;
        const int MAXDIMENSIONS = 5;

        const int MINVALUE = -10;
        const int MAXVALUE = 10;
        
        Console.WriteLine("Welcome to the EuclideanDistance calculator.\n");
        const string linePattern = "|{0,20}|{1,20}|{2,20}|{3,20}|";
        Console.WriteLine(String.Format(linePattern, "P1", "P2", "Iterative Dist.", "Recursive Dist."));
        Console.WriteLine("+--------------------+--------------------+--------------------+--------------------+");

        Random randNum = new Random();
        for (int test = 1; test <=10; test++) {
            int numDimensions = randNum.Next(MINDIMENSIONS, MAXDIMENSIONS);;
            int[] p1 = new int[numDimensions];
            int[] p2 = new int[numDimensions];
            for (int dim = 0; dim < numDimensions; dim++) {
                p1[dim] = randNum.Next(MINVALUE, MAXVALUE);
                p2[dim] = randNum.Next(MINVALUE, MAXVALUE);
            }
            // int numDimensions = 3;
            // int[] p1 = new int[] {-3, 2, -4};
            // int[] p2 = new int[] {-7, 2, 2};

            EuclideanDistance euDist = new EuclideanDistance();
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

