using UnityEngine;
using UnityEngine.Pool;

// Patron SINGLETON + patron POOL.
// Un unico BulletPool administra un pool por cada prefab de proyectil.
// Si no existe en la escena, se crea solo la primera vez que alguien lo pide.
public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;

    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("BulletPool");
                _instance = go.AddComponent<BulletPool>();
            }
            return _instance;
        }
    }

    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 300;

    // TDA Dictionary propio: prefab -> pool de ese prefab
    private readonly SimpleArrayDictionary<GameObject, ObjectPool<GameObject>> pools =
        new SimpleArrayDictionary<GameObject, ObjectPool<GameObject>>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // Pide un proyectil: lo toma del pool (o lo crea si no hay libres)
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
            return null;

        ObjectPool<GameObject> pool = GetOrCreatePool(prefab);

        GameObject obj = pool.Get();

        // por si alguien destruyo un objeto del pool desde afuera
        int attempts = 0;
        while (obj == null && attempts < 5)
        {
            obj = pool.Get();
            attempts++;
        }
        if (obj == null)
            return null;

        // primero se posiciona y recien despues se activa
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    // Devuelve un proyectil al pool (si no es del pool, lo destruye)
    public void Release(GameObject obj)
    {
        if (obj == null || !obj.activeSelf)
            return;

        PooledProjectile marker = obj.GetComponent<PooledProjectile>();

        if (marker != null && marker.Prefab != null &&
            pools.TryGetValue(marker.Prefab, out ObjectPool<GameObject> pool))
        {
            pool.Release(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    // Atajo estatico para que las balas se devuelvan solas
    public static void Return(GameObject obj)
    {
        if (obj == null)
            return;

        if (_instance != null)
            _instance.Release(obj);
        else
            Destroy(obj);
    }

    private ObjectPool<GameObject> GetOrCreatePool(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out ObjectPool<GameObject> existing))
            return existing;

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            () => CreatePoolItem(prefab),
            null,
            OnReturnedToPool,
            OnDestroyPoolObject,
            false,
            defaultCapacity,
            maxSize);

        pools.Add(prefab, pool);
        return pool;
    }

    private GameObject CreatePoolItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);

        PooledProjectile marker = obj.GetComponent<PooledProjectile>();
        if (marker == null)
            marker = obj.AddComponent<PooledProjectile>();
        marker.Prefab = prefab;

        obj.SetActive(false);
        return obj;
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
}
