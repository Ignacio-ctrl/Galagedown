using UnityEngine;

public class DamageDecorator : WeaponsStatsDecorator
{
    private float bonus;

    public DamageDecorator(IWeaponStats stats, float bonus)
        : base(stats)
    {
        this.bonus = bonus;
    }

    public override float Damage
    {
        get
        {
            return wrappedStats.Damage * (1f + bonus);
        }
    }
}
