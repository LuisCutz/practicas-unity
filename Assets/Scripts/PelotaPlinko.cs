using UnityEngine;

public class PelotaPlinko : MonoBehaviour
{
    [Header("Fuerza aleatoria inicial")]
    [SerializeField] private float fuerzaHorizontalMin = -0.15f;
    [SerializeField] private float fuerzaHorizontalMax = 0.15f;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        // Variación diminuta en la posición inicial X
        float desvioX = Random.Range(-0.02f, 0.02f);
        transform.position += new Vector3(desvioX, 0f, 0f);

        // Impulso horizontal aleatorio hacia la izquierda o derecha
        float impulso = Random.Range(fuerzaHorizontalMin, fuerzaHorizontalMax);
        rb.AddForce(new Vector2(impulso, 0f), ForceMode2D.Impulse);
    }
}