using UnityEngine;

public class RewardFacade : MonoBehaviour
{
    [SerializeField] private ShopStockTest shop;

    public bool HasShop => shop != null;


    //abre la tienda
    public void OpenShop()
    {
        if(shop == null)
        {
            return;
        }

        shop.OpenShop();
    }

    //recompensas por ganar elite
    public ShopItem CompleteEliteRound()
    {
        return GiveSpecialReward(300);
    }

    //recompensas por ganar un boss
    public ShopItem CompleteBossRound()
    {
        return GiveSpecialReward(800);
    }

    //central de dar puntos y recompensas
    private ShopItem GiveSpecialReward(int points)
    {
        ShopItem reward = null;

        if(shop != null)
        {
            reward = shop.GiveEliteReward();
        }

        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(points);
        }

        return reward;
    }

}
