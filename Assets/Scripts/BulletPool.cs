using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("BulletPool Instance Missing.");
            }
            return _instance;
        }
    }

    [SerializeField] private BulletBoss bulletPrefab;
    [SerializeField] private int poolSize = 10;

    private List<BulletBoss> bulletPool = new List<BulletBoss>();

    private void Awake()
    {
        //SETUP SINGLETON
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //SETUP POOL
        AddBulletsToPool(poolSize);
    }

    private void AddBulletsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            BulletBoss bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Add(bullet);
            bullet.transform.parent = transform;
        }
    }

    public BulletBoss RequestBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].gameObject.activeSelf)
            {                 
                bulletPool[i].gameObject.SetActive(true);
                return bulletPool[i];
            }
        }
        AddBulletsToPool(1);
        bulletPool[bulletPool.Count - 1].gameObject.SetActive(true);
        return bulletPool[bulletPool.Count - 1];
    }
}
