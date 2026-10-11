
using UnityEngine;
using static Weapon;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Armas disponibles")]
    [SerializeField] private AbstractWeapon normal;
    [SerializeField] private AbstractWeapon escopeta;
    [SerializeField] private AbstractWeapon circular;
    [SerializeField] private AbstractWeapon escopetaDoble;

    private readonly SImpleArrayDictionary<WeaponType, AbstractWeapon>
        weaponCatalog =
        new SImpleArrayDictionary<WeaponType, AbstractWeapon>();

    private AbstractWeapon currentWeapon;

    public AbstractWeapon CurrentWeapon => currentWeapon;

    private void Awake()
    {
        RegisterWeapon(WeaponType.Normal, normal);
        RegisterWeapon(WeaponType.Escopeta, escopeta);
        RegisterWeapon(WeaponType.Circular, circular);
        RegisterWeapon(WeaponType.EscopetaDoble, escopetaDoble);

        EquipWeapon(WeaponType.Normal);
    }

    private void RegisterWeapon(WeaponType type, AbstractWeapon prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Falta asignar: " + type);
            return;
        }

        AbstractWeapon instance = Instantiate(prefab, transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;

        weaponCatalog.add(type, instance);
    }

    public void Initialize(Transform firePoint, GameObject bulletPrefab)
    {
        foreach (AbstractWeapon weapon in weaponCatalog.Values())
        {
            weapon.Initialize(firePoint, bulletPrefab);
        }
    }

    public bool EquipWeapon(WeaponType type)
    {
        if (!weaponCatalog.TryGetValue(type, out AbstractWeapon weapon)
            || weapon == null)
            return false;

        currentWeapon = weapon;
        Debug.Log("Arma equipada: " + type);
        return true;
    }

    public void Attack()
    {
        if (Time.timeScale == 0f || currentWeapon == null)
            return;

        currentWeapon.Atack();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipWeapon(WeaponType.Normal);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipWeapon(WeaponType.Escopeta);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            EquipWeapon(WeaponType.Circular);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            EquipWeapon(WeaponType.EscopetaDoble);
    }
}
