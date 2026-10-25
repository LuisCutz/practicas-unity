using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugador : MonoBehaviour
{
    public float velocidadMovimiento = 5.5f;
    public float fuerzaSalto = 6.5f;
    public float impulsoTrampolinVertical = 9.2f;
    public float reboteTrampolinLateral = 6.0f;

    [Header("Ajustes del Piso de Hielo")]
    public float velocidadMaxHielo = 2.5f;
    public float desaceleracionHielo = 0.85f;

    [Header("Pantalla de Victoria")]
    public GameObject textoVictoria;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 posicionInicial;
    private bool enSuelo = false;
    private bool enHielo = false;
    private bool juegoTerminado = false;
    private float velocidadDeslizamiento = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        posicionInicial = transform.position;
        rb.linearDamping = 0f;

        // Fricción 0 para que el cubo nunca se quede pegado a las paredes laterales al saltar
        PhysicsMaterial2D matSinFriccion = new PhysicsMaterial2D("SinFriccion");
        matSinFriccion.friction = 0f;
        matSinFriccion.bounciness = 0f;
        GetComponent<Collider2D>().sharedMaterial = matSinFriccion;

        Time.timeScale = 1f;
        if (textoVictoria != null) textoVictoria.SetActive(false);

        Debug.Log("¡Inicia el juego! Llega a la meta amarilla en la cima.");
    }

    void Update()
    {
        // Si ya ganaste, no se procesa movimiento; si presionas R se reinicia la partida
        if (juegoTerminado)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        float direccionX = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direccionX = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direccionX = 1f;
        }

        if (direccionX != 0f)
        {
            rb.linearVelocity = new Vector2(direccionX * velocidadMovimiento, rb.linearVelocity.y);
            velocidadDeslizamiento = direccionX * velocidadMaxHielo;
        }
        else
        {
            if (!enSuelo)
            {
                rb.linearVelocity = new Vector2(velocidadDeslizamiento, rb.linearVelocity.y);
            }
            else if (enHielo)
            {
                velocidadDeslizamiento = Mathf.MoveTowards(velocidadDeslizamiento, 0f, desaceleracionHielo * Time.deltaTime);
                rb.linearVelocity = new Vector2(velocidadDeslizamiento, rb.linearVelocity.y);
            }
            else
            {
                velocidadDeslizamiento = 0f;
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && enSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            enSuelo = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string objetoChocado = collision.gameObject.name;

        if (objetoChocado.Contains("Pared"))
        {
            velocidadDeslizamiento = 0f;
        }
        else if (objetoChocado.Contains("Trampolin"))
        {
            ContactPoint2D contacto = collision.GetContact(0);

            // 1. Si caes desde ARRIBA sobre el trampolín: impulso vertical hacia el siguiente piso
            if (contacto.normal.y > 0.5f)
            {
                enSuelo = false;
                rb.linearVelocity = new Vector2(velocidadDeslizamiento, impulsoTrampolinVertical);
                Debug.Log("¡Gran impulso vertical en trampolín!");
            }
            // 2. Si chocas desde un LATERAL sin saltar: rebote horizontal hacia atrás
            else if (Mathf.Abs(contacto.normal.x) > 0.5f)
            {
                float direccionRebote = contacto.normal.x > 0 ? 1f : -1f;
                velocidadDeslizamiento = direccionRebote * velocidadMaxHielo;
                rb.linearVelocity = new Vector2(direccionRebote * reboteTrampolinLateral, rb.linearVelocity.y);
                Debug.Log("¡Rebote horizontal contra el trampolín!");
            }
        }
        else if (objetoChocado.Contains("Peligro"))
        {
            Debug.Log("¡Chocaste con " + objetoChocado + "! Regresando al inicio...");
            sr.color = Color.cyan;
            enSuelo = false;
            enHielo = false;
            velocidadDeslizamiento = 0f;
            rb.linearVelocity = Vector2.zero;
            transform.position = posicionInicial;
        }
        else if (objetoChocado.Contains("Meta"))
        {
            // Victoria: se congela todo el juego y se activa el cartel en pantalla
            juegoTerminado = true;
            sr.color = Color.green;
            rb.linearVelocity = Vector2.zero;

            if (textoVictoria != null)
            {
                textoVictoria.SetActive(true);
            }

            Time.timeScale = 0f;
            Debug.Log("¡FELICIDADES! Ganaste la partida. Presiona R para reiniciar.");
        }
        else if (objetoChocado.Contains("Suelo") || objetoChocado.Contains("Plataforma"))
        {
            if (collision.transform.position.y < transform.position.y - 0.1f && rb.linearVelocity.y <= 0.1f)
            {
                enSuelo = true;
                enHielo = objetoChocado.Contains("Plataforma_2");
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        string objeto = collision.gameObject.name;

        if (objeto.Contains("Suelo") || objeto.Contains("Plataforma"))
        {
            if (collision.transform.position.y < transform.position.y - 0.1f && rb.linearVelocity.y <= 0.1f)
            {
                enSuelo = true;
                enHielo = objeto.Contains("Plataforma_2");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        string objeto = collision.gameObject.name;

        if (objeto.Contains("Suelo") || objeto.Contains("Plataforma"))
        {
            if (collision.transform.position.y < transform.position.y)
            {
                enSuelo = false;
            }
        }
    }
}