using UnityEngine;

public class Circular : MonoBehaviour, IWeapon
{
<<<<<<< Updated upstream
    [SerializeField] private int bulletCount = 8;
    protected override void Fire()
    {
        int count = Mathf.Max(1, bulletCount);
        float step = count > 1 ? 360f / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i;
            Shoot(angle);
        }
=======
    public string WeaponName => "Circular";

    public void Attack()
    {
        Debug.Log("¡Disparo CIRCULAR pew pew!");
>>>>>>> Stashed changes
    }

    public void SetWeaponActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}