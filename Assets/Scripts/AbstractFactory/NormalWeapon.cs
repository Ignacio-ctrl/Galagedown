using UnityEngine;

public class NormalWeapon : AbstractWeapon
{
    protected override void Fire()
    {
        Shoot(0f);
    }
}