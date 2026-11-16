
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
    private WeaponType currentWeaponType;
    private readonly SimpleArrayStack<WeaponType> weaponHistory =new SimpleArrayStack<WeaponType>(5);
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
        return EquipWeaponInternal(type, true);
    }

    private bool EquipWeaponInternal(WeaponType type, bool saveHistory)
    {
        int index = (int)type;

        if (index < 0 || index >= ownedTiers.Length)
        {
            return false;
        }
           

        if (ownedTiers[index] == 0)
        {
            return false;
        }

        if (!weaponCatalog.TryGetValue(type, out AbstractWeapon weapon) || weapon == null)
        {
            return false;
        }
            

        // Si ya tenemos esta arma, no guardamos nada
        if (currentWeapon == weapon)
            return true;

        // Guardamos la anterior solamente cuando corresponde
        if (saveHistory && currentWeapon != null)
        {
            weaponHistory.PushDiscardOldest(currentWeaponType);
        }

        currentWeapon = weapon;
        currentWeaponType = type;

        
        return true;
    }


    public bool EquipPreviousWeapon()
    {
        while (!weaponHistory.IsEmpty())
        {
            WeaponType previousWeapon = weaponHistory.Pop();

            if (previousWeapon == currentWeaponType)
            {
                continue;
            }
               

            if (EquipWeaponInternal(previousWeapon, false))
            {
                return true;
            }
        }

        return false;
    }


    public void Attack()
    {
        if (Time.timeScale == 0f || currentWeapon == null)
            return;

        currentWeapon.Atack();
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
