using System.Threading;
using UnityEngine;

public class SimpleArrayQueue<T> : ISimpleQueue<T>
{
    int count = 0;
    int defaultCapacity = 4;
    T[] internalArray;

    public int Count => count;

    public bool IsEmpty => count == 0;

    public SimpleArrayQueue()
    {
        internalArray = new T[defaultCapacity];
    }

    public void Clear()
    {
        internalArray = new T[internalArray.Length];
        count = 0;
    }

    public T Dequeue()
    {
        if (IsEmpty) throw new System.Exception("Cannot Dequeue from this Queue");
        T result = internalArray[0];
        ShiftLeft(1); //aca falta un offset que seria un segundo parametro
        count--;
        return result;
    }

    public void Enqueue(T item)
    {
        ValidateSize(count + 1);
        internalArray[count] = item;
        count++;
    }

    public T Peek()
    {
        if (IsEmpty) throw new System.Exception("Cannot Peek from this Queue");
        return internalArray[0];
    }

    public T[] ToArray()
    {
        T[] result = new T[count]; //creo un array con la cantidad de espacion ocupados (count) en la lista

        for (int i = 0; i < count; i++)
        {
            result[i] = internalArray[i]; //copiamos 1x1
        }
        return result; //devolvemos el array
    }

    void ValidateSize(int nextIndex)
    {
        if (nextIndex >= internalArray.Length)
        {
            Resize(nextIndex + 1);
        }
    }

    //-------------------------------------------
    void Resize(int targetSize)
    {
        int currentLength = internalArray.Length;

        while (targetSize > currentLength)
        {
            currentLength *= 2;
        }


        //creamos un array del doble del largo del anterior
        T[] nextArray = new T[currentLength];

        //copiamos lo que hay en el array viejo en el nuevo
        for (int i = 0; i < count; i++)
        {
            nextArray[i] = internalArray[i];
        }

        //reemplazamos el array actual por el nuevo mas grande
        internalArray = nextArray;
    }


    //Corremos todo lo que viene despues de index, uno para adelante
    void ShiftLeft(int index)
    {
        for (int i = index; i < count; i++)
        {
            //lo que esta en el casillero actual se pisa con el siguiente
            internalArray[i] = internalArray[i + 1];
        }
        //vaciamos el ultimo espacio para evitar que guarde algo que no sirve o crashee
        internalArray[count] = default;
    }
}
