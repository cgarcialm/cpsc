using System;

class Heap{

    private int[] H;

    public Heap(int[] anArray) {
        H = new int[anArray.Length];
        for(int i = 0; i < anArray.Length; i++) {
            H[i] = anArray[i];
        }
        heapify();
    }

    /// <summary>
    /// Checks if length of H is 0
    /// </summary>
    /// <returns>Boolean indicating if H is empty</returns>
    public bool empty() {
        return H.Length == 0;
    }

    public string toString()
    {
        string s = "{";
        for (int i = 0; i < H.Length; i++)
        {
            s += H[i] + ",";
        }
        s = s.Remove(s.Length - 1, 1);
        s += "}";
        return s;
    }

    private void percolateDown(int k) {
        int v = H[k];
        bool heap = false;
        while(!heap && 2 * k + 1 <= H.Length - 1) { // there are any children
            int j = 2 * k + 1;
            if(j < H.Length - 1) { // there are two children
                if(H[j] < H[j+1]) {
                    j++;
                }
            }
            if(v >= H[j]) { // already heap
                heap = true;
            } else {
                H[k] = H[j];
                k = j;
            }
        }
        H[k] = v;
    }

    private void heapify() {
        int i = (H.Length - 1)/2;
        while(i >= 0) {
            percolateDown(i);
            i--;
        }
    }
}

class Homework5 {
    static void Main(string[] args) {
        Heap h = new Heap(new int[] {2, 9, 7, 6, 5, 8});
        Console.WriteLine("Heap: " + h.toString());
    }
}