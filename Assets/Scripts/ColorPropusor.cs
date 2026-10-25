using UnityEngine;

public class ColorPropusor : MonoBehaviour
{
    public float velocidad = 5f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            spriteRenderer.color = Color.red;
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            spriteRenderer.color = Color.yellow;
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)
        || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            spriteRenderer.color = Color.orange;
        }
    }
}
