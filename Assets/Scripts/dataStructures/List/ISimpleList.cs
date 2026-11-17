using UnityEngine;

namespace Galagedon.DataStructures.CoreStructures
{
    // Interfaz que comparten ambas listas
    public interface ISimpleList<T>
    {
        int Count { get; }
        T this[int index] { get; set; }
        void Add(T item);
        bool Remove(T item);
        void RemoveAt(int index);
        void Clear();
        bool Contains(T item);
        void Insert(int index, T item);
        T[] ToArray();
    }

    // Nodo para la Lista Enlazada
    public class LinkedNode<T>
    {
        public T value;
        public LinkedNode<T> next;
        public LinkedNode<T> prev;

        public LinkedNode(T value)
        {
            this.value = value;
            this.next = null;
            this.prev = null;
        }
    }
}