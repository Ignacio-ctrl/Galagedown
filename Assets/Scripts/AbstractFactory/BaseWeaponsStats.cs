using UnityEngine;

public class BaseWeaponsStats : IWeaponStats
{
    //estadisticas originales del arma
    public float Damage { get; }

    public float Cooldown { get; }

    public BaseWeaponsStats(float damage, float cooldown)
    {
        Damage = damage;
        Cooldown = cooldown;
    }
}
