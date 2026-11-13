using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemyOrder;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private bool loop = false;

    private ISimpleQueue<GameObject> enemyQueue = new SimpleLinkedQueue<GameObject>();
    private float spawnTimer;

    //verificar si ya salieron los enemigos del spawner.
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
            Debug.Log($"Enqueued enemy: {enemyOrder[i].name}");
        }
    }

    private void SpawnNext()
    {
        if (enemyQueue.IsEmpty)
            return;

        GameObject enemyPrefab = enemyQueue.Dequeue();

        if (enemyPrefab == null)
            return;

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void RestarSpawner() { 
        // volvemos a cargar la cola de enemigos
        LoadQueue();


        //agregamos enemigos si es combate pro
        if(enemyOrder.Length > 0 && enemyOrder[0] != null)
        {
            //agregamos enemigos a la cola
            for (int i = 0; i < enemyOrder.Length; i++)
            {
                enemyQueue.Enqueue(enemyOrder[0]);
                
            }
        }

        //reiniciamos el temporizador de spawn
        spawnTimer = 0f;

    }
}
