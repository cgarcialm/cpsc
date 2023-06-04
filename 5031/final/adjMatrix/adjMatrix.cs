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

    public void addEdge(int x, int y) {
        adj[x, y] = 1;
        if(undirected) {
            adj[y, x] = 1;
        }
    }

    private void _dfs(int start, bool[] visited) {
        // Print the current node
        Console.Write(start + " ");

        // Set current node as visited
        visited[start] = true;

        for(int i = 0; i < nNodes; i++) {
            if(adj[start, i] == 1 && !visited[i]) {
                _dfs(i, visited);
            }
        }
    }

    public void dfs() {
        bool[] visited = new bool[nNodes];

        _dfs(0, visited);
    }

    private void bfs(int start) {
        bool[] visited = new bool[nNodes];
        List<int> q = new List<int>();

        q.Add(start);
        visited[start] = true;

        while(q.Count > 0) {
            Console.Write(q[0] + " ");
            for(int i = 0; i < nNodes; i++) {
                if(adj[q[0], i] == 1 && !visited[i]) {
                    q.Add(i);
                    visited[i] = true;
                }
            }
            q.Remove(q[0]);
        }
    }

    public static void Main(string[] args) {
        int[, ] graph = { { 0, 1, 1, 0 },
                          { 1, 0, 0, 1 },
                          { 1, 0, 0, 0 },
                          { 0, 1, 0, 0 } };

        adjMatrix aM = new adjMatrix(graph);
        Console.Write("Depth First Search: ");
        aM.dfs();
        Console.WriteLine();
        Console.Write("Breadth First Search: ");
        aM.bfs(0);
    }
}