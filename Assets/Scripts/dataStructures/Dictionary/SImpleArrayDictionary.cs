using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SImpleArrayDictionary<Tkey, Tvalue> : IsimpleDictionary<Tkey, Tvalue>
{
    KeyValuePair<Tkey, Tvalue>[] internalArray;

    int defaultCapacity = 5;
    int count = 0;
    public SImpleArrayDictionary() => internalArray = new KeyValuePair<Tkey, Tvalue>[defaultCapacity];
    
    public Tvalue this[Tkey key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

  
    public int Count => count;

    public bool IsEmpty => count == 0;

    public void add(Tkey key, Tvalue value)
    {
        
        if(ContainsKey(key))
        {
            throw new System.ArgumentException("Key already exists in the dictionary");
        }
        ExcecuteAdd(key, value);
    }

    public void Clear()
    {
        internalArray = new KeyValuePair<Tkey, Tvalue> [count];
        count = 0;
    }

    public bool ContainsKey(Tkey key)
    {
        if (key == null)
        {
            throw new System.ArgumentNullException("Key cannot be null");
        }
        return indexOf(key) >= 0;
        
    }

    public Tkey[] Keys()
    {
        Tkey[] result = new Tkey[count]; //creo un array con la cantidad de espacios ocupados (count)

        for (int i = 0; i < count; i++)
        {
            result[i] = internalArray[i].Key; //copiamos 1x1
        }
        return result; //devolvemos el array
    }

    public bool remove(Tkey key)
    {
        throw new System.NotImplementedException();
    }

    public bool TryAdd(Tkey key, out Tvalue value)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetValue(Tkey key, out Tvalue value)
    {
        throw new System.NotImplementedException();
    }

    public Tvalue[] Values()
    {
        Tvalue[] result = new Tvalue[count]; //creo un array con la cantidad de espacios ocupados (count)

        for (int i = 0; i < count; i++)
        {
            result[i] = internalArray[i].Value; //copiamos 1x1
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

    void Resize(int targetSize)
    {
        //FIX: si el largo fuera 0, multiplicar por 2 nunca lo hace crecer
        int currentLength = System.Math.Max(internalArray.Length, 1);

        while (targetSize > currentLength)
        {
            currentLength *= 2;
        }

        //creamos un array del doble del largo del anterior
        KeyValuePair<Tkey, Tvalue>[] nextArray = new KeyValuePair<Tkey, Tvalue>[currentLength];

        //copiamos lo que hay en el array viejo en el nuevo
        for (int i = 0; i < count; i++)
        {
            nextArray[i] = internalArray[i];
        }

        //reemplazamos el array actual por el nuevo mas grande
        internalArray = nextArray;
    }

    //recorre el set y devuelve el indice del elemento, o -1 si no esta (valor centinela)
    int indexOf(Tkey key)
    {
        for (int i = 0; i < count; i++)
        {
            //FIX: EqualityComparer evita el NullReferenceException si hay elementos null
            if (internalArray[i].Key.Equals(key)) return i;
        }
        return -1;
    }

    void ExcecuteAdd(Tkey key, Tvalue value)
    {
        ValidateSize(count);
        internalArray[count] = new KeyValuePair<Tkey, Tvalue>(key, value);
        count++;
    }
}
