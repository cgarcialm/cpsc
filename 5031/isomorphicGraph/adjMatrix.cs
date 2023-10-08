using System;
using System.Collections.Generic;

class adjMatrix {
    int nNodes;
    int[, ] adj;
    bool undirected;

    public adjMatrix(int[, ] graph, bool undirected = true) {
        nNodes = graph.GetLength(0);
        adj = new int[nNodes, nNodes];
        this.undirected = undirected;

        for (int i = 0; i < nNodes; i++) {
            for (int j = 0; j < nNodes; j++) {
                adj[i, j] = graph[i, j];
            }
        }
    }

    private int nodesDegree(int i) {
        int degree = 0;
        for(int col = 0; col<nNodes; col++) {
            if(adj[i, col] == 1) {
                degree++;
            }
        }
        return degree;
    }

    private int countEdges() {
        int edges = 0;
        for(int i = 0; i < nNodes; i++) {
            for(int j = i+1; j < nNodes; j++) {
                if(adj[i, j] == 1) {
                    edges++;
                }
            }
        }
        return edges;
    }

    public bool isNotIsomorphic(adjMatrix otherMatrix) {
        if(nNodes != otherMatrix.nNodes) {
            return true;
        }
        if(countEdges() != otherMatrix.countEdges()) {
            return true;
        }

        List<int> thisNodesDegrees = new List<int>();
        List<int> thatNodesDegrees = new List<int>();
        for(int i = 0; i < nNodes; i++) {
            thisNodesDegrees.Add(nodesDegree(i));
            thatNodesDegrees.Add(otherMatrix.nodesDegree(i));
        }
        if(!thisNodesDegrees.SequenceEqual(thatNodesDegrees)) {
            return true;
        }

        return false;
    }

    public static void Main(string[] args) {
        int[, ] graph1 = { { 0, 1, 0, 0, 1 },
                           { 1, 0, 1, 0, 0 },
                           { 0, 1, 0, 1, 0 },
                           { 0, 0, 1, 0, 1 },
                           { 1, 0, 0, 1, 0 } };
        adjMatrix aM1 = new adjMatrix(graph1);

        int[, ] graph2 = { { 0, 0, 1, 1, 0 },
                           { 0, 0, 0, 1, 1 },
                           { 1, 0, 0, 0, 1 },
                           { 1, 1, 0, 0, 0 },
                           { 0, 1, 1, 0, 0 } };
        adjMatrix aM2 = new adjMatrix(graph2);

        Console.WriteLine("Graph1 is not isomorphic to graph2: " + (aM1.isNotIsomorphic(aM2) ? "true" : "false"));

        int[, ] graph3 = { { 0, 0, 1, 1, 0 },
                           { 0, 0, 0, 1, 1 },
                           { 1, 0, 0, 0, 0 },
                           { 1, 1, 0, 0, 0 },
                           { 0, 1, 0, 0, 0 } };
        adjMatrix aM3 = new adjMatrix(graph3);

        Console.WriteLine("Graph1 is not isomorphic to graph3: " + (aM1.isNotIsomorphic(aM3) ? "true" : "false"));

        int[, ] graph4 = { { 0, 0, 1, 1, 0 },
                           { 0, 0, 0, 1, 1 },
                           { 1, 0, 0, 0, 0 },
                           { 1, 1, 0, 0, 1 },
                           { 0, 1, 0, 1, 0 } };
        adjMatrix aM4 = new adjMatrix(graph4);

        Console.WriteLine("Graph1 is not isomorphic to graph3: " + (aM1.isNotIsomorphic(aM4) ? "true" : "false"));
    }
}