using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    GameOver
}

// Patron SINGLETON: administra el ciclo general de la partida
// (estado, fin de juego y puntaje final).
// El puntaje "en vivo" sigue siendo responsabilidad de ScoreManager.
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            // Si la escena no tiene uno, se crea solo
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private string gameOverScene = "Die";

    public GameState State { get; private set; } = GameState.Playing;
    public int FinalScore { get; private set; }
    public bool IsGameOver => State == GameState.GameOver;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre escenas
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Al entrar a cualquier escena que no sea la de game over empieza una partida nueva
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != gameOverScene)
        {
            State = GameState.Playing;
        }
    }

    // Fin de la partida: guarda el puntaje final y carga la pantalla de derrota
    public void GameOver()
    {
        // evita que se ejecute dos veces (por ejemplo dos balas el mismo frame)
        if (IsGameOver)
            return;

        State = GameState.GameOver;

        FinalScore = ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore: 0;

        // Lo siguen leyendo ScoreboardManager y scenesMan
        PlayerPrefs.SetInt("PuntajeFinal", FinalScore);

        SceneManager.LoadScene(gameOverScene);
    }
}
