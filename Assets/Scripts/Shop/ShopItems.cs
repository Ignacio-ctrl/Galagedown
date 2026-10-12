using UnityEngine;
public enum ShopCategory
{
    Weapon,
    Upgrade,
    
}
public enum ShopRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class ShopItem
{
    public string id;
    public string displayName;
    public ShopCategory category;
    public ShopRarity rarity;
    

    public string familyId; //Familia de arma o mejora
    public int tier; //Version o nivel
    public float effectValue; // Potencia de la version

    public ShopItem(string id, string displayName, ShopCategory category, ShopRarity rarity, string familyId, int tier, float effectValue)
    {
        this.id = id;
        this.displayName = displayName;
        this.category = category;
        this.rarity = rarity;
        this.familyId = familyId;
        this.tier = tier;
        this.effectValue = effectValue;
    }
  
}
