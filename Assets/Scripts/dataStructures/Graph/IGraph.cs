using Galagedon.DataStructures.CoreStructures;
using UnityEngine;

public interface IGraph<T>
{
    public T[] Vertices { get; }
    public bool AddVertex(T vertex);
    public bool RemoveVertex(T vertex);
    public bool AddEdge(T from, T to, int weight);
    public bool RemoveEdge(T from, T to);

    public ISimpleList<Edge<T>> GetNeighbors(T vertex);

    public bool ContainsVertex(T vertex);
    public bool ContainsEdge(T from, T to);
    public int GetWeight(T from, T to);
}
