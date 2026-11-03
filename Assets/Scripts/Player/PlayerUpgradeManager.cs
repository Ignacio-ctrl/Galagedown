using UnityEngine;


public enum UpgradeType
{
   Speed,
   Cadence,
   WaveCharges,
   Damage
}

public class PlayerUpgradeManager : MonoBehaviour
{
    [SerializeField] private ShipMovement ship;
    [SerializeField] private PlayerWeaponController weapons;
    [SerializeField] private expansiveWave wave;

    private readonly int[] shopTiers = new int[4];
    private readonly float[] shopValues = new float [4];


    public bool CanReceiveUpgrade(ShopItem item)
    {
        if (item == null ||
            item.category != ShopCategory.Upgrade)
            return false;

        if (!System.Enum.TryParse<UpgradeType>(
            item.familyId, out UpgradeType type))
            return false;

        int index = (int)type;

        if (index < 0 || index >= shopTiers.Length)
            return false;

        if (item.tier <= shopTiers[index])
            return false;

        if (item.effectValue <= shopValues[index])
            return false;

        if (type == UpgradeType.WaveCharges &&
            Mathf.RoundToInt(item.effectValue) <=
            Mathf.RoundToInt(shopValues[index]))
            return false;

        return true;
    }


    public bool ReceiveShopUpgrade(ShopItem item)
    {
        if (!CanReceiveUpgrade(item))
            return false;

        System.Enum.TryParse<UpgradeType>(
            item.familyId, out UpgradeType type);

        int index = (int)type;

        float difference =
            item.effectValue - shopValues[index];

        switch (type)
        {
            case UpgradeType.Speed:
                if (ship == null) return false;
                ship.AddSpeedBonus(difference);
                break;

            case UpgradeType.Cadence:
                if (weapons == null) return false;
                weapons.AddCadenceBonus(difference);
                break;

            case UpgradeType.Damage:
                if (weapons == null) return false;
                weapons.AddDamageBonus(difference);
                break;

            case UpgradeType.WaveCharges:
                if (wave == null) return false;

                int extraCharges =
                    Mathf.RoundToInt(item.effectValue) -
                    Mathf.RoundToInt(shopValues[index]);

                wave.AddChargeCapacity(extraCharges);
                break;
        }

        shopTiers[index] = item.tier;
        shopValues[index] = item.effectValue;

        Debug.Log(
            "Mejora obtenida: " + item.displayName +
            " | Nivel: " + item.tier
        );

        return true;
    }


}
