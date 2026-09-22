using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemyOrder;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private bool loop = false;

    private ISimpleQueue<GameObject> enemyQueue = new SimpleLinkedQueue<GameObject>();
    private float spawnTimer;

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
}
