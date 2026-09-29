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

        // Debería decir 3
        Debug.Log("Objetos en inventario: " + itemsDelJugador.Count); 
        
        // Probamos usar/eliminar un objeto
        UsarObjeto("Escudo");

        // Debería decir 2
        Debug.Log("Objetos restantes: " + itemsDelJugador.Count); 
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