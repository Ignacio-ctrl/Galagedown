using UnityEngine;

public class PreviousWeaponCommand : IPlayerCommand
{
  private readonly PlayerWeaponController weapons;

    public PreviousWeaponCommand(PlayerWeaponController weapons)
    {
        this.weapons = weapons;
    }

    public void Execute()
    {

        if (weapons != null)
        {
            weapons.EquipPreviousWeapon();
        }
    }

}
