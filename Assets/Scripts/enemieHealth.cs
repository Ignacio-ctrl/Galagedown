using UnityEngine;
using UnityEngine.SceneManagement;

public class enemieHealth : MonoBehaviour
{

     [SerializeField] public float EnemieHealth;
    public float checkInterval = 1f;
    private float checkTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckForEnemies();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EnemieHealth > 0)
        {
            if (collision.gameObject.CompareTag("AllyBullet"))
            {
                EnemieHealth -= collision.gameObject.GetComponent<bulletMovement>().damage; 
                Destroy(collision.gameObject); 
                Debug.Log("Enemy hit! Remaining health: " + EnemieHealth); 
                if (EnemieHealth <= 0)
                {
                    Destroy(gameObject); 
                    
                }
            }
        }
    }
    void CheckForEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemie");

        if (enemies.Length == 0)
        {
            win();
        }
    }
    public void win()
    {
        SceneManager.LoadScene(2);
    }
}