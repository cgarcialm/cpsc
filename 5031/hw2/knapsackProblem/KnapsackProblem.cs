/******************************************************************************

Welcome to KnapsackProblem.
KnapsackProblem is a program that solves a knapsack problem with Brute Force and Exhaustive Search.

*******************************************************************************/

using System;
using System.Collections.Generic;

/// <summary>
/// Knapsack class implements an object that can hold Items which weight is the weight of all its Items and the value is the value of all its Items.
/// It has a limit for the total weight that is given by its maximum weight.
/// </summary>
class Knapsack
{
    public int maxWeight;
    private int weight = 0;
    private int value = 0;
    private List<Item> items = new List<Item>();
    private int nItems = 0;

    /// <summary>
    /// Constructor of Knapsack.
    /// </summary>
    /// <param name="maxW">The maximum weight it can hold</param>
    public Knapsack(int maxW)
    {
        maxWeight = maxW;
    }

    /// <summary>
    /// Getter of the weight of the knapsack. If weight is greater than the limit, returns -1.
    /// </summary>
    /// <returns></returns>
    public int getWeight()
    {
        if (weight <= maxWeight)
        {
            return weight;
        }
        return -1;
    }

    /// <summary>
    /// Getter of the value of the knapsack. If weight is greater than the limit, returns -1.
    /// </summary>
    /// <returns></returns>
    public int getValue()
    {
        if (weight <= maxWeight)
        {
            return value;
        }
        return -1;
    }

    /// <summary>
    /// Adds an Item to the knapsack.
    /// </summary>
    /// <param name="item">The Item to add</param>
    public void addItem(Item item)
    {

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == item.id)
            {
                throw new Exception(String.Format("There is already an Item with id {0}", item.id));
            }
        }

        items.Add(item);
        weight += item.weight;
        value += item.value;
        nItems++;
    }

    /// <summary>
    /// Creates a string that represents the Items that are in the knapsack.
    /// </summary>
    /// <returns></returns>
    public string getKnapsackItemList()
    {
        string s = "{";
        foreach (Item i in items)
        {
            s += i.id + ",";
        }
        s += "}";
        return s;
    }

    /// <summary>
    /// Creates a string that represents the knapsack: list of items, weight and value.
    /// </summary>
    /// <returns></returns>
    public string toString()
    {
        const string linePattern = "|{0,20}|{1,20}|{2,20}|";
        if (weight <= maxWeight)
        {
            return String.Format(linePattern, string.Format("{0}", getKnapsackItemList()), weight, value);
        }
        return String.Format(linePattern, string.Format("{0}", getKnapsackItemList()), weight, "not feasible");
    }

    /// <summary>
    /// Drops an item with the given id from the knapsack.
    /// </summary>
    /// <param name="id">Id of the item to drop</param>
    /// <returns></returns>
    public Knapsack dropItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
            {
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

/// <summary>
/// Item class implements an object that has an id, a weight and a value.
/// </summary>
class Item
{
    public int id;
    public int weight;
    public int value;

    /// <summary>
    /// Constructor of Item.
    /// </summary>
    /// <param name="id">A number to identify the Item</param>
    /// <param name="w">The weight of the Item</param>
    /// <param name="v">The value of the Item</param>
    public Item(int id, int w, int v)
    {
        this.id = id;
        this.weight = w;
        this.value = v;
    }
}

/// <summary>
/// KnapsackProblem class implements a program that solves the knapsack problem for a knapsack of max weight 10 and the following items:
///     - Item 1: weight 7 and value 42.
///     - Item 2: weight 3 and value 12.
///     - Item 3: weight 4 and value 40.
///     - Item 4: weight 5 and value 25.
/// </summary>
class KnapsackProblem
{
    /// <summary>
    /// Solves, using brute force, the knapsack problem recursively considering the option to add or not the next item in the list to the given knapsack. 
    /// </summary>
    /// <param name="knapsack">The knapsack in a given state</param>
    /// <param name="items">A list of items to potentially add to the knapsack</param>
    /// <param name="maxValue">The current maximum value for the given state of the knapsack</param>
    /// <returns></returns>
    static int solveKnapsackBruteForceRecursive(Knapsack knapsack, List<Item> items, int maxValue)
    {
        int newMaxValue = 0;

        if (items.Count > 0)
        {
            // include
            knapsack.addItem(items[0]);
            Console.WriteLine(knapsack.toString());
            int maxValueInclude = Math.Max(knapsack.getValue(), solveKnapsackBruteForceRecursive(knapsack, items.GetRange(1, items.Count - 1), maxValue));

            // don't include
            int maxValueNotInclude = solveKnapsackBruteForceRecursive(knapsack.dropItem(items[0].id), items.GetRange(1, items.Count - 1), maxValue);

            // update new max value
            newMaxValue = Math.Max(maxValueInclude, maxValueNotInclude);
        }

        return Math.Max(maxValue, newMaxValue);
    }

    /// <summary>
    /// A helper method for solveKnapsackBruteForceRecursive to print the empty knapsack.
    /// </summary>
    /// <param name="knapsack">An empty knapsack</param>
    /// <param name="items">A list of items to potentially add to the knapsack</param>
    /// <returns></returns>
    static int solveKnapsackBruteForce(Knapsack knapsack, List<Item> items)
    {
        Console.WriteLine(knapsack.toString());
        int maxValue = solveKnapsackBruteForceRecursive(knapsack, items, 0);

        return maxValue;
    }

    /// <summary>
    /// The main entry point of the program. Creates a list of Items and an empty knapsack and solves the knapsack problem with Brute Force and Exhaustive Search.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        List<Item> items = new List<Item>();
        items.Add(new Item(1, 7, 42));
        items.Add(new Item(2, 3, 12));
        items.Add(new Item(3, 4, 40));
        items.Add(new Item(4, 5, 25));

        Console.WriteLine("Welcome to the KnapsackProblem using Brute Force and Exhaustive Search.\n");
        const string linePattern = "|{0,20}|{1,20}|{2,20}|";
        Console.WriteLine(String.Format(linePattern, "Subset", "Total Weight", "Total Value"));
        Console.WriteLine("+--------------------+--------------------+--------------------+");
        int maxValue = solveKnapsackBruteForce(new Knapsack(10), items);
        Console.WriteLine(String.Format("\nThe maximum value you can put in the knapsack is {0}.", maxValue));
        Console.WriteLine("Goodbye!");
    }
}