using UnityEngine;

public class ChangeWeaponCommand : IPlayerCommand
{
    private PlayerWeaponController weapons;
    private Weapon.WeaponType weaponType;

    public ChangeWeaponCommand(
        PlayerWeaponController weapons,
        Weapon.WeaponType weaponType)
    {
        this.weapons = weapons;
        this.weaponType = weaponType;
    }

    public void Execute()
    {
        if (weapons != null)
            weapons.EquipWeapon(weaponType);
    }
}