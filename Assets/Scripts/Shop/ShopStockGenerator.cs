using UnityEngine;
using Galagedon.Estructuras;
using Galagedon.DataStructures.CoreStructures;


public static class ShopStockGenerator
{
  private static readonly int[] rarityWeights = { 55, 28, 13, 4 }; // Common, Rare, Epic, Legendary
    
  public static SimpleArrayList<ShopItem> Generate(ISimpleList<ShopItem> catalog, int stockSize)
  {
        SimpleArrayList<ShopItem> stock = new SimpleArrayList<ShopItem>();

        if(catalog == null || stockSize <= 0)
        {
            return stock;
        }

        if(stockSize >= 2)
        {
            AddIfAvailable(stock, PickItem(catalog, stock, ShopCategory.Weapon));
            AddIfAvailable(stock,PickItem(catalog, stock, ShopCategory.Upgrade));
        }

        while (stock.Count < stockSize)
        {
            ShopItem item = PickItem(catalog, stock, null);

            if (item == null)
            {
                break;
            }
            stock.Add(item);

        }

        for (int i = stock.Count - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);

            ShopItem temp = stock[i];
            stock[i] = stock[j];
            stock[j] = temp;
        }
        return stock;

    }

   private static void AddIfAvailable(SimpleArrayList<ShopItem> stock, ShopItem item)
    {
        if (item != null)
        {
            stock.Add(item);
        }
    }

    private static bool AlreadySelected(ISimpleList<ShopItem> stock, string id)
    {
       for(int i = 0; i < stock.Count; i++)
        {
            if (stock[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsEligible(
        ShopItem item,
        ISimpleList<ShopItem> stock,
        ShopCategory? category)
    {
        if (item == null || string.IsNullOrEmpty(item.id))
            return false;

        if (category.HasValue && item.category != category.Value)
            return false;

        return !AlreadySelected(stock, item.id);
    }
    private static ShopItem PickItem(ISimpleList<ShopItem> catalog, ISimpleList<ShopItem> stock, ShopCategory? category)
    {
        int[] availableByRarity = new int[4];

        for (int i = 0; i < catalog.Count; i++)
        {
            ShopItem item = catalog[i];

            if (IsEligible(item, stock, category))
                availableByRarity[(int)item.rarity]++;
        }

        int totalWeight = 0;

        for (int i = 0; i < rarityWeights.Length; i++)
        {
            if (availableByRarity[i] > 0)
                totalWeight += rarityWeights[i];
        }

        if (totalWeight == 0)
            return null;

        int roll = Random.Range(0, totalWeight);
        int selectedRarity = -1;

        for (int i = 0; i < rarityWeights.Length; i++)
        {
            if (availableByRarity[i] == 0)
                continue;

            if (roll < rarityWeights[i])
            {
                selectedRarity = i;
                break;
            }

            roll -= rarityWeights[i];
        }

        if (selectedRarity < 0)
            return null;

        int randomIndex =
            Random.Range(0, availableByRarity[selectedRarity]);

        for (int i = 0; i < catalog.Count; i++)
        {
            ShopItem item = catalog[i];

            if (!IsEligible(item, stock, category))
                continue;

            if ((int)item.rarity != selectedRarity)
                continue;

            if (randomIndex == 0)
                return item;

            randomIndex--;
        }

        return null; // Fallback in case no item was selected
    }



}

