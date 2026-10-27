using UnityEngine;
using UnityEngine.SceneManagement;

public class health : MonoBehaviour
{
    [SerializeField] public float aHealth;

    public int CurrentScore { get; private set; }
    public void TakeDamage(float damage)
    {
        if (aHealth <= 0) return; 

        aHealth -= damage;
        Debug.Log("Player hit! Remaining health: " + aHealth); 

        if (aHealth <= 0)
        {
            Destroy(gameObject);
            PlayerPrefs.SetInt("PuntajeFinal", ScoreManager.Instance.CurrentScore);
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

    public void EjecutarGameOver()
    {
        // esto es para que se vea la pantalla de game over y no se mueva nada
        Time.timeScale = 0f;

        // para esta wea pasamos un nombre generico o player
        // en un juego final esto vendría de un input de texto inicial

        ScoreboardManager.Instance.MostrarScoreboard();
    }
}