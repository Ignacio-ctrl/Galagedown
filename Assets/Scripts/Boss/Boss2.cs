
using System.Collections;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
   
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float aimedShotInterval = 1.5f;

   
    [SerializeField] private float tentacleInterval = 3f;

   
    [SerializeField] private Transform firePoint;

    private Transform player;
    private RadialShotWeapon radialWeapon;
    private float tentacleRotation = 0f;

    void Start()
    {
        radialWeapon = GetComponent<RadialShotWeapon>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
            

        if (firePoint == null)
        {
            firePoint = transform;
        }
           

        if (radialWeapon == null)
        {
            Debug.LogError("Boss2 necesita RadialShotWeapon");
        }
           

        StartCoroutine(StartAttacksWithDelay());
    }

    IEnumerator StartAttacksWithDelay()
    {
        yield return new WaitForSeconds(1f);

        InvokeRepeating(nameof(ShootTentacles),0f,tentacleInterval);

        InvokeRepeating(nameof(ShootAtPlayer),1f,aimedShotInterval);
    }

    void ShootTentacles()
    {
        if (radialWeapon == null)
        {
            return;
        }
            

        //el boss usa el patron compartido
        if (radialWeapon.ShootPattern(tentacleRotation))
        {
            //rotacion individual del boss
            tentacleRotation += 15f;
        }
    }

    void ShootAtPlayer()
    {
        if (player == null || bulletPrefab == null)
        {
            return;
        }
            

        Vector2 direction = player.position - firePoint.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.Euler(0, 0, angle));
    }
}
