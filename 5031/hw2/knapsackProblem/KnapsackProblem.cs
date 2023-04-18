using System;

namespace knapsack;


class Knapsack {
    public int maxWeight;
    public int weight = 0;
    public int value = 0;
    public List<Item> items = new List<Item>();
    const string linePattern = "|{0,20}|{1,20}|{2,20}|";

    public Knapsack(int maxW)
    {
        maxWeight = maxW;
    }
    public void addItem(Item item) {
        items.Add(item);
        weight += item.weight;
        weight += item.value;
    }

    public string getKnapsackItemList() {
        string s = "{";
        foreach(Item i in items) {
            s += i.id;
        }
        s += "}";
        return s;
    }

    public string toString() {
        if(weight<=maxWeight) {
            return String.Format(linePattern, string.Format("{0}", getKnapsackItemList()), weight, value);
        }
        return String.Format(linePattern, string.Format("{0}", getKnapsackItemList()), weight, "not feasible");
    }

    public void empty() {
        weight = 0;
        value = 0;
        items.Clear();
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

    static Knapsack solveKnapsackBruteForce(Knapsack knapsack, List<Item> items) {
        if(items.Count > 0) {
            // don't include
            solveKnapsackBruteForce(knapsack, items.GetRange(1, items.Count-1));

            // include
            knapsack.addItem(items[0]);
            Console.WriteLine(knapsack.toString());
            solveKnapsackBruteForce(knapsack, items.GetRange(1, items.Count-1));

            if (items.Count == 1) {
                knapsack.empty();
            }
        }
        
        return knapsack;
    }

    static void Main(string[] args)
    {
        List<Item> items = new List<Item>();
        items.Add(new Item(1, 7, 42));
        items.Add(new Item(2, 3, 12));
        items.Add(new Item(3, 4, 40));
        items.Add(new Item(4, 5, 25));

        Console.WriteLine("Number of addition operations in… ");
        Console.WriteLine(String.Format(linePattern, "Subset", "Total Weight", "Total Value"));
        solveKnapsackBruteForce(new Knapsack(10), items);
    }
}