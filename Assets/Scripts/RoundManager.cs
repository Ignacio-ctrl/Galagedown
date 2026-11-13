using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class RoundManager : MonoBehaviour
{

    [SerializeField] private EnemySpawnerManager[] spawners;
    [SerializeField] private GameObject roundPanel;
    [SerializeField] private TMP_Text roundText;

    [SerializeField]
    private string enemyTag = "Enemie";

    private int currentRound = 1;
    private bool roundInProgress = false;

    void Start()
    {
        Time.timeScale = 1f;

        roundPanel.SetActive(false);

        
    }

    
    void Update()
    {
        
    }

    void StartRound()
    {
        //cargamos los enemigos de la ronda actual
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i].RestarSpawner();
        }

        roundInProgress = true;


    }

    void CheckRound()
    {
        //espera que los spawner terminen
        for(int i = 0; i < spawners.Length; i++)
        {
            if(!spawners[i].FinishedSpawning)
            {
                return;
            }
        }

        //identifica los enemigos vivos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if(enemies.Length == 0)
        {
         FinishRound();
        }
    }

    void FinishRound() { 
    
        roundInProgress = false;

        //pantalla de ronda terminada
        roundText.text = "Round " + currentRound + " Finished!";

        roundPanel.SetActive(true);
        Time.timeScale = 0f;


    }

    public void NextRound() {

        if (roundInProgress)
            return;

        currentRound++;

        roundPanel.SetActive(false);
        Time.timeScale = 1f;

        StartRound();

    }
}
