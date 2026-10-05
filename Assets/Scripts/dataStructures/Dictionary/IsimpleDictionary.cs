using UnityEngine;

public interface IsimpleDictionary<Tkey, Tvalue>    
{


    public Tvalue this[Tkey key] { get; set; }
    public int Count { get; }

    public bool IsEmpty { get; }

    public void add(Tkey key, Tvalue value);
    public bool remove(Tkey key);
    public bool ContainsKey(Tkey key);
    public bool TryAdd(Tkey key, out Tvalue value);
    public bool TryGetValue(Tkey key, out Tvalue value);

    public void Clear();

    public Tkey[] Keys();

    public Tvalue[] Values();


}
