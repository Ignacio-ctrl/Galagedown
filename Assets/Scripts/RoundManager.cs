using UnityEngine;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.UI;
using Unity.Collections;
using Galagedon.DataStructures.CoreStructures;
using Galagedon.Estructuras;

public class RoundManager : MonoBehaviour
{

    [SerializeField] private EnemySpawnerManager[] spawners;
    [SerializeField] private GameObject roundPanel;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private Transform bossSpawnpoint;
   

    [SerializeField] private RewardFacade rewards;

    [SerializeField] private string enemyTag = "Enemie";

    private GameObject activeBoss;
    private bool isBossRound = false;

    private int currentRound = 1;
    private bool roundInProgress = false;
    // historial puntaje de la ronda
    private readonly BST<ScoreEntry> roundScores = new BST<ScoreEntry>();

    private int scoreAtRoundStart;

    //grafo de sectores de la ronda
    private ListGraph<string> graph = new ListGraph<string>();

    private string currentSector = "Inicio";

    private readonly ISimpleSet<string> visitedSectorsTypes = new SimpleArraySet<string>();
    //opciones para la sig ronda
    private ISimpleList<Edge<string>> options;

    private int extraEnemies = 0;

    void Start()
    {
        Time.timeScale = 1f;

        roundPanel.SetActive(false);

        // Creamos las conexiones del grafo
        //Caminos desde el inicio
        graph.AddEdge("Inicio", "Combate", 0);
        graph.AddEdge("Inicio", "Elite", 2);
        graph.AddEdge("Inicio", "Tienda", 0);

        //Caminos desde combate normal
        graph.AddEdge("Combate", "Combate", 0);
        graph.AddEdge("Combate", "Elite", 2);
        graph.AddEdge("Combate", "Tienda", 0);

        //caminos desde combate elite
        graph.AddEdge("Elite", "Combate", 0);
        graph.AddEdge("Elite", "Elite", 2);
        graph.AddEdge("Elite", "Tienda", 0);

        //caminos desde la tienda
        graph.AddEdge("Tienda", "Combate", 0);
        graph.AddEdge("Tienda", "Elite", 2);

        //antes del boss
        graph.AddEdge("AntesBoss", "Boss", 0);

        //despues de boss
        graph.AddEdge("Boss", "Combate", 0);
        graph.AddEdge("Boss", "Elite", 2);
        graph.AddEdge("Boss", "Tienda", 0);

        visitedSectorsTypes.Add(currentSector);

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
        // Guardamos el puntaje al inicio de la ronda
        scoreAtRoundStart = ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore : 0;

        isBossRound = currentSector == "Boss";
        activeBoss = null;

        if (isBossRound)
        {
            // Elegimos el Boss que corresponde
            int bossIndex = currentRound / 6 - 1;

            if (bossPrefabs != null && bossPrefabs.Length > 0 && bossSpawnpoint != null)
            {
                bossIndex = bossIndex % bossPrefabs.Length;

                if (bossPrefabs[bossIndex] != null)
                {
                    // Detenemos los spawners normales
                    for (int i = 0; i < spawners.Length; i++)
                    {
                        spawners[i].StopSpawning();
                    }

                    activeBoss = Instantiate(bossPrefabs[bossIndex], bossSpawnpoint.position, bossSpawnpoint.rotation);

                    roundInProgress = true;

                    
                    return;
                }
            }

            // Si falta configurar un Boss, hacemos ronda normal
           
            isBossRound = false;
        }

        // Agregamos enemigos segun el numero de ronda
        int progressiveEnemies =
            Mathf.Min(4, (currentRound - 1) / 2);

        int totalEnemies =
            progressiveEnemies + extraEnemies;

        // Progresion de velocidad y cadencia
        float speedMultiplier =
            Mathf.Min(1.6f, 1f + (currentRound - 1) * 0.06f);

        float cadenceMultiplier =
            Mathf.Min(2f, 1f + (currentRound - 1) * 0.08f);

        // Los elites son mas rapidos
        if (currentSector == "Elite")
        {
            speedMultiplier += 0.1f;
            cadenceMultiplier += 0.2f;
        }

        // Desde la ronda 4 aparece el enemigo de abanico
        int fanEnemies = 0;

        if (currentRound >= 4)
            fanEnemies = 1;

        if (currentRound >= 8)
            fanEnemies = 2;

        // Cargamos la ronda en los spawners
        for (int i = 0; i < spawners.Length; i++)
        {
            // Por ahora el enemigo nuevo sale del primer spawner
            int fanCount = 0;

            if (i == 0)
                fanCount = fanEnemies;

            spawners[i].RestarSpawner(
                totalEnemies,
                speedMultiplier,
                cadenceMultiplier,
                fanCount,
                currentRound);
        }

        roundInProgress = true;

        
    }


    void CheckRound()
    {

        //si es boss se espera a que este muera
        if (isBossRound)
        {
            if(activeBoss == null)
            {
                FinishRound();
            }
            return;
        }
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


    void FinishRound()
    {
        roundInProgress = false;

        string message = "Ronda " + currentRound + " completa";

        // Recompensa por derrotar al Boss
        if (isBossRound)
        {
            message += "\nBOSS DERROTADO";

            ShopItem reward = null;

            if (rewards != null)
                reward = rewards.CompleteBossRound();

            if (reward != null)
            {
                message += "\nRecompensa: " + reward.displayName;
            }

            message += "\n+800 puntos";

        }
        // Recompensa por completar un elite
        else if (currentSector == "Elite")
        {
            ShopItem reward = null;

            if (rewards != null)
                reward = rewards.CompleteEliteRound();

            if (reward != null)
            {
                message += "\nRecompensa elite: " + reward.displayName;
            }
            else
            {
                message += "\nNo quedan recompensas elite";
            }

            message += "\n+300 puntos";
        }

        
        if (ScoreManager.Instance != null)
        {
            int pointsEarned = ScoreManager.Instance.CurrentScore - scoreAtRoundStart;
            // Guardamos el puntaje de la ronda en nuestro BST
            ScoreEntry roundEntry = new ScoreEntry("Ronda " + currentRound + " (" + currentSector + ")", pointsEarned );

            // Guardamos el resultado en nuestro BST
            roundScores.Insert(roundEntry);

            // El BST devuelve las mejores puntuaciones primero
            var bestRounds = roundScores.GetDescendingElements();

            message += "\nPuntos obtenidos: " + pointsEarned;

            if (bestRounds.Count > 0)
            {
                ScoreEntry best = bestRounds[0];

                message += "\nMejor ronda: " + best.PlayerName + " - " + best.Points + " pts";
            }
        }


        ShowOptions(message);
    }


    void ShowOptions(string message = "")
    {
        //tenemos los vecinos del sector actual
        //si es multiplo de 6 solo aparece camino boss
        if((currentRound + 1) % 6 == 0)
        {
            options = graph.GetNeighbors("AntesBoss");
        }
        else
        {
            options = graph.GetNeighbors(currentSector);
        }

        if(options == null || options.Count == 0)
        {
            return;
        }

        roundPanel.SetActive(true);

        if(message == "")
        {
            roundText.text = "Sellecciona siguiente sector";
        }
        else
        {
            roundText.text = message + "\nElegi el siguiente sector";
        }

        //botones que tienen los dif caminos
        Button[] buttons = { button1, button2, button3 };

        for(int i = 0; i < buttons.Length; i++)
        {
            bool aviable = i < options.Count;

            buttons[i].gameObject.SetActive(aviable);

            if (aviable)
            {
                TMP_Text text = buttons[i].GetComponentInChildren<TMP_Text>();

                if(text != null)
                {
                    //nombre del sector y si fue visitado
                    string sectorName = options[i].to;

                    bool visited = visitedSectorsTypes.Contains(sectorName);
                    //si fue visitado se indica
                    text.text = sectorName + (visited ? " (visitado)" : " (Nuevo)");
                }
            }
        }
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

    public void ChooseThird()
    {
        ChooseSector(2);
    }

    void ChooseSector(int index)
    {
        if (roundInProgress || options == null)
            return;

        if (index < 0 || index >= options.Count)
            return;

        string nextSector = options[index].to;

        //revisar que el boss este 
        if(nextSector == "Boss")
        {
            int bossIndex = (currentRound + 1) / 6 - 1;

            if (bossPrefabs == null || bossPrefabs.Length == 0 || bossSpawnpoint == null)
            {
                return;
            }

            bossIndex = bossIndex % bossPrefabs.Length;

            if (bossPrefabs[bossIndex] == null)
            {
                return;
            }
        }

        //Si no esta la tienda no se avanza
        if(nextSector == "Tienda" && (rewards  == null || !rewards.HasShop))
        {
            return;
        }

        // Guardamos el sector seleccionado
        currentSector = options[index].to;

        // Guardamos el sector visitado para no repetirlo
        visitedSectorsTypes.Add(currentSector);

        // El peso indica cuantos enemigos extra agregar
        extraEnemies = options[index].weight;

        options = null;
        roundPanel.SetActive(false);

        Debug.Log("Sector elegido: " + currentSector);

        //si se elige tienda no inicia el combate
        if(currentSector == "Tienda")
        {
            rewards.OpenShop();
            return;
        }

        //si no es tienda inicia ronda
        currentRound++;

        
        Time.timeScale = 1f;

        StartRound();
    }

   public void ShopFinished()
   {
        if(currentSector != "Tienda" || roundInProgress)
        {
            return;
        }

        ShowOptions();
   }

}
