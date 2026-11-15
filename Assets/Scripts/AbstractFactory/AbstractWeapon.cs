
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
   
    [SerializeField] protected int bulletSpeed = 10;
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float cooldown = 0.25f;

    //Bonus acumulados de la tienda
    private float damageBonus = 0f;
    private float cadenceBonus = 0f;

    //Version actual del arma
    private float versionDamageMultiplier = 1f;

    //Cadena de decoradores
    private IWeaponStats currentStats;

    private Transform firePoint;
    private GameObject bulletPrefab;
    private float nextFireTime = 0f;

    private void Awake()
    {
        RebuildStats();
    }

    public void Initialize(Transform point, GameObject prefab)
    {
        firePoint = point;
        bulletPrefab = prefab;
    }

    public void Atack()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
           

        if (Time.time < nextFireTime)
        {
            return;
        }
            

        if (firePoint == null || bulletPrefab == null)
        {
            return;
        }
            

        Fire();

        //Cooldown calculado por los decoradores
        nextFireTime = Time.time + currentStats.Cooldown;
    }

    protected abstract void Fire();

    protected void Shoot(float angle, float sideOffset = 0f)
    {
        Vector3 position =
            firePoint.position + firePoint.right * sideOffset;

        Quaternion rotation =
            firePoint.rotation * Quaternion.Euler(0, 0, angle);

        GameObject projectile =
            Instantiate(bulletPrefab, position, rotation);

        bulletMovement bullet =
            projectile.GetComponent<bulletMovement>();

        if (bullet != null)
        {
            //Daño calculado por los decoradores
            bullet.Configure(bulletSpeed, currentStats.Damage);
        }
    }

    //Cuando recibimos una nueva version de arma
    public void SetWeaponVersion(float multiplier)
    {
        versionDamageMultiplier = Mathf.Max(1f, multiplier);
        RebuildStats();
    }

    //Mejora de daño de la tienda
    public void AddDamageBonus(float bonus)
    {
        if (bonus <= 0f)
        {
            return;
        }
            

        damageBonus += bonus;
        RebuildStats();
    }

    //Mejora de cadencia de la tienda
    public void AddCadenceBonus(float bonus)
    {
        if (bonus <= 0f)
        {
            return;
        }
           

        cadenceBonus += bonus;
        RebuildStats();
    }

    //Reconstruimos la cadena de decoradores
    private void RebuildStats()
    {
        //Estadisticas originales
        IWeaponStats stats = new BaseWeaponsStats( damage, Mathf.Max(0.05f, cooldown));

        //Multiplicador de version del arma
        if (versionDamageMultiplier > 1f)
        {
            stats = new DamageDecorator( stats, versionDamageMultiplier - 1f);
        }

        //Mejoras de daño
        if (damageBonus > 0f)
        {
            stats = new DamageDecorator( stats, damageBonus);
        }

        //Mejoras de cadencia
        if (cadenceBonus > 0f)
        {
            stats = new CadenceDecorator( stats, cadenceBonus);
        }

        currentStats = stats;
    }
}
