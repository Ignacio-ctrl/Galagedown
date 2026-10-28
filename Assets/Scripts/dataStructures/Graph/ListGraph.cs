using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class ListGraph<T> : IGraph<T>
{
    SimpleArrayDictionary<T, SimpleArrayList<Edge<T>>> adjacencyList;

    public ListGraph()
    {
        adjacencyList = new SimpleArrayDictionary<T, SimpleArrayList<Edge<T>>>();
    }

    public T[] Vertices => adjacencyList.Keys();

    public bool AddEdge(T from, T to, int weight)
    {
        // Agregamos los vertices
        // Si ya estan, sigue de largo
        AddVertex(from);
        AddVertex(to);
        // Buscamos el edge de from a to
        Edge<T> edge = GetEdge(from, to);
        // Si no existe el edge, lo creamos
        if (edge == null)
        {
            adjacencyList[from].Add(new Edge<T>(to, weight));
            return true;
        }
        // Si el peso no es igual, actualizamos
        if (edge.weight != weight)
        {
            edge.weight = weight;
            return true;
        }
        // Si llegamos hasta aca, todo es igual, no cambia
        return false;
    }

    public bool AddVertex(T vertex)
    {
        if (ContainsVertex(vertex)) return false;

        adjacencyList.Add(
            vertex,
            new SimpleArrayList<Edge<T>>(4)
        );

        return true;
    }

    public bool ContainsEdge(T from, T to)
    {
        return GetEdge(from, to) != null;
    }

    public bool ContainsVertex(T vertex)
    {
        return adjacencyList.ContainsKey(vertex);
    }

    public ISimpleList<Edge<T>> GetNeighbors(T vertex)
    {
        if (!ContainsVertex(vertex)) return null;
        return adjacencyList[vertex];
    }

    public int GetWeight(T from, T to)
    {
        // Buscamos la arista entre from y to
        Edge<T> targetEdge = GetEdge(from, to);

        // Si no existe, devolvemos -1 (valor imposible)
        if (targetEdge == null) return -1;

        // Si existe, devolvemos el peso
        return targetEdge.weight;
    }

    public bool RemoveEdge(T from, T to)
    {
        throw new System.NotImplementedException();
    }

    public bool RemoveVertex(T vertex)
    {
        // No podemos borrarlo si no estaba
        if (!ContainsVertex(vertex)) return false;

        // Buscamos todos los vertices como origen de un Edge que apunta al que queremos remover
        // Borramos todos los edges para que no queden referencias colgadas
        for (int i = 0; i < Vertices.Length; i++)
        {
            if (Vertices[i].Equals(vertex)) continue;
            Edge<T> targetEdge = GetEdge(Vertices[i], vertex);

            // Aca iria RemoveEdge
            //if (targetEdge != null)
        }

        // Ahora que borramos todas las aristas vinculadas, podemos borrar al vertice
        // Remove va a devolver true porque ya chequeamos que existe
        return adjacencyList.Remove(vertex);
    }

    Edge<T> GetEdge(T from, T to)
    {
        // Si no esta el nodo inicial en el diccionario, no esta el edge
        if (!adjacencyList.ContainsKey(from)) return null;

        // Si llegamos hasta aca, estaba el nodo inicial y guardamos a sus vecinos
        SimpleArrayList<Edge<T>> neighborEdges = adjacencyList[from];

        // Recorremos la lista de Edges
        for (int i = 0; i < neighborEdges.Count; i++)
        {
            // Si el Edge tiene el mismo destino, es el que buscamos
            if (neighborEdges[i].to.Equals(to)) return neighborEdges[i];
        }

        // Si llegamos hasta aca, estaba el inicial pero no el final
        return null;
    }
}
