using UnityEngine;
using UnityEngine.UI;

public class HealthHud : MonoBehaviour
{
    [SerializeField] public Image imageFill;
    private health Health;
    private float MaxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = GameObject.Find("Player").GetComponent<health>();
        MaxHealth = Health.aHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Health.aHealth / MaxHealth; 
    }
}
