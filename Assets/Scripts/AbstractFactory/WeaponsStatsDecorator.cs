using UnityEngine;

public abstract class WeaponsStatsDecorator : IWeaponStats
{

    protected IWeaponStats wrappedStats;

    public WeaponsStatsDecorator(IWeaponStats stats)
    {
        wrappedStats = stats;
    }
    public virtual float Damage => wrappedStats.Damage;

    public virtual float Cooldown => wrappedStats.Cooldown;
}
