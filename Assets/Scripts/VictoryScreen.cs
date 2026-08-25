using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{

    // Array con los nombres de TODOS tus niveles en orden
    [SerializeField]
    private string[] levelNames = {
        "Level1",
        "Level2",
      
        // Agrega más niveles aquí según necesites
    };

    private int currentLevelIndex;

    void Start()
    {
        // Obtener el nivel que acabamos de completar
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);

        Debug.Log($"Nivel actual guardado: {currentLevelIndex}");

    }
    private void Update()
    {
        GoToNextLevel();
    }

    void GoToNextLevel()
    {
        // Calcular índice del siguiente nivel
        int nextLevelIndex = currentLevelIndex + 1;



        // Verificar que el siguiente nivel existe
        if (nextLevelIndex < levelNames.Length)
        {
            // Cargar la escena del siguiente nivel
            SceneManager.LoadScene(levelNames[nextLevelIndex]);
        }
  
    }
    public string[] GetLevelNames()
    {
        
        return new string[] { "Level1", "Level2", };
    }
}