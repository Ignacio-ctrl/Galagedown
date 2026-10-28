using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using UnityEngine;

public interface ISimpleDictionary<Tkey, Tvalue>    
{


    public Tvalue this[Tkey key] { get; set; }
    public int Count { get; }

    public bool IsEmpty { get; }

    public void Add(Tkey key, Tvalue value);
    public bool Remove(Tkey key);
    public bool ContainsKey(Tkey key);
    public bool TryAdd(Tkey key, out Tvalue value);
    public bool TryGetValue(Tkey key, out Tvalue value);

    public void Clear();

    public Tkey[] Keys();

    public Tvalue[] Values();


}

