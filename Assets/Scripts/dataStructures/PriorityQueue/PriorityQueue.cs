using UnityEngine;

public class PriorityQueue<T> : IPriorityQueue<T> 
{

    private class Node
    {
        public T Item;
        public int Priority;
        public Node Next;

        public Node(T item, int priority)
        {
            Item = item;
            Priority = priority;
            Next = null;
        }
    }

    private Node first;
    public int count { get; private set; }

    public void Clear()
    {
        first = null;
        count = 0;
    }

    public T Dequeue()
    {
        if (IsEmpty())
        {
            throw new System.InvalidOperationException("Queue is empty.");
        }
        T item = first.Item;
        first = first.Next;
        count--;
        return item;
    }

    public void Enqueue(T item, int priority)
    {
        Node newNode = new Node(item, priority);
        if(first == null || priority > first.Priority)
        {
            newNode.Next = first;
            first = newNode;
        }
        else
        {
            Node current = first;
            while(current.Next != null && current.Next.Priority >= priority)
            {
                current = current.Next;
            }
            newNode.Next = current.Next;
            current.Next = newNode;
        }
        count++;
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public T Peek()
    {
        if (IsEmpty())
        {
            throw new System.InvalidOperationException("Queue is empty.");
        }
        return first.Item;
    }


}
