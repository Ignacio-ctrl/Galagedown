using Galagedon.DataStructures.CoreStructures;
using System;

namespace Galagedon.Estructuras
{
    public class SimpleArrayList<T> : ISimpleList<T>
    {
        T[] internalArray;
        int count = 0;
        int defaultCapacity = 4;

        public int Count { get => count; }

        public T this[int index]
        {
            get => internalArray[index];
            set => internalArray[index] = value;
        }

        public SimpleArrayList()
        {
            internalArray = new T[defaultCapacity];
        }

        public void Add(T item)
        {
            ValidateSize(count + 1);
            internalArray[count] = item;
            count++;
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (internalArray[i].Equals(item))
                {
                    ShiftLeft(i, 1);
                    count--;
                    return true;
                }
            }
            return false;
        }

        public void Insert(int index, T item)
        {
            ValidateSize(count + 1);
            ShiftRight(index);
            internalArray[index] = item;
            count++;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= internalArray.Length)
                throw new ArgumentOutOfRangeException("Index is outside of bounds");

            ShiftLeft(index, 1);
            count--;
        }

        public void AddRange(T[] items)
        {
            ValidateSize(count + items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                internalArray[count] = items[i];
                count++;
            }
        }

        public void RemoveRange(int index, int countToRemove)
        {
            if (index < 0 || index >= internalArray.Length)
                throw new ArgumentOutOfRangeException("Index is outside of bounds");

            ShiftLeft(index, countToRemove);
            this.count -= countToRemove;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (internalArray[i].Equals(item)) return true;
            }
            return false;
        }

        public void Clear()
        {
            count = 0;
            internalArray = new T[defaultCapacity];
        }

        void ValidateSize(int nextIndex)
        {
            if (nextIndex >= internalArray.Length) Resize(nextIndex);
        }

        void Resize(int targetAmount)
        {
            int currentLength = internalArray.Length;
            while (targetAmount > currentLength)
                currentLength *= 2;

            T[] nextArray = new T[currentLength];
            for (int i = 0; i < count; i++)
                nextArray[i] = internalArray[i];

            internalArray = nextArray;
        }

        void ShiftLeft(int index, int offset)
        {
            for (int i = index; i < count; i++)
                internalArray[i] = internalArray[i + offset];

            for (int i = count - offset; i < count; i++)
                internalArray[i] = default;
        }

        void ShiftRight(int index)
        {
            for (int i = count + 1; i > index; i--)
            {
                internalArray[i] = internalArray[i - 1];
            }
            internalArray[index] = default;
        }

        public T[] ToArray()
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = internalArray[i];

            return result;
        }
    }
}