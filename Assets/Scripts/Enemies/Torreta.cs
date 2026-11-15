
using UnityEngine;

public class Torreta : MonoBehaviour
{
    
    [SerializeField] private float interval = 5f;

    private RadialShotWeapon radialWeapon;

    void Awake()
    {
        radialWeapon = GetComponent<RadialShotWeapon>();
    }

    void Start()
    {
        if (radialWeapon == null)
        {
            
            return;
        }

        InvokeRepeating( nameof(ShootCircularWave), 1f, Mathf.Max(0.1f, interval));
    }

    void ShootCircularWave()
    {
        radialWeapon.ShootPattern();
    }
}
