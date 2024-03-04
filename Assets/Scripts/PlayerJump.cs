using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 700f;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool jumpRequested = false; // Flag para indicar se o pulo foi solicitado

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura a solicitação de pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Aplica a força de pulo se um pulo foi solicitado e o personagem está no chão
        if (jumpRequested)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isGrounded = false; // Supõe-se que o personagem não esteja mais no chão
            jumpRequested = false; // Reseta a solicitação de pulo
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
