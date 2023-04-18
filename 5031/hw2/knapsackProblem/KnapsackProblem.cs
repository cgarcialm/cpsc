using System;

namespace knapsack;


class Knapsack {
    public int maxWeight;
    public int weight = 0;
    public int value = 0;
    public List<Item> items = new List<Item>();
    public int nItems = 0;

    public Knapsack(int maxW)
    {
        maxWeight = maxW;
    }
    
    public void addItem(Item item) { // TODO: Check if id is already there throw exception
        items.Add(item);
        weight += item.weight;
        value += item.value;
        nItems++;
    }

    public string getKnapsackItemList() {
        string s = "{";
        foreach(Item i in items) {
            s += i.id + ",";
        }
        s += "}";
        return s;
    }

    public string toString() {
        const string linePattern = "|{0,20}|{1,20}|{2,20}|";
        if(weight <= maxWeight) {
            return String.Format(linePattern, string.Format("{0}", getKnapsackItemList()), weight, value);
        }
        return String.Format(linePattern, string.Format("{0}", getKnapsackItemList()), weight, "not feasible");
    }

    public void dropItem(int id) {
        for(int i = 0; i < items.Count; i++) {
            if(items[i].id == id) {
                Item itemToRemove = items[i];
                weight -= itemToRemove.weight;
                value -= itemToRemove.value;
                items.RemoveAt(i);
                nItems--;
            }
        }
    }
}

class Item {
    public int id;
    public int weight;
    public int value;

    public Item(int id, int w, int v)
    {
        this.id = id;
        this.weight = w;
        this.value = v;
    }
}

class KnapsackProblem
{
    Knapsack knapsack;
    List<Item> items;
    const string linePattern = "|{0,20}|{1,20}|{2,20}|";

    static int solveKnapsackBruteForce(Knapsack knapsack, List<Item> items) {
        int maxValueInclude = 0, maxValueNotInclude = 0;

        if(items.Count > 0) {
            // include
            knapsack.addItem(items[0]);
            Console.WriteLine(knapsack.toString());
            maxValueInclude = solveKnapsackBruteForce(knapsack, items.GetRange(1, items.Count-1));
            
            // don't include
            knapsack.dropItem(items[0].id);
            maxValueNotInclude = solveKnapsackBruteForce(knapsack, items.GetRange(1, items.Count-1));
        }
        
        return Math.Max(maxValueInclude, maxValueNotInclude);
    }

    static void Main(string[] args)
    {
        List<Item> items = new List<Item>();
        items.Add(new Item(1, 7, 42));
        items.Add(new Item(2, 3, 12));
        items.Add(new Item(3, 4, 40));
        items.Add(new Item(4, 5, 25));

        Console.WriteLine(String.Format(linePattern, "Subset", "Total Weight", "Total Value"));
        solveKnapsackBruteForce(new Knapsack(10), items);
    }
}