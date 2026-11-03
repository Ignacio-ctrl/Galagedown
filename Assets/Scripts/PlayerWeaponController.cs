
using UnityEngine;
using static Weapon;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Armas disponibles")]
    [SerializeField] private AbstractWeapon normal;
    [SerializeField] private AbstractWeapon escopeta;
    [SerializeField] private AbstractWeapon circular;
    [SerializeField] private AbstractWeapon escopetaDoble;

    private readonly SimpleArrayDictionary<WeaponType, AbstractWeapon>
        weaponCatalog =
        new SimpleArrayDictionary<WeaponType, AbstractWeapon>();

    private AbstractWeapon currentWeapon;
    private readonly int[] ownedTiers = {1,0,0,0};

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

        weaponCatalog.Add(type, instance);
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
        int index = (int)type;

        if (index < 0 || index >= ownedTiers.Length)
            return false;

        if (ownedTiers[index] == 0)
        {
            Debug.Log("Arma no desbloqueada: " + type);
            return false;
        }

        if (!weaponCatalog.TryGetValue(
            type, out AbstractWeapon weapon) || weapon == null)
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

    public bool CanRecieveWeapon(WeaponType type, int tier)
    {
        int index = (int)type;

        if(index < 0 || index >= ownedTiers.Length)
            return false;

        if(tier <= ownedTiers[index])
            return false;

        return weaponCatalog.TryGetValue(type, out AbstractWeapon weapon) && weapon != null;

    }

    public bool ReceiveWeapon(WeaponType type, int tier, float power)
    {
       if(!CanRecieveWeapon(type, tier) || power < 1f)
            return false;
        
        if (!weaponCatalog.TryGetValue(type, out AbstractWeapon weapon))
        return false;


        weapon.SetWeaponVersion(power);

        ownedTiers[(int)type] = tier;

        EquipWeapon(type);
        Debug.Log(
     "Arma obtenida: " + type +
     " MK-" + tier +
     " | Multiplicador x" + power
        );
        return true;
    }
    public void AddDamageBonus(float amount)
    {
        foreach (AbstractWeapon weapon in weaponCatalog.Values())
            weapon.AddDamageBonus(amount);
    }

    public void AddCadenceBonus(float amount)
    {
        foreach (AbstractWeapon weapon in weaponCatalog.Values())
            weapon.AddCadenceBonus(amount);
    }

}
