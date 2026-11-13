using UnityEngine;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.UI;
using Unity.Collections;
using Galagedon.DataStructures.CoreStructures;

public class RoundManager : MonoBehaviour
{

    [SerializeField] private EnemySpawnerManager[] spawners;
    [SerializeField] private GameObject roundPanel;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;

    [SerializeField] private string enemyTag = "Enemie";


    private int currentRound = 1;
    private bool roundInProgress = false;

    //grago de sectores de la ronda
    private ListGraph<string> graph = new ListGraph<string>();

    private string currentSector = "Inicio";

    //opciones para la sig ronda
    private ISimpleList<Edge<string>> options;

    private int extraEnemies = 0;

    void Start()
    {
        Time.timeScale = 1f;

        roundPanel.SetActive(false);

        // Creamos las conexiones del grafo
        graph.AddEdge("Inicio", "Combate", 0);
        graph.AddEdge("Inicio", "Elite", 2);

        graph.AddEdge("Combate", "Combate", 0);
        graph.AddEdge("Combate", "Elite", 2);

        graph.AddEdge("Elite", "Combate", 0);
        graph.AddEdge("Elite", "Elite", 2);


        StartRound();
        
    }

    
    void Update()
    {
        if(!roundInProgress)
        {
            return;
        }
        CheckRound();   
    }

    void StartRound()
    {
        //cargamos los enemigos de la ronda actual
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i].RestarSpawner();
        }

        roundInProgress = true;


    }

    void CheckRound()
    {
        //espera que los spawner terminen
        for(int i = 0; i < spawners.Length; i++)
        {
            if(!spawners[i].FinishedSpawning)
            {
                return;
            }
        }

        //identifica los enemigos vivos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if(enemies.Length == 0)
        {
         FinishRound();
        }
    }

    void FinishRound() { 
    
        roundInProgress = false;

        //buscamos caminos del sector actual
        options = graph.GetNeighbors(currentSector);

        if (options == null || options.Count < 2)
        {
            Debug.LogError("No hay suficientes opciones de camino para la ronda actual.");
            return;
        }

        roundPanel.SetActive(true);


        roundText.text = $"Ronda {currentRound} finalizada. Elige el siguiente camino:";

        //mostramos opciones en los botones
        button1.GetComponentInChildren<TMP_Text>().text = options[0].to;    
        button2.GetComponentInChildren<TMP_Text>().text = options[1].to;
        Time.timeScale = 0f;


    }
    public void ChooseFirst()
    {
        ChooseSector(0);
    }

    public void ChooseSecond()
    {
        ChooseSector(1);
    }

    void ChooseSector(int index)
    {
        if (roundInProgress || options == null)
            return;

        if (index < 0 || index >= options.Count)
            return;

        // Guardamos el sector seleccionado
        currentSector = options[index].to;

        // El peso indica cuantos enemigos extra agregar
        extraEnemies = options[index].weight;

        Debug.Log("Sector elegido: " + currentSector);

        currentRound++;

        roundPanel.SetActive(false);
        Time.timeScale = 1f;

        StartRound();
    }

}
