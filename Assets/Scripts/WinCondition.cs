using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemie";
    [SerializeField] private GameObject winScreen;
                                                                                                                                                            

    void Update()
    {
        CheckVictoryCondition();
    }

    void CheckVictoryCondition()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemies.Length == 0)
        {
            WinLevel();
        }
    }
    void WinLevel()
    {
        //Time.timeScale = 0f;
        winScreen.SetActive(true);
        

    }


}
