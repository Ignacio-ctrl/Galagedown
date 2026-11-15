using UnityEngine;

public class AttackCommand : IPlayerCommand
{
    private PlayerWeaponController weapons;

    public AttackCommand(PlayerWeaponController weapons)
    {
        this.weapons = weapons;
    }

    public void Execute()
    {
        if (weapons != null)
            weapons.Attack();
    }
}
