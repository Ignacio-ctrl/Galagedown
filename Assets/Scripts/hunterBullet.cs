using UnityEngine;

public class hunterBullet : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 4f;
    [SerializeField] private float fuerzaPersecucion = 2f;
    [SerializeField] private float tiempoVida = 5f;

    private Transform objetivo;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        if (objetivo != null)
        {
            PerseguirObjetivo();
        }
        else
        {
            SeguirAdelante();
        }
    }

    void PerseguirObjetivo()
    {
        // Calcular dirección hacia el objetivo
        Vector2 direccion = (objetivo.position - transform.position).normalized;

        // Suavizar rotación hacia el objetivo
        Vector2 direccionActual = transform.up;
        Vector2 nuevaDireccion = Vector2.Lerp(direccionActual, direccion, fuerzaPersecucion * Time.deltaTime);

        // Rotar
        float angulo = Mathf.Atan2(nuevaDireccion.y, nuevaDireccion.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // Mover
        transform.Translate(Vector2.up * velocidad * Time.deltaTime, Space.Self);
    }

    void SeguirAdelante()
    {
        // Movimiento recto
        transform.Translate(Vector2.up * velocidad * Time.deltaTime, Space.Self);
    }

    public void Configurar(Transform nuevoObjetivo, float nuevaVelocidad)
    {
        objetivo = nuevoObjetivo;
        velocidad = nuevaVelocidad;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Destruir misil al tocar jugador
            Destroy(gameObject);
        }
    }
}
