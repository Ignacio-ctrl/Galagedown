using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLinkedStack<T> : ISimpleStack<T>
{
    private sealed class Node
    {
        public T Value;
        public Node Next;
        public Node(T value, Node next)
        {
            Value = value;
            Next = next;
        }
    }

    private Node head;
    public int Count { get; private set; }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public void Push(T item)
    {
        head = new Node(item, head);
        Count++;
    }

    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty.");
        T value = head.Value;
        head = head.Next;
        Count--;
        return value;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty.");
        return head.Value;
    }

    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (Node cur = head; cur != null; cur = cur.Next)
        {
            if (comparer.Equals(cur.Value, item))
                return true;
        }
        return false;
    }

    public void Clear()
    {
        head = null;
        Count = 0;
    }

    public T[] ToArray()
    {
        T[] array = new T[Count];
        int i = 0;
        for (Node cur = head; cur != null; cur = cur.Next)
        {
            array[i++] = cur.Value;
        }
        return array;
    }
}
