using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 700f;
    private Rigidbody2D rb;
    // Flag para indicar se o personagem está no chão
    private bool isGrounded = true;
    // Flag para indicar se o pulo foi solicitado
    private bool jumpRequested = false; 
    // Flag para indicar se o personagem pode realizar um segundo pulo
    bool doubleJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura a solicitação de pulo
        if (Input.GetButtonDown("Jump"))
        {
            // Verifica se o personagem está no chão
            // Se sim, solicita o pulo
            if(isGrounded)
            {
                jumpRequested = true;
            }
            // Se não, verifica se o personagem pode realizar um segundo pulo
            else if (doubleJump)
            {
                // Ao realizar um segundo pulo, as forças do Rigidbody2D
                // geralmente são somadas, o que pode resultar
                // em um pulo muito alto. Para evitar isso,
                // a velocidade do personagem é zerada antes
                rb.velocity = Vector2.zero;
                jumpRequested = true;
                doubleJump = false;
            }
            
        }
    }

    void FixedUpdate()
    {
        // Aplica a força de pulo se um pulo foi solicitado e o personagem está no chão
        if (jumpRequested)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isGrounded = false; // Ao pular o personagem não está mais no chão
            jumpRequested = false; // Reseta a solicitação de pulo
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o personagem está no chão
        if(collision.gameObject.CompareTag("Ground"))
        {
            // Se sim, permite pular novamente
            isGrounded = true;
            doubleJump = true;
        }
    }
}
