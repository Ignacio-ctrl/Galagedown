using UnityEngine;

public class bulletMovement : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] public float damage = 1f;
    [SerializeField] private float lifeTime = 4f;

    // valores originales del prefab (la bala se reutiliza desde el pool)
    private float defaultSpeed;
    private float defaultDamage;
    private bool defaultsStored;
    private float timer;

    private void StoreDefaults()
    {
        if (defaultsStored)
            return;

        defaultSpeed = Speed;
        defaultDamage = damage;
        defaultsStored = true;
    }

    void Awake()
    {
        StoreDefaults();
    }

    // cada vez que sale del pool vuelve a su estado original
    void OnEnable()
    {
        StoreDefaults();
        Speed = defaultSpeed;
        damage = defaultDamage;
        timer = 0f;
    }

    void Update()
    {
        transform.Translate(transform.up * Speed * Time.deltaTime, Space.World);

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            BulletPool.Return(gameObject);
        }
    }

    public void Configure(float newSpeed, float newDamage)
    {
        Speed = newSpeed;
        damage = newDamage;
    }
}