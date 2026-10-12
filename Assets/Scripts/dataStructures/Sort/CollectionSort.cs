using UnityEngine;
using System;

using Galagedon.DataStructures.CoreStructures;
using UnityEngine.Rendering;
public static class CollectionSort 
{
    public static void SortList<T>(ISimpleList<T> collection,Comparison<T> comparison, bool ascending = true)
    {
        if (collection == null || comparison == null)
        {
            throw new ArgumentNullException("Collection or comparison cannot be null.");
        }
        
        for(int i = 0; i < collection.Count - 1; i++)
        {
            int selectedIndex = i;
            for (int j = i + 1; j < collection.Count; j++)
            {
               int result = comparison(collection[j], collection[selectedIndex]);
                
               bool shouldSelect = ascending ? result < 0 : result > 0;

                if (shouldSelect)
                {
                    selectedIndex = j;
                }

            }
            if(selectedIndex != i)
                Swap(collection, i, selectedIndex);
        }
    }

    private static void Swap<T>(ISimpleList<T> collection, int A, int B)
    {
        T aux = collection[A];
        collection[A] = collection[B];
        collection[B] = aux;
    }



}
