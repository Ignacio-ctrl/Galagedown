using UnityEngine;

public class boss2Move : MonoBehaviour
{
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float intervaloCambioDireccion = 2f;
    [SerializeField] private float anguloMaximoCambio = 45f;

    [SerializeField] private string[] tagsParaRebotar = { "Limite" };
    [SerializeField] private float fuerzaRebote = 5f;
    [SerializeField] private bool usarFisica = true;

    private Rigidbody2D rb;
    private Vector2 direccionActual;
    private float timerDireccion;

    void Start()
    {
        // Obtener Rigidbody si existe
        if (usarFisica)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogWarning("No hay Rigidbody2D. Usando movimiento simple.");
                usarFisica = false;
            }
            else
            {
                rb.gravityScale = 0;
                rb.linearDamping = 0.5f;
            }
        }

        // Dirección inicial aleatoria
        direccionActual = Random.insideUnitCircle.normalized;
        timerDireccion = intervaloCambioDireccion;

        Debug.Log("Enemigo errático iniciado. Velocidad: " + velocidad);
    }

    void Update()
    {
        // Cambiar dirección periódicamente
        timerDireccion -= Time.deltaTime;
        if (timerDireccion <= 0)
        {
            CambiarDireccionAleatoria();
            timerDireccion = intervaloCambioDireccion;
        }

        // Rotar para mirar en dirección de movimiento
        RotarHaciaDireccion();
    }

    void FixedUpdate()
    {
        if (usarFisica && rb != null)
        {
            // Movimiento con física
            rb.AddForce(direccionActual * velocidad);

            // Limitar velocidad máxima
            if (rb.linearVelocity.magnitude > velocidad * 1.5f)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * velocidad * 1.5f;
            }
        }
        else
        {
            // Movimiento simple sin física
            transform.Translate(direccionActual * velocidad * Time.deltaTime, Space.World);
        }
    }

    void CambiarDireccionAleatoria()
    {
        // Ángulo aleatorio dentro del rango permitido
        float anguloAleatorio = Random.Range(-anguloMaximoCambio, anguloMaximoCambio);

        // Rotar la dirección actual
        Quaternion rotacion = Quaternion.Euler(0, 0, anguloAleatorio);
        direccionActual = rotacion * direccionActual;
        direccionActual.Normalize();

        // Pequeño impulso adicional
        if (usarFisica && rb != null)
        {
            rb.AddForce(direccionActual * velocidad * 0.3f, ForceMode2D.Impulse);
        }
    }

    void RotarHaciaDireccion()
    {
        if (direccionActual != Vector2.zero)
        {
            float angulo = Mathf.Atan2(direccionActual.y, direccionActual.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angulo);
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        RebotarSiEsNecesario(otro.gameObject);
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        RebotarSiEsNecesario(colision.gameObject);
    }

    void RebotarSiEsNecesario(GameObject objeto)
    {
        // Verificar si el objeto tiene una tag de rebote
        foreach (string tag in tagsParaRebotar)
        {
            if (objeto.CompareTag(tag))
            {
                AplicarRebote(objeto);
                return;
            }
        }
    }

    void AplicarRebote(GameObject objetoColisionado)
    {
        // Calcular vector desde el enemigo al objeto
        Vector2 haciaObjeto = objetoColisionado.transform.position - transform.position;
        Vector2 normal = -haciaObjeto.normalized;

        // Calcular nueva dirección (reflejo)
        direccionActual = Vector2.Reflect(direccionActual, normal).normalized;

        // Aplicar fuerza de rebote si usa física
        if (usarFisica && rb != null)
        {
            rb.AddForce(direccionActual * fuerzaRebote, ForceMode2D.Impulse);
        }

        // Resetear timer de cambio de dirección
        timerDireccion = intervaloCambioDireccion;

        Debug.Log("Rebotó con: " + objetoColisionado.name);
    }

    // Métodos públicos para control externo

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    public void ForzarDireccion(Vector2 nuevaDireccion)
    {
        direccionActual = nuevaDireccion.normalized;
        timerDireccion = intervaloCambioDireccion;
    }

    public void DetenerMovimiento()
    {
        if (usarFisica && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        direccionActual = Vector2.zero;
    }

    // Debug visual en el Editor
    void OnDrawGizmosSelected()
    {
        // Mostrar dirección actual
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direccionActual * 2f);

        // Mostrar rango de cambio de dirección
        Gizmos.color = Color.yellow;
        if (direccionActual != Vector2.zero)
        {
            Vector2 limiteIzq = Quaternion.Euler(0, 0, -anguloMaximoCambio) * direccionActual;
            Vector2 limiteDer = Quaternion.Euler(0, 0, anguloMaximoCambio) * direccionActual;
            Gizmos.DrawRay(transform.position, limiteIzq * 1.5f);
            Gizmos.DrawRay(transform.position, limiteDer * 1.5f);
        }

        // Mostrar velocidad actual
        if (usarFisica && rb != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, rb.linearVelocity.normalized * (rb.linearVelocity.magnitude / velocidad));
        }
    }
}
