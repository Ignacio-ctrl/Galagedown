using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scenesMan : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject PanelLvlSelect;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject optionPan;
    [SerializeField] private GameObject optionvideo;
    [SerializeField] private GameObject optionaudio;
    [SerializeField] private GameObject optioncontrols;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string[] levelScenes; // Nombres de las escenas de niveles

    private int currentLevelIndex;
    void Start()
    {
        // Obtener el nivel actual desde PlayerPrefs
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void muerte()
    {
        SceneManager.LoadScene("Die");
    }
    public void exit()
    {
        Application.Quit();
    }
    public void menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
 
    public void resume()
    {
        Time.timeScale = 1.0f;
        Panel.SetActive(false);
    }
    public void actLvlSelect()
    {
        PanelLvlSelect.SetActive(true);
        optionPan.SetActive(false);
    }

    public void lvlboss()
    {
        SceneManager.LoadScene("Level5");
        Time.timeScale = 1.0f;
    }
    public void nextLvl()
    {
      
        Time.timeScale = 1.0f;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        
        SceneManager.LoadScene(nextScene);
        winScreen.SetActive(false);
        
       
    }
    public void restart()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        
        SceneManager.LoadScene(currentScene);
        Time.timeScale = 1.0f;  
    }
    public void level1()
    {
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1.0f;
    }
    public void level2()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1.0f;
    }
    public void level3()
    {
        SceneManager.LoadScene("Level3");
        Time.timeScale = 1.0f;
    }
    public void level4()
    {
        SceneManager.LoadScene("Level4");
        Time.timeScale = 1.0f;
    }
    public void level5()
    {
        SceneManager.LoadScene("Level5");
        Time.timeScale = 1.0f;
    }
    public void level6()
    {
        SceneManager.LoadScene("Level6");
        Time.timeScale = 1.0f;
    }   
    public void level7()
    {
        SceneManager.LoadScene("Level7");
        Time.timeScale = 1.0f;
    }
    public void level8()
    {
        SceneManager.LoadScene("Level8");
        Time.timeScale = 1.0f;
    }
    public void level9()
    {
        SceneManager.LoadScene("Level9");
        Time.timeScale = 1.0f;
    }
    public void level10()
    {
        SceneManager.LoadScene("Level10");
        Time.timeScale = 1.0f;
    }
    public void level11()
    {
        SceneManager.LoadScene("Level11");
        Time.timeScale = 1.0f;
    }
    public void level12()
    {
        SceneManager.LoadScene("Level12");
        Time.timeScale = 1.0f;
    }
    public void level13()
    {
        SceneManager.LoadScene("Level13");
        Time.timeScale = 1.0f;
    }
    public void level14()
    {
        SceneManager.LoadScene("Level14");
        Time.timeScale = 1.0f;
    }
    public void level15()
    {
        SceneManager.LoadScene("Level15");
        Time.timeScale = 1.0f;
    }

   

    // TOGGLE (Alternar) - La función principal
    public void AlternarPantallaCompleta()
    {
        // Cambiar entre pantalla completa y ventana
        Screen.fullScreen = !Screen.fullScreen;

        // Guardar preferencia del jugador
        PlayerPrefs.SetInt("PantallaCompleta", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.Save();

       
    }

    //  ACTIVAR pantalla completa
    public void ActivarPantallaCompleta()
    {
        Screen.fullScreen = true;
        PlayerPrefs.SetInt("PantallaCompleta", 1);
        PlayerPrefs.Save();
        Debug.Log("Pantalla completa ACTIVADA");
    }

    //  DESACTIVAR pantalla completa (modo ventana)
    public void DesactivarPantallaCompleta()
    {
        Screen.fullScreen = false;
        PlayerPrefs.SetInt("PantallaCompleta", 0);
        PlayerPrefs.Save();
        Debug.Log("Pantalla completa DESACTIVADA (Modo ventana)");
    }

    

    //  Cargar configuración guardada
    public void CargarConfiguracionGuardada()
    {
        int configGuardada = PlayerPrefs.GetInt("PantallaCompleta", 1);
        Screen.fullScreen = (configGuardada == 1);
        Debug.Log("Configuración cargada: " + (Screen.fullScreen ? "Completa" : "Ventana"));
    }

    //  Restaurar configuración por defecto
    public void RestaurarConfiguracionPorDefecto()
    {
        // Por defecto: pantalla completa activada
        Screen.fullScreen = true;
        PlayerPrefs.SetInt("PantallaCompleta", 1);
        PlayerPrefs.Save();
        Debug.Log("Configuración restaurada a valores por defecto");
    }

   

    //  Verificar estado actual
    public bool EstaEnPantallaCompleta()
    {
        return Screen.fullScreen;
    }

    //  Obtener estado como texto
    public string ObtenerEstadoTexto()
    {
        return Screen.fullScreen ? "Pantalla Completa" : "Modo Ventana";
    }

    public void Volver()
    {
        // Cargar la escena anterior guardada
        int escenaAnterior = PlayerPrefs.GetInt("EscenaAnterior", 0);
        SceneManager.LoadScene(escenaAnterior);
    }
    public void optciones()
    {
        optionPan.SetActive(true);
        PanelLvlSelect.SetActive(false);
    }
    public void videoOpt()
    {
        optionvideo.SetActive(true);
        optionaudio.SetActive(false);
        optioncontrols.SetActive(false);
    }
    public void audioOpt()
    {
        optionaudio.SetActive(true);
        optionvideo.SetActive(false);
        optioncontrols.SetActive(false);
    }
    public void controlsOpt()
    {
        optioncontrols.SetActive(true);
        optionaudio.SetActive(false);
        optionvideo.SetActive(false);
    }



}
