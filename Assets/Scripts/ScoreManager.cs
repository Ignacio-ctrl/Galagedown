using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int CurrentScore { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore(int points)
    {
        CurrentScore += points;
        Debug.Log("Puntuación actual: " + CurrentScore);
        // aca agregamos llamada a la funcion que actualiza el texto en pantalla
    }

    private void OnEnable()
    {
        //suscribirse a la muerte de los enemigos
        enemieHealth.OnEnemyKilled += AddScore;
    }

    private void OnDisable()
    {
        enemieHealth.OnEnemyKilled -= AddScore;
    }
}