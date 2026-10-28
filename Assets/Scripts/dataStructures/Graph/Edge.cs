using UnityEngine;

public class Edge<T>
{
    public T to;
    public int weight;

    public Edge(T to, int weight)
    {
        this.to = to;
        this.weight = weight;
    }

    // Compara dos Edges para saber si son iguales
    public override bool Equals(object obj)
    {
        if (obj.GetType() != GetType()) return false;

        Edge<T> other = (Edge<T>)obj;

        if (other.to == null || !other.to.Equals(to)) return false;
        if (other.weight != weight) return false;

        return true;
    }
}

