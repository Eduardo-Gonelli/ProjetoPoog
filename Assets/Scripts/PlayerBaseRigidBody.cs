using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum serve como um seletor
// onde só pode ser escolhido um valor
// de cada vez

public class PlayerBaseRigidBody : MonoBehaviour
{
    // Variável para controlar a vida do personagem    
    public float health = 100f;
    public float speed = 5f;

    // Variável para controlar o tipo de movimento
    [Header("Movimento")]
    Vector3 inputVector;
    public Rigidbody2D rb2d;
    bool facingRight = true;

    // Variáveis para controle do pulo
    [Header("Pulo")]
    private bool canJump = true;
    public float jumpForce = 500f;
    private bool isGrounded = true;      // Flag para indicar se o personagem está no chão    
    private bool jumpRequested = false;  // Flag para indicar se o pulo foi solicitado    
    bool doubleJump = true;              // Flag para indicar se o personagem pode realizar um segundo pulo
    LayerMask whatIsGround;              // Máscara de camadas para verificar se o personagem está no chão
    public Transform groundCheck;        // Transform que indica a posição do objeto que verifica se o personagem está no chão


    // Variáveis para controle do Wall Slide
    // baseado no tutorial do canal Blackthornprod: https://www.youtube.com/watch?v=KCzEnKLaaPc
    // É dividido em algumas partes.
    // Parte 1 - Declaração de variáveis
    LayerMask whatIsWall;
    bool isTouchingWall;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlideSpeed;

    // Variáveis do WallJump
    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    // Variável para controlar o puxar caixa
    public bool podePuxarCaixa = false;

    // Variável para controlar as chaves
    public List<int> chaves = new List<int>();


    void Start()
    {        
        // Inicializa o componente Rigidbody2D
        rb2d = GetComponent<Rigidbody2D>();
        // Inicializa a máscara de camadas para o Wall Slide
        whatIsWall = LayerMask.GetMask("whatIsWall");
        // Inicializa a máscara de camadas para o Ground Check
        whatIsGround = LayerMask.GetMask("whatIsGround");
    }

    void Update()
    {
        // Recebe o input do jogador
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // Cria um vetor com os valores do input
        inputVector = new Vector2(horizontalInput, 0);

        Vector2 forceVector = new Vector2(inputVector.x * speed, rb2d.velocity.y);
        rb2d.velocity = new Vector2(forceVector.x, rb2d.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
        // Jump
        // Captura a solicitação de pulo
        if (Input.GetButtonDown("Jump") && wallSliding == false)
        {
            // Verifica se o personagem está no chão
            // Se sim, solicita o pulo
            if (isGrounded)
            {
                jumpRequested = true;
                doubleJump = true;
            }
            // Se não, verifica se o personagem pode realizar um segundo pulo
            else if (doubleJump)
            {
                // Ao realizar um segundo pulo, as forças do Rigidbody2D
                // geralmente são somadas, o que pode resultar
                // em um pulo muito alto. Para evitar isso,
                // a velocidade do personagem é zerada antes                
                jumpRequested = true;
                doubleJump = false;
            }

        }

        if(Input.GetButtonDown("Jump") && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (jumpRequested && canJump)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);            
            jumpRequested = false; // Reseta a solicitação de pulo
        }
        else if (jumpRequested && !canJump)
        {
            jumpRequested = false;
        }

        if (wallJumping)
        {
            rb2d.velocity = new Vector2(xWallForce * -horizontalInput, yWallForce);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            podePuxarCaixa = !podePuxarCaixa;
            if (podePuxarCaixa) canJump = false;
            else canJump = true;
        }


        // Wall Slide - Parte 2
        // Cria um círculo na frente do personagem para verificar se ele está tocando a parede
        // O círculo tem um raio de 0.2f e está na posição do objeto frontCheck
        // Apenas a Layer whatIsWall é considerada
        isTouchingWall = Physics2D.OverlapCircle(frontCheck.position, 0.2f, whatIsWall);

        // Se o personagem está tocando a parede e não está no chão
        // e não está pressionando o movimento para os lados
        if (isTouchingWall && !isGrounded && horizontalInput != 0)
        {
            wallSliding = true;
            doubleJump = true;
        }
        else
        {
            wallSliding = false;
        }

        // Controle do flip
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        // Se o personagem está deslizando na parede
        if (wallSliding)
        {
            // Aplica a velocidade de deslizamento na parede utilizando o cálculo
            // Mathf.Clamp(rb2d.velocity.y, wallSlideSpeed, float.MaxValue), que limita
            // a velocidade do personagem entre wallSlideSpeed e float.MaxValue
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    // Método para receber dano
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Vida: " + health);
        // Atualiza a UI
        UIManager.instance.UpdateHealthText();
        if (health <= 0)
        {
            // Die();
            Debug.Log("Morreu");
        }
    }

    public void AdicionarChaves(int chave)
    {
        chaves.Add(chave);
    }

    public void RemoverChave(int chaveId)
    {
        chaves.Remove(chaveId);
        Debug.Log("Chave removida");
    }
}

