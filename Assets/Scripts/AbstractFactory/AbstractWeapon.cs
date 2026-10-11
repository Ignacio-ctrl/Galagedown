using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField]protected float cooldown; 
    [SerializeField]protected int bulletSpeed = 10;
    [SerializeField]protected float damage = 1f;

    private Transform firePoint;
    private GameObject bulletPrefab;
    private float nextFireTime = 0f;

    public void Initialize(Transform point, GameObject prefab)
    {
        firePoint = point;
        bulletPrefab = prefab;
    }

    public void Atack()
    {
        if (Time.time < nextFireTime)
            return;
        if(firePoint == null || bulletPrefab == null)
            return;
        Fire();
        nextFireTime = Time.time + cooldown;

    }
    protected abstract void Fire();

    protected void Shoot(float angle, float sideOffset = 0f)
    {
        Vector3 position = firePoint.position + firePoint.right * sideOffset;

        Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);

        GameObject projectile = Instantiate(bulletPrefab, position, rotation);

        bulletMovement bullet = projectile.GetComponent<bulletMovement>();

        if (bullet != null)
        {
            bullet.Configure(bulletSpeed, damage);
            
        }



    }
}
