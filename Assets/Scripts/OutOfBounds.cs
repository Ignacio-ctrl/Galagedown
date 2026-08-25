using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private float timeForHorde = 10f;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private int minEnemies = 8;
    [SerializeField] private int maxEnemies = 15;
    [SerializeField] private float enemySRadius = 3f;
    [SerializeField] private float SpawnEFromPlayer = 2f;

    private bool isOutOfBounds;
    private float outOfBoundsTimer = 0f;
    private Coroutine hordeCoroutine;

    // Update is called once per frame
    private void Update()
    {
        if (isOutOfBounds)
        {
            outOfBoundsTimer += Time.deltaTime;
            

            if (outOfBoundsTimer >= timeForHorde && hordeCoroutine == null)
            {
                hordeCoroutine = StartCoroutine(SpawnEnemyHorde());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayZone"))
        {
            ReturnToSafety();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayZone"))
        {
            EnterOutOfBounds();
        }
    }

    void EnterOutOfBounds()
    {
        isOutOfBounds = true;
        outOfBoundsTimer = 0f;
        Debug.Log("¡Saliste de los límites! Tienes " + timeForHorde + " segundos...");

    }
    void ReturnToSafety()
    {
        isOutOfBounds = false;
        outOfBoundsTimer = 0f;

        if (hordeCoroutine != null)
        {
            StopCoroutine(hordeCoroutine);
            hordeCoroutine = null;
            
        }
    }

        IEnumerator SpawnEnemyHorde()
        {
             int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

          for (int i = 0; i < enemyCount; i++)
            {
            SpawnSingleEnemy();

            // Esperar un poco entre spawns para no laggear
            yield return new WaitForSeconds(0.2f);
            }
        

            hordeCoroutine = null;
        }
    void SpawnSingleEnemy()
    {
        if (EnemyPrefab != null)
        {
            // Calcular posición de spawn alrededor del jugador
            Vector2 randomCircle = Random.insideUnitCircle.normalized * SpawnEFromPlayer;
            Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

            // Asegurar que no spawn demasiado cerca
            spawnPosition += (spawnPosition - transform.position).normalized * enemySRadius;

            // Instanciar enemigo
            Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        }
    

    }
}
