
using UnityEngine;
using Galagedon.Estructuras;

public class ShopStockTest : MonoBehaviour
{
    [SerializeField, Min(2)]
    private int stockSize = 4;

    private SimpleArrayList<ShopItem> catalog;
    private SimpleArrayList<ShopItem> currentStock;

    private bool shopOpen;

    public ShopItem SelectedReward { get; private set; }
    public bool ShopOpen => shopOpen;

    private void Awake()
    {
        catalog = new SimpleArrayList<ShopItem>();

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
        // DEBUG: simula la siguiente tienda
        // No permite cambiar el stock sin elegir primero.
        if (Input.GetKeyDown(KeyCode.F9) && !shopOpen)
        {
            OpenShop();
        }

        if (!shopOpen)
            return;

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

        currentStock =
            ShopStockGenerator.Generate(catalog, stockSize);

        SelectedReward = null;
        shopOpen = true;

        Debug.Log("===== NUEVA TIENDA =====");

        for (int i = 0; i < currentStock.Count; i++)
        {
            ShopItem item = currentStock[i];

            Debug.Log(
                (i + 1) + ". " + item.displayName +
                " | " + item.category +
                " | " + item.rarity
            );
        }

        Debug.Log("Elegí una recompensa con F1-F4");
    }

    public bool SelectItem(int index)
    {
        if (!shopOpen || currentStock == null)
        {
            Debug.Log("La tienda está cerrada.");
            return false;
        }

        if (index < 0 || index >= currentStock.Count)
        {
            Debug.LogWarning("Opción inválida.");
            return false;
        }

        // Guardamos la recompensa seleccionada
        SelectedReward = currentStock[index];

        // Cerramos la tienda
        shopOpen = false;

        Debug.Log(
            "RECOMPENSA ELEGIDA: " +
            SelectedReward.displayName
        );

        Debug.Log(
            "No podés elegir otra recompensa en esta tienda."
        );

        return true;
    }
}
