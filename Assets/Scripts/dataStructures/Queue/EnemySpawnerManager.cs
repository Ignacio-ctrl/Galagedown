
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyOrder;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private bool loop = false;

    private float speedMultiplier = 1f;
    private float cadenceMultiplier = 1f;


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
        LoadQueue();
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
            enemyQueue.Enqueue(enemyOrder[i]);
            Debug.Log("Enqueued enemy: " + enemyOrder[i].name);
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

    public void RestarSpawner(int extraEnemies = 0, float newSpeedMultiplier = 1f, float newCadenceMultiplier = 1f)
    {
        //guarda nueva dificultad depende de la ronda
        speedMultiplier = newSpeedMultiplier;
        cadenceMultiplier = newCadenceMultiplier;


        // Cargamos la cola normal
        LoadQueue();

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
}
