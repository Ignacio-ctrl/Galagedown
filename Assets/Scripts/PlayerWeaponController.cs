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

    private void Update()
    {
        
    }

    private void RegisterWeapon(WeaponType type, AbstractWeapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("Falta asignar el arma: " + type, this);
            return;
        }

        weaponCatalog.add(type, weapon);
    }

    public bool EquipWeapon(WeaponType type)
    {
        if (!weaponCatalog.TryGetValue(type, out AbstractWeapon weapon)
            || weapon == null)
        {
            Debug.LogWarning("Arma no disponible: " + type, this);
            return false;
        }

        currentWeapon = weapon;
        return true;
    }

    public void Attack()
    {
      
    
        if (Time.timeScale == 0f)
            return;

        if (currentWeapon == null)
        {
            Debug.LogWarning("No hay un arma equipada", this);
            return;
        }

        currentWeapon.Atack();
    
    }
}