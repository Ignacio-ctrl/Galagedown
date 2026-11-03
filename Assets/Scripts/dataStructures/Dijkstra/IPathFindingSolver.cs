using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfindingSolver<T>
{
    
    public ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> GetAllShortestPaths(GraphNode<T> origin, IGraph<GraphNode<T>> graph);

    public ISimpleList<GraphNode<T>> GetShortestPath(GraphNode<T> origin, GraphNode<T> destination, IGraph<GraphNode<T>> graph);




}
