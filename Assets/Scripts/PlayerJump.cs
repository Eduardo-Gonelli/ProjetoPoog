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
        // Captura a solicita��o de pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Aplica a for�a de pulo se um pulo foi solicitado e o personagem est� no ch�o
        if (jumpRequested)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isGrounded = false; // Sup�e-se que o personagem n�o esteja mais no ch�o
            jumpRequested = false; // Reseta a solicita��o de pulo
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
