using System;
using System.Collections.Generic;
using Galagedon.DataStructures.CoreStructures;

namespace Galagedon.Estructuras
{
    public class SimpleArrayList<T> : ISimpleList<T>
    {
        private const int DefaultCapacity = 4;

        private T[] internalArray;
        private int count;

        public int Count => count;

        public SimpleArrayList()
        {
            internalArray = new T[DefaultCapacity];
        }

        public T this[int index]
        {
            get
            {
                ValidateIndex(index);
                return internalArray[index];
            }
            set
            {
                ValidateIndex(index);
                internalArray[index] = value;
            }
        }

        public void Add(T item)
        {
            EnsureCapacity(count + 1);
            internalArray[count] = item;
            count++;
        }

        public void Insert(int index, T item)
        {
            // Insertar al final también es válido.
            if (index < 0 || index > count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            EnsureCapacity(count + 1);

            for (int i = count; i > index; i--)
            {
                internalArray[i] = internalArray[i - 1];
            }

            internalArray[index] = item;
            count++;
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(
                    internalArray[i], item))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            ValidateIndex(index);

            for (int i = index; i < count - 1; i++)
            {
                internalArray[i] = internalArray[i + 1];
            }

            count--;
            internalArray[count] = default(T);
        }

        public void AddRange(T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            EnsureCapacity(count + items.Length);
            Array.Copy(items, 0, internalArray, count, items.Length);
            count += items.Length;
        }

        public void RemoveRange(int index, int countToRemove)
        {
            if (index < 0 || index > count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (countToRemove < 0 || countToRemove > count - index)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(countToRemove));
            }

            if (countToRemove == 0)
            {
                return;
            }

            int elementsToMove = count - index - countToRemove;

            Array.Copy(
                internalArray,
                index + countToRemove,
                internalArray,
                index,
                elementsToMove
            );

            int newCount = count - countToRemove;
            Array.Clear(internalArray, newCount, countToRemove);
            count = newCount;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(
                    internalArray[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            Array.Clear(internalArray, 0, count);
            count = 0;
        }

        public T[] ToArray()
        {
            T[] result = new T[count];
            Array.Copy(internalArray, result, count);
            return result;
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private void EnsureCapacity(int requiredCapacity)
        {
            if (requiredCapacity <= internalArray.Length)
            {
                return;
            }

            int newCapacity = Math.Max(
                internalArray.Length * 2,
                requiredCapacity
            );

            T[] newArray = new T[newCapacity];
            Array.Copy(internalArray, newArray, count);
            internalArray = newArray;
        }
    }
}