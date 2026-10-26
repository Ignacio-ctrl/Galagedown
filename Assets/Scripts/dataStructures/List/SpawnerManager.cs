using UnityEngine;
using Galagedon.Estructuras;
using System.Xml.Serialization;

public class SpawnerManager : MonoBehaviour
{
    // Usamos el ArrayList para guardar los Transform de los Spawners
    private SimpleArrayList<Transform> roomSpawners;
    [SerializeField] public int minSpawners = 1;

    void Start()
    {
        roomSpawners = new SimpleArrayList<Transform>(minSpawners);

        // Supongamos que buscamos los hijos de este GameObject que son spawners
        foreach (Transform child in transform)
        {
            roomSpawners.Add(child);
        }
    }

    public Transform GetRandomSpawner()
    {
        if (roomSpawners.Count == 0) return null;
        int randomIndex = UnityEngine.Random.Range(0, roomSpawners.Count);
        return roomSpawners[randomIndex]; // Acceso rapidísimo por índice
    }


}