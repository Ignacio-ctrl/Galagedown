using UnityEngine;

public class TreeNode<T>
{
    public T Value;
    public TreeNode<T> Left = null;
    public TreeNode<T> Right = null;

    // Seteamos un valor inicial al nodo
    public TreeNode(T value) => Value = value;
}