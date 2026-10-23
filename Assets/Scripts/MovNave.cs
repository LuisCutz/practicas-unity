using UnityEngine;

public class MovNave : MonoBehaviour
{
    public float fuerza = 5f;
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rigidbody2D.AddForce(Vector2.up * fuerza, ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            rigidbody2D.AddForce(Vector2.down * fuerza, ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rigidbody2D.AddForce(Vector2.right * fuerza, ForceMode2D.Force);
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rigidbody2D.AddForce(Vector2.left * fuerza, ForceMode2D.Force);
        }
    }
}
