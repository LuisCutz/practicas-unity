using UnityEngine;

public class ObstaculoTransform : MonoBehaviour
{
    [Header("Patrulla con Translate (Llamitas)")]
    public bool moverArribaAbajo = true;
    public float velocidadMovimiento = 2.0f;
    public float distanciaRecorrido = 1.55f; // Recorre todo el alto libre del pasillo

    [Header("Giro con Rotate (Láser)")]
    public bool girarContinuamente = false;
    public float velocidadGiro = 120f;

    private Vector3 posicionInicial;
    private int direccion = 1;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // 1. Va desde el suelo exacto (posicionInicial.y) hasta rozar el techo sin pasarse (Clase 04)
        if (moverArribaAbajo)
        {
            transform.Translate(Vector3.up * direccion * velocidadMovimiento * Time.deltaTime);

            if (transform.position.y >= posicionInicial.y + distanciaRecorrido)
            {
                transform.position = new Vector3(transform.position.x, posicionInicial.y + distanciaRecorrido, transform.position.z);
                direccion = -1;
            }
            else if (transform.position.y <= posicionInicial.y)
            {
                transform.position = new Vector3(transform.position.x, posicionInicial.y, transform.position.z);
                direccion = 1;
            }
        }

        // 2. Rotación continua para los láseres con Transform.Rotate (Clase 04)
        if (girarContinuamente)
        {
            transform.Rotate(0f, 0f, velocidadGiro * Time.deltaTime);
        }
    }
}