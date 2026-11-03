using UnityEngine;

public class Shotgun : AbstractWeapon
{
    [SerializeField] private int pallet = 5;
    [SerializeField] private float spreadAngle = 48f;
    protected override void Fire()
    {
        int count = Mathf.Max(1, pallet);
        float step = count > 1 ? spreadAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = -spreadAngle / 2f + step * i;
            Shoot(angle);
        }
    }

}
