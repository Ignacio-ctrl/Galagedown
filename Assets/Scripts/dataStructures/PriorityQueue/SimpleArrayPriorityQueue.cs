using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class SimpleArrayPriorityQueue<T> : IPriorityQueue<T>
{
    T[] internalArray;
    int[] priorities;

    int defaultCapacity = 4;

    public int count { get; private set; }

    public SimpleArrayPriorityQueue()
    {
        internalArray = new T[defaultCapacity];
        priorities = new int[defaultCapacity];
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public void Enqueue(T item, int priority)
    {
        // Si no hay espacio, agrandamos los arrays
        if (count >= internalArray.Length)
            Resize();

        internalArray[count] = item;
        priorities[count] = priority;

        count++;
    }

    public T Dequeue()
    {
        if (IsEmpty())
            throw new System.Exception("Queue is empty");

        // Buscamos el elemento con menor prioridad numerica
        int index = GetMinIndex();

        T result = internalArray[index];

        // Corremos los elementos para llenar el espacio
        for (int i = index; i < count - 1; i++)
        {
            internalArray[i] = internalArray[i + 1];
            priorities[i] = priorities[i + 1];
        }

        count--;

        internalArray[count] = default(T);
        priorities[count] = 0;

        return result;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new System.Exception("Queue is empty");

        int index = GetMinIndex();

        return internalArray[index];
    }

    public void Clear()
    {
        internalArray = new T[defaultCapacity];
        priorities = new int[defaultCapacity];
        count = 0;
    }

    int GetMinIndex()
    {
        int minIndex = 0;

        // Buscamos la prioridad mas baja
        for (int i = 1; i < count; i++)
        {
            if (priorities[i] < priorities[minIndex])
            {
                minIndex = i;
            }
        }

        return minIndex;
    }

    void Resize()
    {
        // Duplicamos la capacidad
        int newSize = internalArray.Length * 2;

        T[] newArray = new T[newSize];
        int[] newPriorities = new int[newSize];

        for (int i = 0; i < count; i++)
        {
            newArray[i] = internalArray[i];
            newPriorities[i] = priorities[i];
        }

        internalArray = newArray;
        priorities = newPriorities;
    }
}

