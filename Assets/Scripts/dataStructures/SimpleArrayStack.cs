using System.Collections;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class SimpleArrayStack<T> : ISimpleStack<T>
{

    private T[] items;
    private int bottom;

    public int Count { get; private set; }

    public int Capacity
    {
        get { return items.Length; }

    }

    public SimpleArrayStack(int initialCapacity = 10)
    {
        if (initialCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be greater than zero.");
        items = new T[initialCapacity];
       
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public bool IsFull()
    {
        return Count == Capacity;
    }

    public void Push(T item)
    {


    if(IsFull())
        throw new InvalidOperationException("Stack is full.");

        items[(bottom + Count++)%Capacity] = item;

    }

    public void PushDiscardOldest(T item)
    {
        if(Capacity == 0) throw new InvalidOperationException("Stack capacity is zero.");

        if(!IsFull())
        {
            Push(item);
            return;
        }

        items[bottom] = item;
        bottom = (bottom + 1) % Capacity;

    }

    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty.");
        int index = (bottom + Count - 1) % Capacity;
        T item = items[index];
        items[index] = default(T); // Clear the reference
        
        return item;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty.");
        int index = (bottom + Count - 1) % Capacity;
        return items[index];
    }

    public bool Contains(T item)
    {
        for(int i = Count - 1; i >= 0; i--)
        {
            int index = (bottom + i) % Capacity;
            if (EqualityComparer<T>.Default.Equals(items[index], item))
                return true;
        }
        return false;
    }

    public void Clear()
    {
      while (!IsEmpty())
        
            Pop();

        bottom = 0;
    }

    public T[] ToArray()
    {
        T[] array = new T[Count];
        for (int i = 0; i < Count; i++)
        {
            int index = (bottom + i) % Capacity;
            array[i] = items[index];
        }
        return array;
    }

























}





