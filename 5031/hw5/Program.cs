using System;

class HeapSort{

    int[] array;

    HeapSort(int[] anArray) {
        array = new int[anArray.Length];
        for(int i = 0; i < anArray.Length; i++) {
            array[i] = anArray[i];
        }
    }
}