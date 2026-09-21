using Galagedon.DataStructures.CoreStructures;
using System;
using UnityEngine;

public class ImpleLinkedQueue<T> : ISimpleQueue<T>
{
    int count = 0;
    LinkedNode<T> first = null;
    LinkedNode<T> last = null;
    public int Count => count;

    public bool IsEmpty => count == 0;

    public void Clear()
    {
        first = null;
        last = null;
        count = 0;
    }

    public T Dequeue()
    {
        if (IsEmpty) throw new System.Exception("Cannot Dequeue from this Queue");

        T result = first.value;
        first = first.next;
        if (first != null) first.prev = null;
        count--;
        return result;
    }

    public void Enqueue(T item)
    {
        LinkedNode<T> newNode = new LinkedNode<T>(item);

        if (count == 0) first = newNode;

        else
        {
            //conectamos al nuevo nodo con el que era el ultimo
            newNode.prev = last;
            last.next = newNode;
        }

        //el ultimo ahora es el nuevo
        last = newNode;
        count++;
    }

    public T Peek()
    {
        if (IsEmpty) throw new Exception("Cannot Peek from empty Stack");
        return first.value;
    }

    public T[] ToArray()
    {
        T[] result = new T[count];
        LinkedNode<T> current = first;

        for (int i = 0; i < count; i++)
        {
            //recorro el array y le asigno a la posicion actual el valur del nodo
            result[i] = current.value;
            //despues paso al siguiente nodo
            current = current.next;
        }
        return result;
    }
}