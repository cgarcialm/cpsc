using System;

namespace knapsack;


class Knapsack {
    public int maxWeight;
    private int weight = 0;
    private int value = 0;
    private List<Item> items = new List<Item>();
    private int nItems = 0;

    public Knapsack(int maxW)
    {
        maxWeight = maxW;
    }
     public int getWeight() {
        if(weight <= maxWeight) {
            return weight;
        }
        return -1;
    }

    public int getValue() {
        if(weight <= maxWeight) {
            return value;
        }
        return -1;
    }
    
    public void addItem(Item item) {
    
        for(int i = 0; i < items.Count; i++) {
            if(items[i].id == item.id) {
                throw new Exception(String.Format("There is already an Item with id {0}", item.id));
            }
        }

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

    public Knapsack dropItem(int id) {
        for(int i = 0; i < items.Count; i++) {
            if(items[i].id == id) {
                Item itemToRemove = items[i];
                weight -= itemToRemove.weight;
                value -= itemToRemove.value;
                items.RemoveAt(i);
                nItems--;
            }
        }
        return this;
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
    const string linePattern = "|{0,20}|{1,20}|{2,20}|";

    static int solveKnapsackBruteForceRecursive(Knapsack knapsack, List<Item> items, int maxValue) {
        int newMaxValue = 0;

        if(items.Count > 0) {
            // include
            knapsack.addItem(items[0]);
            Console.WriteLine(knapsack.toString());
            int maxValueInclude = Math.Max(knapsack.getValue(), solveKnapsackBruteForceRecursive(knapsack, items.GetRange(1, items.Count-1), maxValue));
            
            // don't include
            int maxValueNotInclude = solveKnapsackBruteForceRecursive(knapsack.dropItem(items[0].id), items.GetRange(1, items.Count-1), maxValue);

            // update new max value
            newMaxValue = Math.Max(maxValueInclude, maxValueNotInclude);
        }
        
        return Math.Max(maxValue, newMaxValue);
    }

    static int solveKnapsackBruteForce(Knapsack knapsack, List<Item> items) {
        Console.WriteLine(knapsack.toString());
        int maxValue = solveKnapsackBruteForceRecursive(knapsack, items, 0);

        return maxValue;
    }


    static void Main(string[] args)
    {
        List<Item> items = new List<Item>();
        items.Add(new Item(1, 7, 42));
        items.Add(new Item(2, 3, 12));
        items.Add(new Item(3, 4, 40));
        items.Add(new Item(4, 5, 25));

        Console.WriteLine(String.Format(linePattern, "Subset", "Total Weight", "Total Value"));
        int maxValue = solveKnapsackBruteForce(new Knapsack(10), items);
        Console.WriteLine(String.Format("\nThe maximum value you can put in the knapsack is {0}", maxValue));
    }
}