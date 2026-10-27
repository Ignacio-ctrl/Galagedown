using System;
using UnityEngine;

public class AVL<T> : BST<T> where T : IComparable
{
    protected override TreeNode<T> InsertRecursive(TreeNode<T> current, T value)
    {
        // Version comprimida a una linea
        

        // Insertamos normalmente segun BST
        current = base.InsertRecursive(current, value);

        // Balanceamos el nodo que quedo insertado
        current = RebalanceNode(current);

        // Devolvemos el nodo post-balance
        return current;
    }

    protected override TreeNode<T> RemoveRecursive(TreeNode<T> current, T value)
    {
        return RebalanceNode(base.RemoveRecursive(current, value));
    }

    TreeNode<T> RebalanceNode(TreeNode<T> node)
    {
        int bf = GetNodeBalanceFactor(node);

        // Casos Lx
        if (bf > 1)
        {
            // Caso LL
            if (GetNodeBalanceFactor(node.Left) >= 0)
                return RotateRight(node);

            // Caso LR
            else
                return RotateLeftRight(node);
        }
        // Casos Rx
        else if (bf < -1)
        {
            // Caso RR
            if (GetNodeBalanceFactor(node.Right) <= 0)
                return RotateLeft(node);

            // Caso RL
            else
                return RotateRightLeft(node);
        }

        // Si ya esta balanceado, devuelve el mismo nodo que recibio
        return node;
    }

    TreeNode<T> RotateLeftRight(TreeNode<T> node)
    {
        node.Left = RotateLeft(node.Left);
        return RotateRight(node);
    }

    TreeNode<T> RotateRightLeft(TreeNode<T> node)
    {
        node.Right = RotateRight(node.Right);
        return RotateLeft(node);
    }

    TreeNode<T> RotateLeft(TreeNode<T> x)
    {
        TreeNode<T> y = x.Right;
        TreeNode<T> t2 = y.Left;

        y.Left = x;
        x.Right = t2;

        return y;
    }

    TreeNode<T> RotateRight(TreeNode<T> y)
    {
        TreeNode<T> x = y.Left;
        TreeNode<T> t2 = x.Right;

        x.Right = y;
        y.Left = t2;

        return x;
    }
}
