using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject areaAttackPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private LayerMask playerLayer; 
    [SerializeField] private LayerMask ignoreLayers; 

   
    [SerializeField] private float detectionDistance = 20f;

   
    [SerializeField] private float timeBetweenAttacks = 3f;
    [SerializeField] private float areaAttackWarningTime = 1.5f; 
    [SerializeField] private float areaAttackDuration = 3f; 
    [SerializeField] private float timeBetweenAreaAttacks = 7f; 

    [SerializeField] private AudioClip normalAttackSound;
    [SerializeField] private AudioClip shotgunAttackSound;
    [SerializeField] private AudioClip areaAttackWarningSound; 
    [SerializeField] private AudioClip areaAttackDamageSound;  

    private Rigidbody2D rb;
    private bool isPlayerDetected = false;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();

        if (playerLayer.value == 0)
            playerLayer = LayerMask.GetMask("Default");


        StartCoroutine(AttackPattern());

  
        StartCoroutine(AreaAttackPattern());
    }

    void Update()
    {
        if (Player == null)
        {
            isPlayerDetected = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 directionToPlayer = (Player.transform.position - transform.position).normalized;


        int finalLayerMask = playerLayer.value & ~ignoreLayers.value; 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionDistance, finalLayerMask);
        Debug.DrawRay(transform.position, directionToPlayer * detectionDistance, isPlayerDetected ? Color.green : Color.red);

        if (distanceToPlayer <= detectionDistance && hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isPlayerDetected = true;
        }
        else
        {
            isPlayerDetected = false;
        }

        if (isPlayerDetected)
        {
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    private IEnumerator AttackPattern()
    {
        while (true)
        {
            if (isPlayerDetected)
            {
                Debug.Log("Jefe: Ataque Normal");
                PerformNormalAttack();
                yield return new WaitForSeconds(timeBetweenAttacks); 

                if (!isPlayerDetected) continue; 

                Debug.Log("Jefe: Ataque Escopetazo");
                PerformShotgunAttack();
                yield return new WaitForSeconds(timeBetweenAttacks);

            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void PerformNormalAttack()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            if (normalAttackSound != null)
            {
                audioSource.PlayOneShot(normalAttackSound);
            }
        }
    }

    private void PerformShotgunAttack()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0, 0, 0)); 
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0, 0, 90));  
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0, 0, 180));
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0, 0, 270)); 

        if (shotgunAttackSound != null)
        {
            audioSource.PlayOneShot(shotgunAttackSound);
        }
    }

    private IEnumerator PerformAreaAttack()
    {
        if (areaAttackPrefab != null && Player != null)
        {
            if (areaAttackWarningSound != null)
            {
                audioSource.PlayOneShot(areaAttackWarningSound);
            }

            Vector3 spawnPosition = Player.transform.position;


            GameObject aoeInstance = Instantiate(areaAttackPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(areaAttackWarningTime);

            if (aoeInstance != null) 
            {

                if (areaAttackDamageSound != null)
                {
                    audioSource.PlayOneShot(areaAttackDamageSound);
                }

                Transform warningVisual = aoeInstance.transform.Find("WarningVisual");
                if (warningVisual != null)
                {
                    warningVisual.gameObject.SetActive(false);
                }

                Transform damageVisual = aoeInstance.transform.Find("DamageVisual");
                if (damageVisual != null)
                {
                    damageVisual.gameObject.SetActive(true);
                }

                Collider2D damageCollider = aoeInstance.GetComponent<Collider2D>();
                if (damageCollider != null)
                {
                    damageCollider.enabled = true;
                }

                yield return new WaitForSeconds(areaAttackDuration);

                Destroy(aoeInstance);
            }
        }
        else
        {
            yield return null;
        }
    }

    private IEnumerator AreaAttackPattern()
    {
        yield return new WaitForSeconds(timeBetweenAreaAttacks / 2f);

        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAreaAttacks);

            if (isPlayerDetected && Player != null)
            {
                Debug.Log("Jefe: Ataque de Área (Independiente)");

                StartCoroutine(PerformAreaAttack());
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}