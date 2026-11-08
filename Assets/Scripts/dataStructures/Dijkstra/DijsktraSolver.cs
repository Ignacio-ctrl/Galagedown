using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine;

public class DijsktraSolver<T>: IPathfindingSolver<T>
{
    public ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> GetAllShortestPaths(GraphNode<T> origin, IGraph<GraphNode<T>> graph)
    {
        //creamos un diccionario para almacenar los caminos más cortos desde el origen a cada vértice     
        ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> result = new SimpleArrayDictionary<GraphNode<T>, Edge<GraphNode<T>>>();

        //creamos una cola de prioridad para almacenar los vértices no visitados y sus distancias desde el origen
        SimpleArrayPriorityQueue<GraphNode<T>> unvisited = new SimpleArrayPriorityQueue<GraphNode<T>>();

        //creamos un conjunto para almacenar los vértices visitados
        SimpleArraySet<GraphNode<T>> visited = new SimpleArraySet<GraphNode<T>>();


        //obtenemos todos los vértices del grafo
        GraphNode<T>[] vertices = graph.Vertices;
        //inicializamos el diccionario/tabla con los vértices y un valor inicial de infinito (o un valor muy grande) para cada uno
        for (int i = 0; i < vertices.Length; i++) {

            result.Add(vertices[i], new Edge<GraphNode<T>>(null, int.MaxValue));

        }

        result[origin].weight = 0; //el peso del origen a sí mismo es 0

        unvisited.Enqueue(origin, result[origin].weight); //agregamos el origen a la cola de prioridad con peso 0

        
        while (!unvisited.IsEmpty()) 
        {
            //Sacamos el primer nodo de la queue
            GraphNode<T> current = unvisited.Dequeue();

            if (visited.Contains(current)) continue;

            ISimpleList<Edge<GraphNode<T>>> neighbors = graph.GetNeighbors(current);

            for (int i = 0; i < neighbors.Count; i++)
            {
                //sumamos costo total hasta el nodo actual
                //mas el cosoto de ese nodo actual hasta el vecino que estamos revisando
                int totalWeight = result[current].weight + neighbors[i].weight;

                //si el costo total es menor al que esta en la tabla para el vecino
                // actualizamos ese coto con el que calculamos 
                if(totalWeight < result[neighbors[i].to].weight)
                    result[neighbors[i].to].weight = totalWeight;


            }
            //Marcamos el nodo como visitado para no volverlo a evaluar
            visited.Add(current);
        }

        //devolvemos la tabla completa
        return result;
    }

    public ISimpleList<GraphNode<T>> GetShortestPath(GraphNode<T> origin, GraphNode<T> destination, IGraph<GraphNode<T>> graph)
    {
        

        //creamos un diccionario para almacenar los caminos más cortos desde el origen a cada vértice     
        ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> table = new SimpleArrayDictionary<GraphNode<T>, Edge<GraphNode<T>>>();

        
        //creamos una cola de prioridad para almacenar los vértices no visitados y sus distancias desde el origen
        SimpleArrayPriorityQueue<GraphNode<T>> unvisited = new SimpleArrayPriorityQueue<GraphNode<T>>();

        //creamos un conjunto para almacenar los vértices visitados
        SimpleArraySet<GraphNode<T>> visited = new SimpleArraySet<GraphNode<T>>();


        //obtenemos todos los vértices del grafo
        GraphNode<T>[] vertices = graph.Vertices;
        //inicializamos el diccionario/tabla con los vértices y un valor inicial de infinito (o un valor muy grande) para cada uno
        for (int i = 0; i < vertices.Length; i++)
        {

            table.Add(vertices[i], new Edge<GraphNode<T>>(null, int.MaxValue));

        }

        table[origin].weight = 0; //el peso del origen a sí mismo es 0

        unvisited.Enqueue(origin, table[origin].weight); //agregamos el origen a la cola de prioridad con peso 0


        while (!unvisited.IsEmpty())
        {
            //Sacamos el primer nodo de la queue
            GraphNode<T> current = unvisited.Dequeue();

            //si el nodo actual es el destino, terminamos la búsqueda
            if (current == destination)
            {
               
                break;
            }
            //si ya visitamos este nodo, lo saltamos
            if (visited.Contains(current)) continue;

            //obtenemos los vecinos del nodo actual
            ISimpleList<Edge<GraphNode<T>>> neighbors = graph.GetNeighbors(current);

            for (int i = 0; i < neighbors.Count; i++)
            {
                //sumamos costo total hasta el nodo actual
                //mas el cosoto de ese nodo actual hasta el vecino que estamos revisando
                int totalWeight = table[current].weight + neighbors[i].weight;

                //si el costo total es menor al que esta en la tabla para el vecino
                // actualizamos ese costo con el que calculamos 
                if (totalWeight < table[neighbors[i].to].weight)
                    table[neighbors[i].to].weight = totalWeight;
                //agregamos vecino a la queue si no lo visitamos
                if (!visited.Contains(neighbors[i].to))
                    unvisited.Enqueue(neighbors[i].to, totalWeight);


            }
            //Marcamos el nodo como visitado para no volverlo a evaluar
            visited.Add(current);
        }

        if (table[destination].to == null) return null;

        return GeneratePath(destination, table);

    }
        ISimpleList<GraphNode<T>> GeneratePath(GraphNode<T> destination, ISimpleDictionary<GraphNode<T>, Edge<GraphNode<T>>> table)
        {

            //creamos la lista de atras para adelante, ya que vamos a recorrer la tabla desde el destino hasta el origen
            ISimpleStack<GraphNode<T>> reversePath = new SimpleLinkedStack<GraphNode<T>>();

            //empezamos desde el destino y vamos hacia atrás hasta llegar al origen
            GraphNode<T> current = destination;

            //mientras no lleguemos al origen, vamos agregando los nodos a la lista
            while (current != null)
            {
                reversePath.Push(current);
                current = table[current].to;
            }

        

            //creamos el path definitivo (del primero al ultimo)
            ISimpleList <GraphNode<T>> path = new SimpleArrayList<GraphNode<T>>(reversePath.Count);


            //invertimos la lista para que quede del origen al destino
            while(!reversePath.IsEmpty())
            {
                path.Add(reversePath.Pop());
            }
       

            return path;
        }
}

