using UnityEngine;

public class Boss2 : MonoBehaviour
{
   
    [SerializeField] private GameObject bulletPrefab;  // Tu prefab de bala con su comportamiento
    [SerializeField] private int bulletsPerTentacle = 8;
    [SerializeField] private float tentacleInterval = 3f;

   
    [SerializeField] private float aimedShotInterval = 1.5f;

    
    [SerializeField] private Transform firePoint;  // Donde salen las balas

    private Transform player;
    private float tentacleRotation = 0f;

    void Start()
    {
        // Buscar jugador
        FindPlayer();

        // Verificar prefab
        if (bulletPrefab == null)
        {
            Debug.LogError("Asigna el prefab de bala en el Inspector!");
            return;
        }

        // Usar este transform si no hay firePoint
        if (firePoint == null) firePoint = transform;

        // Iniciar ataques
        StartCoroutine(StartAttacksWithDelay());
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Jugador encontrado: " + player.name);
        }
        else
        {
            Debug.LogWarning("No se encontró jugador con tag 'Player'");
        }
    }

    System.Collections.IEnumerator StartAttacksWithDelay()
    {
        // Pequeño delay inicial
        yield return new WaitForSeconds(1f);

        // Iniciar ataques repetitivos
        InvokeRepeating("ShootTentacles", 0f, tentacleInterval);
        InvokeRepeating("ShootAtPlayer", 1f, aimedShotInterval);
    }

    void ShootTentacles()
    {
        Debug.Log("Disparando tentáculos!");

        // Calcular ángulo entre balas
        float angleStep = 360f / bulletsPerTentacle;

        // Disparar en todas las direcciones
        for (int i = 0; i < bulletsPerTentacle; i++)
        {
            // Ángulo con rotación progresiva
            float angle = (i * angleStep) + tentacleRotation;

            // Crear bala
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        }

        // Rotar patrón para siguiente disparo
        tentacleRotation += 15f;
    }

    void ShootAtPlayer()
    {
        if (player == null) return;

        Debug.Log("Disparando al jugador!");

        // Calcular ángulo hacia el jugador
        Vector2 direction = player.position - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Crear bala apuntando al jugador
        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
    }

    void OnDrawGizmosSelected()
    {
        // Mostrar punto de disparo
        Gizmos.color = Color.red;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Gizmos.DrawWireSphere(spawnPos, 0.2f);

        // Mostrar dirección de tentáculos (solo en Play Mode)
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            float angleStep = 360f / bulletsPerTentacle;

            for (int i = 0; i < bulletsPerTentacle; i++)
            {
                float angle = (i * angleStep) + tentacleRotation;
                Vector2 dir = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );
                Gizmos.DrawRay(spawnPos, dir * 2f);
            }
        }
    }
}
