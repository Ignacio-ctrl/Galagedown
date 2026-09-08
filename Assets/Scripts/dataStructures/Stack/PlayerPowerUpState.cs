
public class PlayerPowerUpState
{
    public bool SpeedBoost;
    public bool DamageBoost;
    public bool Shield;

    public PlayerPowerUpState(bool speedBoost, bool damageBoost, bool hasShield)
    {
        SpeedBoost = speedBoost;
        DamageBoost = damageBoost;
        Shield = hasShield;
    }
}
