using System;

class adjLists {
    int nNodes;
    LinkedList<int>[] adj;

    public adjLists(int nNodes) {
        adj = new LinkedList<int>[nNodes];
        for (int i = 0; i < adj.Length; i++) {
            adj[i] = new LinkedList<int>();
        }
        this.nNodes = nNodes;
    }

    public void addEdge(int x, int y) {
        adj[x].AddLast(y);
    }

    private void _dfs(int start, bool[] visited) {
        // Print the current node
        Console.Write(start + " ");

        // Set current node as visited
        visited[start] = true;

        LinkedList<int> vList = adj[start];
        foreach(int v in vList) {
            _dfs(v, visited);
        }
    }

    public void dfs() {
        bool[] visited = new bool[nNodes];

        _dfs(0, visited);
    }

    public void bfs(int start) {
        bool[] visited = new bool[nNodes];
        List<int> q = new List<int>();

        q.Add(start);
        visited[start] = true;

        while(q.Count > 0) {
            for(int i = 0; i < nNodes; i++) {
                Console.Write(q[0] + " ");
                LinkedList<int> vList = adj[i];
                foreach(int v in vList) {
                    if(!visited[v]) {
                        q.Add(v);
                        visited[v] = true;
                    }
                }
                q.Remove(q[0]);
            }
        }
    }

    public static void Main(string[] args) {
        int[, ] graph = { { 0, 1, 1, 0 },
                          { 1, 0, 0, 1 },
                          { 1, 0, 0, 0 },
                          { 0, 1, 0, 0 } };

        adjLists aL = new adjLists(4);
        aL.addEdge(0, 1);
        aL.addEdge(0, 2);
        aL.addEdge(1, 3);

        Console.Write("Depth First Search: ");
        aL.dfs();
        Console.WriteLine();
        Console.Write("Breadth First Search: ");
        aL.bfs(0);
    }
}