
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyOrder;
    [SerializeField] private GameObject fanEnemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private bool loop = false;

    private float speedMultiplier = 1f;
    private float cadenceMultiplier = 1f;
    private bool roundControlled = false;
    private int activeRound = 1;


    private ISimpleQueue<GameObject> enemyQueue =
        new SimpleLinkedQueue<GameObject>();

    private float spawnTimer;

    // Verificamos si ya salieron todos los enemigos
    public bool FinishedSpawning
    {
        get { return enemyQueue.IsEmpty && !loop; }
    }

    private void Start()
    {
        if (!roundControlled)
        {
            LoadQueue();
        }
        
    }

    private void Update()
    {
        if (enemyQueue.IsEmpty)
        {
            if (loop)
            {
                LoadQueue();
            }
            else
            {
                return;
            }
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnNext();
        }
    }

    private void LoadQueue()
    {
        enemyQueue.Clear();

        for (int i = 0; i < enemyOrder.Length; i++)
        {
            GameObject prefab = enemyOrder[i];

            if(prefab == null)
            {
                continue;
            }

            //tipo de enmigo
            EnemyTypeHolder holder = prefab.GetComponent<EnemyTypeHolder>();

            //revisa en que ronda aparece
            if(holder != null && holder.data != null)
            {
                if(activeRound < holder.data.firstRound)
                {
                    continue;
                }
            }


            enemyQueue.Enqueue(prefab);
            
        }
    }

    private void SpawnNext()
    {
        if (enemyQueue.IsEmpty)
            return;

        GameObject enemyPrefab = enemyQueue.Dequeue();

        if (enemyPrefab == null)
            return;

        //usar factory con dificultad de ronda
        EnemyFactory.CreateEnemy(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation,
            speedMultiplier,
            cadenceMultiplier);

    }

    public void RestarSpawner(int extraEnemies = 0, float newSpeedMultiplier = 1f, float newCadenceMultiplier = 1f, int fanEnemies = 0, int roundNumber = 1)
    {

        roundControlled = true;
        //guarda nueva dificultad depende de la ronda
        speedMultiplier = newSpeedMultiplier;
        cadenceMultiplier = newCadenceMultiplier;
        activeRound = roundNumber;

        // Cargamos la cola normal
        LoadQueue();

        //agrega enemigos escopeta
        if(fanEnemyPrefab != null)
        {
            for(int i = 0; i< fanEnemies; i++)
            {
                enemyQueue.Enqueue(fanEnemyPrefab);
            }
        }

        // Si es combate elite agregamos enemigos extra
        if (enemyOrder.Length > 0 && enemyOrder[0] != null)
        {
            for (int i = 0; i < extraEnemies; i++)
            {
                enemyQueue.Enqueue(enemyOrder[0]);
            }
        }

        // Reiniciamos el tiempo de spawn
        spawnTimer = 0f;
    }

    public void StopSpawning()
    {
        //cuando hay boss no genera enemigos normales
        roundControlled = true;
        enemyQueue.Clear();
        spawnTimer = 0f;
    }
}
