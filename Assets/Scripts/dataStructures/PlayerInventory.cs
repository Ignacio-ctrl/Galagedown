using UnityEngine;
using Galagedon.Estructuras;

public class PlayerInventory : MonoBehaviour
{
    public SimpleLinkedList<string> itemsDelJugador;

    void Start()
    {
        itemsDelJugador = new SimpleLinkedList<string>();
        
        Debug.Log("--- INICIANDO INVENTARIO ---");
        
        // Agregamos items a la lista enlazada
        itemsDelJugador.Add("Láser Básico");
        itemsDelJugador.Add("Escudo");
        itemsDelJugador.Add("Bomba");
        
        Debug.Log("Objetos en inventario: " + itemsDelJugador.Count); // Debería decir 3
        
        // Probamos usar/eliminar un objeto
        UsarObjeto("Escudo");
        
        Debug.Log("Objetos restantes: " + itemsDelJugador.Count); // Debería decir 2
    }

    public void UsarObjeto(string nombreItem)
    {
        if (itemsDelJugador.Contains(nombreItem))
        {
            Debug.Log("Usando y consumiendo: " + nombreItem);
            itemsDelJugador.Remove(nombreItem);
        }
    }
}