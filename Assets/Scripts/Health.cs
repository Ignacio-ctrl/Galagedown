using UnityEngine;
using UnityEngine.SceneManagement;

public class health : MonoBehaviour
{
    [SerializeField] public float aHealth;

    public void TakeDamage(float damage)
    {
        if (aHealth <= 0) return; 

        aHealth -= damage;
        Debug.Log("Player hit! Remaining health: " + aHealth); 

        if (aHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Die"); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemieBullet"))
        {
            float bulletDamage = collision.gameObject.GetComponent<bulletMovement>().damage;

            Destroy(collision.gameObject);

            TakeDamage(bulletDamage);
        }
    }
}