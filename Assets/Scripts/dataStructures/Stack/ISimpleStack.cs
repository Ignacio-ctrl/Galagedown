using UnityEngine;
using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using UnityEngine.Rendering;
using System.Collections.Generic;


public interface ISimpleStack<T>
{

    int Count { get; }
    bool IsEmpty();
    void Push(T item);
    T Pop();
    T Peek();
    bool Contains(T item);
    void Clear();
    T[] ToArray();
}

    

