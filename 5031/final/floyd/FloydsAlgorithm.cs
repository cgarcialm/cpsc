using System;

class FloydsAlgorithm {
    const int INF = int.MaxValue;
    int [,] solMatrix;
    int nNodes, i, j, k;

    public FloydsAlgorithm(int[, ] graph) {
        // TODO: Check if square
        nNodes = graph.GetLength(0);
        solMatrix = new int[nNodes, nNodes];

        // Initialize the solution matrix
        // same as input graph matrix
        for (i = 0; i < nNodes; i++) {
            for (j = 0; j < nNodes; j++) {
                solMatrix[i, j] = graph[i, j];
            }
        }
    }

    void printMatrix(int matrixIndex)
    {
        Console.WriteLine(String.Format("Matrix {0}:", matrixIndex));
        for (int i = 0; i < nNodes; ++i) {
            for (int j = 0; j < nNodes; ++j) {
                if (solMatrix[i, j] == INF) {
                    Console.Write("INF\t");
                }
                else {
                    Console.Write(solMatrix[i, j] + "\t");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void calcShortestPaths() {
        for (k = 0; k < nNodes; k++) {
            // Print current matrix
            printMatrix(k);

            // Pick all vertices as source
            // one by one
            for (i = 0; i < nNodes; i++) {
                // Pick all vertices as destination
                // for the above picked source
                for (j = 0; j < nNodes; j++) {
                    // If vertex k is on the shortest
                    // path from i to j, then update
                    // the value of dist[i][j]
                    if (solMatrix[i,k] != INF && solMatrix[k,j] != INF && solMatrix[i,k] + solMatrix[k,j] < solMatrix[i,j]) {
                        solMatrix[i,j] = solMatrix[i,k] + solMatrix[k,j];
                    }
                }
            }
        }
        printMatrix(k);
    }

    // Driver's Code
    public static void Main(string[] args)
    {
        /* Let us create the following
           weighted graph
              10
        (0)------->(3)
        |         /|\
        5 |         |
        |         | 1
        \|/         |
        (1)------->(2)
             3             */
        // int[, ] graph = { { 0, 5, INF, 10 },
        //                   { INF, 0, 3, INF },
        //                   { INF, INF, 0, 1 },
        //                   { INF, INF, INF, 0 } };

        int[, ] graph = { { 0, INF, 3, INF },
                          { 2, 0, INF, INF },
                          { INF, 7, 0, 1 },
                          { 6, INF, INF, 0 } };
 
        FloydsAlgorithm f = new FloydsAlgorithm(graph);
 
        // Function call
        f.calcShortestPaths();
    }
}