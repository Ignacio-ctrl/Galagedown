using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BST<T> where T : IComparable
{
    int count = 0;
    public int Count { get => count; }
    TreeNode<T> root = null;
    public bool IsEmpty { get => Count == 0; }
    public int Height { get => GetNodeHeight(root); }
    public int BalanceFactor { get => GetNodeBalanceFactor(root); }
    public void Insert(T value)
    {
        root = InsertRecursive(root, value);
    }

    // Recibe un nodo/ posicion, devuelve el nodo que deberia quedar en esa pos ⟦CONTINUACIÓN NO VERIFICABLE⟧
    protected virtual TreeNode<T> InsertRecursive(TreeNode<T> current, T value)
    {
        // Chequeamos si el nodo es null
        // Caso base: creamos y devolvemos un nuevo nodo
        if (current == null)
        {
            count++;
            return new TreeNode<T>(value);
        }

        // Usamos una comparacion para saber si el valor es "mayor" o "menor"
        int comparison = value.CompareTo(current.Value);

        // Si es menor, recursion a la izquierda
        if (comparison < 0)
            current.Left = InsertRecursive(current.Left, value);
        // Si es mayor, recursion a la derecha
        if (comparison > 0)
            current.Right = InsertRecursive(current.Right, value);

        // Devolvemos el mismo nodo que recibimos
        return current;
    }
    public void Remove(T value)
    {
        root = RemoveRecursive(root, value);
    }
    protected virtual TreeNode<T> RemoveRecursive(TreeNode<T> current, T value)
    {
        // Caso base: llegamos al final del arbol y no estaba el valor
        if (current == null) return null;

        // Usamos una comparacion para saber si es igual, menor o mayor
        int comparison = value.CompareTo(current.Value);

        // Si son iguales, encontramos el valor
        if (comparison == 0)
        {
            count--;
            // Caso 1: no tiene hijos
            if (current.Left == null && current.Right == null)
                return null;

            // Caso 2: tiene un solo hijo
            else if (current.Right == null)
                return current.Left;
            else if (current.Left == null)
                return current.Right;

            // Caso 3: tiene los dos hijos
            else
            {
                // El sucesor es el minimo nodo a la derecha
                TreeNode<T> successor = GetMinChild(current.Right);

                // Copiamos el valor del sucesor y removemos ese nodo
                current.Value = successor.Value;
                current.Right = RemoveRecursive(current.Right, successor.Value);

                // Compensamos el hecho de que cuando se remueva el sucesor,
                // tambien resta a count
                count++;
            }
        }
        // Si es menor, recursion a la izquierda
        if (comparison < 0)
            current.Left = RemoveRecursive(current.Left, value);
        // Si es mayor, recursion a la derecha
        if (comparison > 0)
            current.Right = RemoveRecursive(current.Right, value);

        return current;
    }

    // Partiendo de un nodo, itera siempre a la izquierda
    TreeNode<T> GetMinChild(TreeNode<T> current)
    {
        while (current.Left != null)
            current = current.Left;

        return current;
    }
    // Contains -> ContainsRecursive
    public bool Contains(T value)
    {
        return ContainsRecursive(root, value);
    }

    public bool ContainsRecursive(TreeNode<T> current, T value)
    {
        // Caso base: llegamos al final y no estaba
        if (current == null) return false;

        // Hacemos la comparacion
        int comparison = value.CompareTo(current.Value);

        // Si es menor, recursion a la izquierda
        if (comparison < 0)
            return ContainsRecursive(current.Left, value);

        // Si es mayor, recursion a la derecha
        if (comparison > 0)
            return ContainsRecursive(current.Right, value);

        // Caso base: lo encontramos (no era menor ni mayor)
        return true;
    }

    protected int GetNodeHeight(TreeNode<T> node)
    {
        if (node == null) return -1;
        return 1 + Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right));
    }

    protected int GetNodeBalanceFactor(TreeNode<T> node)
    {
        return GetNodeHeight(node.Left) - GetNodeHeight(node.Right);
    }

    public List<T> GetDescendingElements()
    {
        List<T> result = new List<T>(count);
        ReverseInOrderDFS(root, result);
        return result;
    }

    // DFS
    // Todas las funciones DFS pueden ser reemplazadas
    // En vez de devolver void pueden devolver ISimpleList<T> u otros datos
    public void PreOrder()
    {
        PreOrderDFS(root);
    }

    void PreOrderDFS(TreeNode<T> current)
    {
        if (current == null) return;

        // Reemplazamos "Debug" por lo que quieran
        // Puede ser agregar a una lista, borrar, etc.
        Debug.Log(current.Value.ToString());
        PreOrderDFS(current.Left);
        PreOrderDFS(current.Right);
    }

    public void InOrder()
    {
        InOrderDFS(root);
    }

    void InOrderDFS(TreeNode<T> current)
    {
        if (current == null) return;

        InOrderDFS(current.Left);
        Debug.Log(current.Value.ToString());
        InOrderDFS(current.Right);
    }

    void ReverseInOrderDFS(TreeNode<T> current, List<T> list)
    {
        if (current == null) return;

        // primero procesamos primero el subarbol derecho (los mayores)
        ReverseInOrderDFS(current.Right, list);

        // agregamos ell nodo actual
        list.Add(current.Value);

        // desp procesamos el subarbol izquierdo (los menores)
        ReverseInOrderDFS(current.Left, list);
    }

    public void PostOrder()
    {
        PostOrderDFS(root);
    }

    void PostOrderDFS(TreeNode<T> current)
    {
        if (current == null) return;

        PostOrderDFS(current.Left);
        PostOrderDFS(current.Right);
        Debug.Log(current.Value.ToString());
    }

    // BFS
    public ISimpleList<T> LevelOrder()
    {
        // Inicializamos la lista que vamos a devolver y la queue
        ISimpleList<T> result = new SimpleArrayList<T>(count);
        SimpleLinkedQueue<TreeNode<T>> queue = new();
        queue.Enqueue(root);

        // Encolar a los hijos de cada nodo asegura que se recorra el arbol por ⟦CONTINUACIÓN FUERA DE PANTALLA⟧
        while (queue.Count > 0)
        {
            TreeNode<T> current = queue.Dequeue();
            result.Add(current.Value);
            queue.Enqueue(current.Left);
            queue.Enqueue(current.Right);
        }

        return result;
    }
}
