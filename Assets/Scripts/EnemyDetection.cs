using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
   
    [SerializeField] private string enemyTag = "Enemie";
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float detectionRadius = 10f;

   
    [SerializeField] private Transform player;

    private Transform nearestEnemy;

    void Start()
    {
        if (player == null && transform.parent != null)
            player = transform.parent;
    }

    void Update()
    {
        FindNearestEnemy();
        MoveIndicatorOrbit2D();
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        nearestEnemy = null;

        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(player.position, enemy.transform.position);

            if (distance <= detectionRadius && distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }
    }

    void MoveIndicatorOrbit2D()
    {
        if (nearestEnemy != null && player != null)
        {
            // Calcular dirección al enemigo en 2D
            Vector2 directionToEnemy = (Vector2)(nearestEnemy.position - player.position);
            directionToEnemy.Normalize();

            // Calcular posición orbital alrededor del jugador
            Vector2 targetPosition = (Vector2)player.position + (directionToEnemy * orbitRadius);

            // Mover suavemente hacia la posición orbital
            transform.position = Vector2.MoveTowards(
                (Vector2)transform.position,
                targetPosition,
                movementSpeed * Time.deltaTime
            );

            // Rotar el indicador para que apunte hacia el enemigo
            Vector2 lookDirection = (Vector2)(nearestEnemy.position - transform.position);
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Comportamiento cuando no hay enemigos
            transform.position = Vector2.MoveTowards(
                (Vector2)transform.position,
                (Vector2)player.position + Vector2.right * orbitRadius,
                movementSpeed * 0.5f * Time.deltaTime
            );
        }
    }

    // Debug visual para 2D
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            // Dibujar radio de detección
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, detectionRadius);

            // Dibujar órbita del indicador
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, orbitRadius);

            // Dibujar línea hacia el enemigo
            if (nearestEnemy != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(player.position, nearestEnemy.position);
            }
        }
    }
}
