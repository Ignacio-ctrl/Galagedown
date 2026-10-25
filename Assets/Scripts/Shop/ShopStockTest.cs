
using UnityEngine;
using Galagedon.Estructuras;
using System;
using static Weapon;
using System.Runtime.CompilerServices;

public class ShopStockTest : MonoBehaviour
{

    private enum SortCriterion
    {
        Rarity,
        Name,
        Tier
    }

    private SortCriterion currentSort = SortCriterion.Rarity;
    private bool ascending = false;

    [SerializeField, Min(2)]
    private int stockSize = 4;

    private SimpleArrayList<ShopItem> catalog;
    private SimpleArrayList<ShopItem> currentStock;

    private bool shopOpen;

    [SerializeField] private PlayerWeaponController weapons;
    [SerializeField] private PlayerUpgradeManager upgradeManager;

    public ShopItem SelectedReward { get; private set; }
    public bool ShopOpen => shopOpen;

    private void Awake()
    {
        catalog = new SimpleArrayList<ShopItem>(10); // Initial capacity

        // ARMAS
        Add("normal_2", "Normal MK-II",
            ShopCategory.Weapon, ShopRarity.Rare,
            "Normal", 2, 1.25f);

        Add("normal_3", "Normal MK-III",
            ShopCategory.Weapon, ShopRarity.Epic,
            "Normal", 3, 1.50f);

        Add("shotgun_2", "Escopeta MK-II",
            ShopCategory.Weapon, ShopRarity.Rare,
            "Escopeta", 2, 1.25f);

        Add("shotgun_4", "Escopeta MK-IV",
            ShopCategory.Weapon, ShopRarity.Legendary,
            "Escopeta", 4, 1.80f);

        Add("circular_3", "Circular MK-III",
            ShopCategory.Weapon, ShopRarity.Epic,
            "Circular", 3, 1.50f);

        Add("double_4", "Escopeta Doble MK-IV",
            ShopCategory.Weapon, ShopRarity.Legendary,
            "EscopetaDoble", 4, 1.80f);

        // MEJORAS
        Add("speed_1", "Propulsor Reforzado",
            ShopCategory.Upgrade, ShopRarity.Common,
            "Speed", 1, 0.10f);

        Add("speed_2", "Propulsor Avanzado",
            ShopCategory.Upgrade, ShopRarity.Rare,
            "Speed", 2, 0.20f);

        Add("cadence_1", "Recarga Mejorada",
            ShopCategory.Upgrade, ShopRarity.Common,
            "Cadence", 1, 0.10f);

        Add("cadence_3", "Recarga Experimental",
            ShopCategory.Upgrade, ShopRarity.Epic,
            "Cadence", 3, 0.25f);

        Add("wave_2", "Bateria Electromagnetica",
            ShopCategory.Upgrade, ShopRarity.Rare,
            "WaveCharges", 2, 1f);

        Add("damage_3", "Amplificador de Dano",
            ShopCategory.Upgrade, ShopRarity.Epic,
            "Damage", 3, 0.30f);

        Add("damage_4", "Amplificador Supremo",
            ShopCategory.Upgrade, ShopRarity.Legendary,
            "Damage", 4, 0.50f);
    }

    private void Add(
        string id,
        string name,
        ShopCategory category,
        ShopRarity rarity,
        string familyId,
        int tier,
        float effect)
    {
        catalog.Add(new ShopItem(
            id, name, category, rarity,
            familyId, tier, effect
        ));
    }

    private void Start()
    {
        OpenShop();
    }


    private void Update()
    {
        // Nueva tienda de prueba
        if (Input.GetKeyDown(KeyCode.F9) && !shopOpen)
            OpenShop();

        if (!shopOpen)
            return;

        // SORTING
        if (Input.GetKeyDown(KeyCode.F5))
            SortStock(SortCriterion.Rarity);

        if (Input.GetKeyDown(KeyCode.F6))
            SortStock(SortCriterion.Name);

        if (Input.GetKeyDown(KeyCode.F7))
            SortStock(SortCriterion.Tier);

        if (Input.GetKeyDown(KeyCode.F8))
        {
            ascending = !ascending;
            SortStock(currentSort);
        }

        // SELECCION
        if (Input.GetKeyDown(KeyCode.F1))
            SelectItem(0);

        else if (Input.GetKeyDown(KeyCode.F2))
            SelectItem(1);

        else if (Input.GetKeyDown(KeyCode.F3))
            SelectItem(2);

        else if (Input.GetKeyDown(KeyCode.F4))
            SelectItem(3);
    }


    public void OpenShop()
    {
        if (shopOpen)
            return;

        SimpleArrayList<ShopItem> available =
            GetAvailableCatalog();

        currentStock =
            ShopStockGenerator.Generate(available, stockSize);

        SelectedReward = null;

        if (currentStock.Count == 0)
        {
            Debug.LogWarning(
                "No quedan recompensas disponibles."
            );

            shopOpen = false;
            return;
        }

        shopOpen = true;

        Debug.Log("===== NUEVA TIENDA =====");
        PrintStock();
    }


    public bool SelectItem(int index)
    {
        if (!shopOpen || currentStock == null)
            return false;

        if (index < 0 || index >= currentStock.Count)
            return false;

        ShopItem item = currentStock[index];

        if (!GrantReward(item))
        {
            Debug.LogWarning(
                "No se pudo otorgar: " + item.displayName
            );

            return false;
        }

        SelectedReward = item;
        shopOpen = false;

        Debug.Log(
            "RECOMPENSA ELEGIDA: " + item.displayName
        );

        Debug.Log("TIENDA CERRADA");

        return true;
    }



    private bool GrantReward(ShopItem item)
    {
        if (item == null)
            return false;

        if (item.category == ShopCategory.Weapon)
        {
            if (weapons == null)
                return false;

            if (!Enum.TryParse<WeaponType>(
                item.familyId, out WeaponType type))
                return false;

            return weapons.ReceiveWeapon(
                type,
                item.tier,
                item.effectValue
            );
        }

        if (item.category == ShopCategory.Upgrade)
        {
            if (upgradeManager == null)
                return false;

            return upgradeManager.ReceiveShopUpgrade(item);
        }

        return false;
    }


    private bool IsAvailable(ShopItem item)
    {
        if (item == null)
            return false;

        if (item.category == ShopCategory.Weapon)
        {
            if (weapons == null || item.effectValue < 1f)
                return false;

            if (!Enum.TryParse<WeaponType>(
                item.familyId, out WeaponType type))
                return false;

            return weapons.CanRecieveWeapon(type, item.tier);
        }

        if (item.category == ShopCategory.Upgrade)
        {
            return upgradeManager != null &&
                   upgradeManager.CanReceiveUpgrade(item);
        }

        return false;
    }

    private SimpleArrayList<ShopItem> GetAvailableCatalog()
    {
        SimpleArrayList<ShopItem> available =
            new SimpleArrayList<ShopItem>(catalog.Count);

        for (int i = 0; i < catalog.Count; i++)
        {
            if (IsAvailable(catalog[i]))
            {
                available.Add(catalog[i]);
            }
        }

        return available;
    }


    private void SortStock(SortCriterion criterion)
    {
        if (!shopOpen || currentStock == null)
            return;

        currentSort = criterion;

        Comparison<ShopItem> comparison;

        switch (criterion)
        {
            case SortCriterion.Rarity:
                comparison = (a, b) =>
                    a.rarity.CompareTo(b.rarity);
                break;

            case SortCriterion.Name:
                comparison = (a, b) =>
                    string.Compare(
                        a.displayName,
                        b.displayName,
                        StringComparison.OrdinalIgnoreCase
                    );
                break;

            case SortCriterion.Tier:
                comparison = (a, b) =>
                    a.tier.CompareTo(b.tier);
                break;

            default:
                return;
        }

        CollectionSort.SortList(
            currentStock,
            comparison,
            ascending
        );

        Debug.Log(
            "Ordenado por: " + criterion +
            " | Ascendente: " + ascending
        );

        PrintStock();
    }


    private void PrintStock()
    {
        Debug.Log("===== STOCK ACTUAL =====");

        for (int i = 0; i < currentStock.Count; i++)
        {
            ShopItem item = currentStock[i];

            Debug.Log(
                "F" + (i + 1) + " - " +
                item.displayName +
                " | " + item.category +
                " | " + item.rarity +
                " | Nivel " + item.tier
            );
        }
    }


}
