using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class enemieMovement : MonoBehaviour
{
    private enum EnemyAction
    {
        Wait,
        Regroup,
        ChasePlayer
    }

    [Header("Movimiento")]
    [SerializeField] private float speed = 3f;

    // Aumento fijo: 3 pasa a 3.10.
    [SerializeField] private float regroupSpeedBonus = 0.10f;

    [Header("Companeros")]
    [SerializeField] private string enemyTag = "Enemie";

    // Distancia necesaria para considerar que llego al grupo.
    [SerializeField] private float joinDistance = 2f;

    // Debe ser mayor que joinDistance.
    [SerializeField] private float separationDistance = 4f;

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
    [SerializeField] private bool hasCompany;
    [SerializeField] private float currentSpeed;

    private Rigidbody2D rb;
    private Transform nearestAlly;
    private float decisionTimer;

    private readonly IPriorityQueue<EnemyAction> decisions =
        new PriorityQueue<EnemyAction>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Player == null)
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

    private void EvaluateSituation()
    {
        FindNearestAlly();
        UpdateCompany();

        decisions.Clear();

        decisions.Enqueue(EnemyAction.Wait, 0);

        if (Player != null)
        {
            decisions.Enqueue(EnemyAction.ChasePlayer, 50);
        }

        // Reagruparse tiene prioridad sobre perseguir.
        if (nearestAlly != null && !hasCompany)
        {
            decisions.Enqueue(EnemyAction.Regroup, 100);
        }

        currentAction = decisions.Dequeue();

        currentSpeed = speed;

        if (currentAction == EnemyAction.Regroup)
        {
            currentSpeed += regroupSpeedBonus;
        }
    }

    private void FindNearestAlly()
    {
        nearestAlly = null;

        float nearestSquaredDistance = float.PositiveInfinity;

        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject)
            {
                continue;
            }

            Vector2 difference =
                (Vector2)enemy.transform.position - rb.position;

            float squaredDistance = difference.sqrMagnitude;

            if (squaredDistance < nearestSquaredDistance)
            {
                nearestSquaredDistance = squaredDistance;
                nearestAlly = enemy.transform;
            }
        }
    }

    private void UpdateCompany()
    {
        if (nearestAlly == null)
        {
            hasCompany = false;
            return;
        }

        float distance = Vector2.Distance(
            rb.position,
            nearestAlly.position);

        if (!hasCompany && distance <= joinDistance)
        {
            hasCompany = true;
        }
        else if (hasCompany && distance > separationDistance)
        {
            hasCompany = false;
        }
    }

    private void FixedUpdate()
    {
        // Revisa la llegada en cada paso de fisica.
        // Asi recupera la velocidad normal sin esperar
        // hasta la proxima evaluacion.
        if (currentAction == EnemyAction.Regroup)
        {
            if (nearestAlly == null ||
                Vector2.Distance(rb.position, nearestAlly.position)
                <= joinDistance)
            {
                EvaluateSituation();
            }
        }

        switch (currentAction)
        {
            case EnemyAction.Regroup:
                if (nearestAlly != null)
                {
                    MoveTowards(nearestAlly.position);
                }
                break;

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

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        // Sigue avanzando hacia el objetivo.
        // No se detiene a la antigua stopDistance.
        Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            target,
            currentSpeed * Time.fixedDeltaTime);

        rb.MovePosition(nextPosition);

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
        regroupSpeedBonus = Mathf.Max(0f, regroupSpeedBonus);
        joinDistance = Mathf.Max(0.1f, joinDistance);

        separationDistance = Mathf.Max(
            joinDistance + 0.1f,
            separationDistance);

        decisionInterval = Mathf.Max(0.02f, decisionInterval);
    }
}





