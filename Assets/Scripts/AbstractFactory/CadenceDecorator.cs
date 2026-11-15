using UnityEngine;

public class CadenceDecorator : WeaponsStatsDecorator
{
    private float bonus;

    public CadenceDecorator(IWeaponStats stats, float bonus)
        : base(stats)
    {
        this.bonus = bonus;
    }

    public override float Cooldown
    {
        get
        {
            return wrappedStats.Cooldown / (1f + bonus);
        }
    }
}
