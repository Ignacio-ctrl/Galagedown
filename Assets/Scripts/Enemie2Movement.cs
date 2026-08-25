using System.Collections;
using UnityEngine;

public class FanEnemy : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionDistance = 8f;
    [SerializeField] private float stopDistance = 3f;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private int bulletsPerFan = 8;
    [SerializeField] private float fanSpreadAngle = 120f;
    [SerializeField] private float bulletSpeed = 4f;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Referencias")]
    [SerializeField] private GameObject Player;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private bool isFollowing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Buscar jugador
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(Player != null ? "Jugador encontrado" : "NO hay jugador con tag 'Player'");
        }

        StartCoroutine(ShootingRoutine());

        // Configurar layer si no está asignada
        if (playerLayer.value == 0)
        {
            // IMPORTANTE: Crea una Layer llamada "Player" en Project Settings
            playerLayer = LayerMask.GetMask("Player");
            Debug.Log("Layer configurada: " + playerLayer.value);
        }
    }

    void Update()
    {
        if (Player == null)
        {
            Debug.LogWarning("Jugador no encontrado");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;

        // DEBUG: Mostrar siempre el raycast
        Debug.DrawRay(transform.position, directionToPlayer * detectionDistance, Color.yellow);

        // Raycast para ver si hay línea de visión
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            detectionDistance,
            playerLayer
        );

        // LÓGICA CORREGIDA de detección
        if (distanceToPlayer <= detectionDistance)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isFollowing = true;
                Debug.Log("Jugador DETECTADO - Siguiendo");
            }
            else
            {
                isFollowing = false;
                Debug.Log("Jugador OCULTO - No siguiendo");
            }
        }
        else
        {
            isFollowing = false;
        }

        // Rotación SIEMPRE hacia el jugador cuando está en rango
        if (distanceToPlayer <= detectionDistance)
        {
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    void FixedUpdate()
    {
        if (Player != null && isFollowing)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

            // Moverse solo si está fuera de la distancia de parada
            if (distanceToPlayer > stopDistance)
            {
                Vector2 direction = ((Vector2)Player.transform.position - rb.position).normalized;
                Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);
                Debug.Log("Moviéndose hacia jugador");
            }
            else
            {
                Debug.Log("Demasiado cerca - Manteniendo distancia");
            }
        }
    }

    IEnumerator ShootingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            if (isFollowing && Player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
                if (distanceToPlayer <= detectionDistance)
                {
                    ShootFanPattern();
                    Debug.Log("Disparando patrón en abanico");
                }
            }
        }
    }

    void ShootFanPattern()
    {
        float angleStep = fanSpreadAngle / (bulletsPerFan - 1);
        float startAngle = -fanSpreadAngle / 2;

        for (int i = 0; i < bulletsPerFan; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            CreateBullet(currentAngle);
        }
    }

    void CreateBullet(float angle)
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Quaternion bulletRotation = Quaternion.Euler(0, 0, rb.rotation + angle);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

            // Sistema simple de movimiento
            BulletMover mover = bullet.GetComponent<BulletMover>();
            if (mover == null)
            {
                mover = bullet.AddComponent<BulletMover>();
            }
            mover.Setup(bulletSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}

public class BulletMover : MonoBehaviour
{
    private float speed;
    private float lifetime = 3f; // Destruir después de 3 segundos

    public void Setup(float bulletSpeed)
    {
        speed = bulletSpeed;
        Destroy(gameObject, lifetime); // Auto-destrucción
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
    }
}




































