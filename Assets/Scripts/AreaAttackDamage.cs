using UnityEngine;

public class AreaAttackDamage : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            health playerHealth = collision.gameObject.GetComponent<health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}