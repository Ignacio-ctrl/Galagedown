using UnityEngine;

public class huntBoss : MonoBehaviour
{
    [Header("Persecución")]
    [SerializeField] private float velocidad = 6f;          
    [SerializeField] private float distanciaParada = 1f;    

    [Header("Disparo Rápido")]
    [SerializeField] private GameObject balaPrefab;         
    [SerializeField] private float cadenciaDisparo = 0.5f;  
    [SerializeField] private Transform puntoDisparo;

    [Header("Referencias")]
    [SerializeField] private LayerMask capaObstaculos;

    // Variables internas
    private Transform jugador;
    private Rigidbody2D rb;
    private float timerDisparo;
    private bool puedeVerJugador = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Buscar jugador
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
            Debug.Log("Jugador encontrado: " + jugador.name);
        }
        else
        {
            Debug.LogError("No hay objeto con tag 'Player' en la escena!");
            enabled = false; // Desactivar script si no hay jugador
            return;
        }

        // Si no hay punto de disparo, usar este transform
        if (puntoDisparo == null) puntoDisparo = transform;

        // Timer de disparo listo para disparar inmediatamente
        timerDisparo = 0f;
    }

    void Update()
    {
        if (jugador == null) return;

        // Verificar visión con Raycast
        VerificarVision();

        //  Rotar hacia el jugador
        RotarHaciaJugador();

        //  Controlar disparos rápidos
        ControlarDisparos();
    }

    void FixedUpdate()
    {
        if (jugador == null || !puedeVerJugador) return;

        //  Perseguir AGGRESIVAMENTE
        PerseguirAgresivamente();
    }

    void VerificarVision()
    {
        Vector2 direccion = (jugador.position - transform.position).normalized;
        float distancia = Vector2.Distance(transform.position, jugador.position);

        // Raycast para ver si hay línea de visión
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direccion,
            distancia,
            capaObstaculos
        );

        // Debug visual
        Debug.DrawRay(transform.position, direccion * distancia,
                     puedeVerJugador ? Color.green : Color.red);

        // Si el raycast golpea al jugador, puede verlo
        puedeVerJugador = (hit.collider != null && hit.collider.CompareTag("Player"));
    }

    void RotarHaciaJugador()
    {
        // Rotación INSTANTÁNEA hacia el jugador
        Vector2 direccion = jugador.position - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angulo; // Rotación instantánea, sin suavizado
    }

    void PerseguirAgresivamente()
    {
        float distancia = Vector2.Distance(transform.position, jugador.position);

        
        if (distancia > distanciaParada)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            Vector2 nuevaPosicion = rb.position + direccion * velocidad * Time.fixedDeltaTime;
            rb.MovePosition(nuevaPosicion);
        }
    }

    void ControlarDisparos()
    {
        // Solo disparar si puede ver al jugador
        if (!puedeVerJugador || balaPrefab == null) return;

        timerDisparo -= Time.deltaTime;

        
        if (timerDisparo <= 0f)
        {
            DispararBalaRapida();
            timerDisparo = cadenciaDisparo; // Reset timer
        }
    }

    void DispararBalaRapida()
    {
        // Crear bala en el punto de disparo con la rotación actual
        Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);

        
        
    }

    
  

    // Método para cuando el jugador está cerca
    public void JugadorCerca(bool cerca)
    {
        if (cerca)
        {
            // Si el jugador está muy cerca, disparar aún más rápido
            cadenciaDisparo = 0.25f; 
        }
        else
        {
            // Volver a la cadencia normal
            cadenciaDisparo = 0.5f;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Mostrar distancia de parada
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaParada);

        // Mostrar dirección hacia donde mira
        Gizmos.color = Color.yellow;
        Vector3 direccionFrente = transform.up * 2f;
        Gizmos.DrawRay(transform.position, direccionFrente);
    }
}