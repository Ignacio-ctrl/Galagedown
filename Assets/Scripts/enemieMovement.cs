using UnityEngine;

public class enemieMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float stopDistance;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private LayerMask playerLayer;

    Rigidbody2D rb;
    private bool isFollowing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        InvokeRepeating("enemieShot", 3f, 3f);

        if (playerLayer.value == 0)
            playerLayer = LayerMask.GetMask("Default");
    }

    void Update()
    {
        if (Player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionDistance, playerLayer);

        Debug.DrawRay(transform.position, directionToPlayer * detectionDistance,
                     isFollowing ? Color.green : Color.red);

        if (distanceToPlayer <= detectionDistance && hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isFollowing = true;
        }
        else if (distanceToPlayer > stopDistance)
        {
            isFollowing = false;
        }

        if (isFollowing || distanceToPlayer <= detectionDistance)
        {
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (isFollowing || distanceToPlayer <= detectionDistance)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, Player.transform.position, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }

    private void enemieShot()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (isFollowing || distanceToPlayer <= detectionDistance)
        {
           
            if (distanceToPlayer <= detectionDistance)
            {
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
        }
    }


}

