using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleStatickList<T>
{
    private readonly T[] items;

    public int Count => items.Length; 

    public T this[int index]
    {
        get
        {
            ValidateIndex(index);
            return items[index];
        }
    }

    public SimpleStatickList(T[] sourceItems)
    {
        if sourceItems == null)
        {
            items == new T[0];
            return;
        }

        items = new T[sourceItems.Length];
        for (int i = 0; i < sourceItems.Length; i++)
        items[i] = sourceItems[i];
        

    public bool Contains(T item)
    {
       
    }

    public T[] ToArray()
    {
        return items;
    }
    public void ValidateIndex(int index)
    {

        if(index < 0 || index >= Count)
        {
            throw new System.IndexOutOfRangeException($"Index {index} is out of range. Valid range is 0 to {Count - 1}.");
        }

    }




}
