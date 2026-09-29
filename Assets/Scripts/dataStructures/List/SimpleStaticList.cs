using System;
using System.Collections.Generic;

public class SimpleStatickList<T>
{
    private readonly T[] items;

    public int Count => items.Length;

    public SimpleStatickList(T[] sourceItems)
    {
        if (sourceItems == null)
        {
            items = new T[0];
            return;
        }

        items = new T[sourceItems.Length];
        Array.Copy(sourceItems, items, sourceItems.Length);
    }

    public T this[int index]
    {
        get
        {
            ValidateIndex(index);
            return items[index];
        }
    }

    public bool Contains(T item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(items[i], item))
            {
                return true;
            }
        }

        return false;
    }

    public T[] ToArray()
    {
        T[] copy = new T[items.Length];
        Array.Copy(items, copy, items.Length);
        return copy;
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(index),
                index,
                $"El índice debe estar entre 0 y {Count - 1}."
            );
        }
    }
}