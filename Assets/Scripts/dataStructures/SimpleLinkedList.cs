using Galagedon.DataStructures.CoreStructures;
using System.Collections.Generic;
using System;

namespace Galagedon.Estructuras
{
    public class SimpleLinkedList<T> : ISimpleList<T>
    {
        LinkedNode<T> first;
        LinkedNode<T> last;
        int count;

        public T this[int index]
        {
            get => GetNodeByIndex(index).value;
            set => GetNodeByIndex(index).value = value;
        }

        public int Count => count;

        public void Add(T item)
        {
            LinkedNode<T> newNode = new LinkedNode<T>(item);

            if (count == 0) first = newNode;
            else
            {
                newNode.prev = last;
                last.next = newNode;
            }

            last = newNode;
            count++;
        }

        public void Clear()
        {
            first = null;
            last = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            return GetNodeByValue(item) != null;
        }

        public bool Remove(T item)
        {
            LinkedNode<T> toRemove = GetNodeByValue(item);
            if (toRemove == null) return false;

            if (count == 1)
            {
                Clear();
                return true;
            }

            if (toRemove == first)
            {
                first.next.prev = null;
                first = first.next;
            }
            else if (toRemove == last)
            {
                last.prev.next = null;
                last = last.prev;
            }
            else RemoveAndReconnect(toRemove);

            count--;
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is outside of List bounds");

            if (count == 1)
            {
                Clear();
                return;
            }

            if (index == 0)
            {
                first.next.prev = null;
                first = first.next;
            }
            else if (index == count - 1)
            {
                last.prev.next = null;
                last = last.prev;
            }
            else RemoveAndReconnect(GetNodeByIndex(index));

            count--;
        }

        LinkedNode<T> GetNodeByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is outside of List bounds");

            if (index <= count / 2)
            {
                LinkedNode<T> current = first;
                for (int i = 0; i < index; i++)
                    current = current.next;
                return current;
            }
            else
            {
                LinkedNode<T> current = last;
                for (int i = count - 1; i > index; i--)
                    current = current.prev;
                return current;
            }
        }

        LinkedNode<T> GetNodeByValue(T value)
        {
            LinkedNode<T> current = first;
            while (current != null)
            {
                if (current.value.Equals(value)) return current;
                current = current.next;
            }
            return null;
        }

        void RemoveAndReconnect(LinkedNode<T> toRemove)
        {
            toRemove.next.prev = toRemove.prev;
            toRemove.prev.next = toRemove.next;
            toRemove = null;
        }

        // Métodos de la interfaz no implementados en este fragmento
        public void Insert(int index, T item) { throw new NotImplementedException(); }
        public T[] ToArray() { throw new NotImplementedException(); }
    }
}