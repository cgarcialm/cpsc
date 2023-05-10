using System;

class HeapSort{

    int[] array;

    HeapSort(int[] anArray) {
        array = new int[anArray.Length];
        for(int i = 0; i < anArray.Length; i++) {
            array[i] = anArray[i];
        }
    }

    /// <summary>
    /// Checks if length of array is 0
    /// </summary>
    /// <returns>Boolean indicating if array is empty</returns>
    public bool empty() {
        return array.Length == 0;
    }
}