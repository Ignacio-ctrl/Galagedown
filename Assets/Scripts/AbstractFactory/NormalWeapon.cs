using UnityEngine;

public class NormalWeapon : MonoBehaviour, IWeapon
{
<<<<<<< Updated upstream
    protected override void Fire()
    {
        Shoot(0f);
=======
    public string WeaponName => "NormalWeapon";

    public void Attack()
    {
        Debug.Log("¡Disparo normal pew pew!");
    }

    public void SetWeaponActive(bool isActive)
    {
        gameObject.SetActive(isActive);
>>>>>>> Stashed changes
    }
}