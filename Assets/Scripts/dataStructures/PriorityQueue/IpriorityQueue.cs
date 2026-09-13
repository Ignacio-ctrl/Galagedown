using System.Xml.Serialization;
using UnityEngine;

public interface IPriorityQueue<T> 
{
  int count { get; }

    bool IsEmpty();

    void Enqueue(T item, int priority);

    T Dequeue();
    T Peek();
    void Clear();

}
