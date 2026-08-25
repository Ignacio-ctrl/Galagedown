using UnityEngine;

public class bulletMovement : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, 4f);
    }

     void Update()
    {

        transform.Translate(transform.up * Speed * Time.deltaTime, Space.World);

        // transform.position += transform.up * Speed * Time.deltaTime;
    }
}