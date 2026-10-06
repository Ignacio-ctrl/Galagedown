using UnityEngine;

public class NormalWeapon : AbstractWeapon
{
    public override void Atack()
    {
        Debug.Log("Disparo normal");
    }
}