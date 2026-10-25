using UnityEngine;

public class ControlEsferaRebote : MonoBehaviour
{
    public float velocidadBarril = 4.8f;

    private Rigidbody2D rb;
    private Vector3 puntoOrigen;
    private float direccionX = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoOrigen = transform.position;

        Collider2D miCollider = GetComponent<Collider2D>();

        // Quitamos el rebote para que ruede pegado al suelo como un barril de Donkey Kong
        miCollider.sharedMaterial = null;
        rb.gravityScale = 2.5f;

        // Ignorar llamas, láseres, trampolines y meta para que nunca se atore
        Collider2D[] todosLosColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        foreach (Collider2D col in todosLosColliders)
        {
            string nombre = col.gameObject.name;
            if (nombre.Contains("Llama") || nombre.Contains("Laser") || nombre.Contains("Trampolin") || nombre.Contains("Meta"))
            {
                Physics2D.IgnoreCollision(miCollider, col, true);
            }
        }

        ReiniciarBarril();
    }

    void Update()
    {
        // Mantiene el barril rodando con linearVelocity (Clase 05 - Ejercicio 3) sin rebotar hacia arriba
        float velY = rb.linearVelocity.y > 0f ? 0f : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(direccionX * velocidadBarril, velY);

        // Cuando llega al final de la planta baja (esquina inferior izquierda) o presionas R, sale un nuevo barril arriba
        if (Input.GetKeyDown(KeyCode.R) || (transform.position.y < -3.5f && transform.position.x < -7.2f))
        {
            ReiniciarBarril();
        }
    }

    void ReiniciarBarril()
    {
        transform.position = puntoOrigen;
        direccionX = 1f;
        rb.linearVelocity = new Vector2(velocidadBarril, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string piso = collision.gameObject.name;

        // Cambia el sentido de rodado según el piso en el que aterrice para recorrer todo el zig-zag
        if (piso.Contains("Plataforma_3") || piso.Contains("Plataforma_1") || piso.Contains("Pared_Izquierda"))
        {
            direccionX = 1f; // Rueda hacia la derecha
        }
        else if (piso.Contains("Plataforma_2") || piso.Contains("Suelo_Base") || piso.Contains("Pared_Derecha"))
        {
            direccionX = -1f; // Rueda hacia la izquierda
        }
    }
}