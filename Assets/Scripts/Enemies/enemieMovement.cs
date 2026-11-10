using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class enemieMovement : MonoBehaviour
{
    private enum EnemyAction
    {
        Wait,
        ChasePlayer // Saque el estado "Regroup"
    }

    [Header("Movimiento")]
    [SerializeField] private float speed = 3f;

    // Asegurate de que sea menor que detectionDistance para que dispare
    [SerializeField] private float stoppingDistance = 5f;

    [Header("Disparo")]
    [SerializeField] private float detectionDistance = 8f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Jugador")]
    [SerializeField] private GameObject Player;

    [Header("Decisiones")]
    [SerializeField] private float decisionInterval = 0.2f;

    [Header("Estado actual - observar en Play")]
    [SerializeField] private EnemyAction currentAction;
    [SerializeField] private float currentSpeed;

    private Rigidbody2D rb;
    private float decisionTimer;

    private readonly IPriorityQueue<EnemyAction> decisions =
        new PriorityQueue<EnemyAction>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Validamos que el Player asignado sea el de la escena actual, no un Prefab (que suele estar en 0,0,0)
        if (Player == null || !Player.scene.IsValid())
        {
            Player = GameObject.FindWithTag("Player");
        }

        EvaluateSituation();

        InvokeRepeating(nameof(enemieShot), 3f, 3f);
    }

    private void Update()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0f;
            EvaluateSituation();
        }
    }

    private void UpdatePlayerReference()
    {
        if (Player == null || !Player.activeInHierarchy)
        {
            Player = GameObject.FindWithTag("Player");
        }
    }

    private void EvaluateSituation()
    {
        UpdatePlayerReference();

        decisions.Clear();

        decisions.Enqueue(EnemyAction.Wait, 0);

        // Como el jugador muere de forma definitiva (no revive), si Player != null, va directo a atacarlo
        if (Player != null)
        {
            decisions.Enqueue(EnemyAction.ChasePlayer, 50);
        }

        currentAction = decisions.Dequeue();

        currentSpeed = speed; // Ya no hay bonus de velocidad
    }

    private void FixedUpdate()
    {
        switch (currentAction)
        {
            case EnemyAction.ChasePlayer:
                if (Player != null)
                {
                    MoveTowards(Player.transform.position);
                }
                break;

            case EnemyAction.Wait:
                break;
        }
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = target - rb.position;
        float distance = direction.magnitude;

        if (distance < 0.0001f)
        {
            return;
        }

        // Solo avanza hacia el objetivo si está más lejos que la stoppingDistance
        // Si está más cerca, se queda quieto manteniendo la distancia
        if (distance > stoppingDistance)
        {
            Vector2 nextPosition = Vector2.MoveTowards(
                rb.position,
                target,
                currentSpeed * Time.fixedDeltaTime);

            rb.MovePosition(nextPosition);
        }

        // Aunque no avance, sigue rotando para mirar hacia el jugador
        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg - 90f;

        rb.MoveRotation(angle);
    }

    private void enemieShot()
    {
        if (currentAction != EnemyAction.ChasePlayer)
        {
            return;
        }

        if (Player == null ||
            bullet == null ||
            bulletSpawnPoint == null)
        {
            return;
        }

        float distance = Vector2.Distance(
            rb.position,
            Player.transform.position);

        if (distance > detectionDistance)
        {
            return;
        }

        Instantiate(
            bullet,
            bulletSpawnPoint.position,
            bulletSpawnPoint.rotation);
    }

    private void OnValidate()
    {
        speed = Mathf.Max(0f, speed);
        stoppingDistance = Mathf.Max(0.1f, stoppingDistance);
        decisionInterval = Mathf.Max(0.02f, decisionInterval);
    }
}