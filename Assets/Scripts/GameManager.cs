using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public static GameManager instance; // Singleton instance // Static instance to allow easy access from other scripts
   
    void Awake()
    {
        if(instance == null) //init Game Manager
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
