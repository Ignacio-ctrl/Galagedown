using UnityEngine;
using UnityEngine.SceneManagement;

public class escenaActu : MonoBehaviour
{
    void Start()
    {
        // Guardar la escena actual como "anterior" para la próxima
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("EscenaAnterior", escenaActual);
        PlayerPrefs.Save();
    }
}
