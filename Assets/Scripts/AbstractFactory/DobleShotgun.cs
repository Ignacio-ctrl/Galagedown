using UnityEngine;

public class DobleShotgun : AbstractWeapon
{
    [SerializeField] private int pallletsPerBarrel = 5;
    [SerializeField] private float spreadAngle = 48f;
    [SerializeField] private float barrelOffset = 0.25f;

    protected override void Fire()
    {
        int count = Mathf.Max(1, pallletsPerBarrel);
        float step = count > 1 ? spreadAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = -spreadAngle / 2f + step * i;
            Shoot(angle, -barrelOffset); // Left barrel
            Shoot(angle, barrelOffset);  // Right barrel
        }
    }
}
