using Unity.VisualScripting;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 3f;
    private float lifeTime = 0f;

    public Vector2 Velocity;

    private void Update()
    {
        transform.position += (Vector3) Velocity * Time.deltaTime;
        lifeTime += Time.deltaTime;

        if (lifeTime > MAX_LIFE_TIME)
        {
            Disable();
        }
    }
    private void Disable()
    {
        lifeTime = 0f;
        gameObject.SetActive(false);
    }
}
