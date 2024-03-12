using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum serve como um seletor
// onde s� pode ser escolhido um valor
// de cada vez
public enum MoveType
{
    Transform,
    Translate,
    Rigidbody2D
}

enum DashState
{
    Ready,
    Dashing,
}

public enum DashType
{
    ModoImpulso,
    ModoAlteracaoTemporariaVelocidade,
    ModoMovimentoLinearInterpolado
}

public class PlayerBase : MonoBehaviour
{
    // Vari�vel para controlar a vida do personagem
    const int MAX_HEALTH = 100;
    private int health;
    
    // Vari�vel para controlar o tipo de movimento
    public MoveType moveType = MoveType.Translate;
    [SerializeField] float speed = 5.0f;
    Vector3 inputVector;
    Rigidbody2D rb2d;

    // Vari�vel para controlar o tipo de dash
    public DashType dashType = DashType.ModoImpulso;
    DashState dashState = DashState.Ready;
    float dashCooldown = 3.0f;
    [SerializeField] float dashDuration = 1.0f;


    // Vari�veis para controle do pulo
    public float jumpForce = 500f;
    private Rigidbody2D rb;
    private bool isGrounded = true;      // Flag para indicar se o personagem est� no ch�o    
    private bool jumpRequested = false;  // Flag para indicar se o pulo foi solicitado    
    bool doubleJump = true;              // Flag para indicar se o personagem pode realizar um segundo pulo


    // Vari�veis para controlar o wall slide
    public float slideVelocity = -2f;     // velocidade do slide
    public LayerMask whatIsWall;          // camada que representa a parede
    public Transform wallCheck;           // ponto de verifica��o da parede
    public float wallCheckRadius = 0.2f;  // raio do ponto de verifica��o
    bool isTouchingWall;                  // flag para indicar se est� tocando a parede
    bool isWallSliding = false;           // flag para indicar se est� deslizando na parede

    void Start()
    {
        
        health = MAX_HEALTH;                 // inicializa a vida do personagem com o valor m�ximo
        rb2d = GetComponent<Rigidbody2D>();  // Inicializa o componente Rigidbody2D
    }
    
    void Update()
    {
        // Recebe o input do jogador
        float horizontalInput = Input.GetAxis("Horizontal");        
        // Cria um vetor com os valores do input
        inputVector = new Vector3(horizontalInput, 0, 0);
        // Chama o m�todo de movimento de acordo com o tipo selecionado
        // No Update chama os tipos n�o f�sicos
        switch(moveType)
        {
            case MoveType.Transform:
                MoveByTransform();
                break;
            case MoveType.Translate:
                MoveByTransformTranslate();
                break;
        }  
        
        // Chama o m�todo de Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(dashState == DashState.Ready)
            {
                dashState = DashState.Dashing;
                StartDash();
            }
        }

        // cooldown do dash
        if (dashState == DashState.Dashing)
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown <= 0)
            {
                dashCooldown = 3.0f;
                dashState = DashState.Ready;
            }
        }

        // Jump
        // Captura a solicita��o de pulo
        if (Input.GetButtonDown("Jump"))
        {
            // Verifica se o personagem est� no ch�o
            // Se sim, solicita o pulo
            if (isGrounded)
            {
                jumpRequested = true;
            }
            // Se n�o, verifica se o personagem pode realizar um segundo pulo
            else if (doubleJump)
            {
                // Ao realizar um segundo pulo, as for�as do Rigidbody2D
                // geralmente s�o somadas, o que pode resultar
                // em um pulo muito alto. Para evitar isso,
                // a velocidade do personagem � zerada antes
                rb2d.velocity = Vector2.zero;
                jumpRequested = true;
                doubleJump = false;
            }

        }
        WallSlide();

    }

    private void FixedUpdate()
    {
        // No FixedUpdate chama os tipos f�sicos
        if (moveType == MoveType.Rigidbody2D)
        {
            MoveByRigidbody2dForce();
        }

        // Aplica a for�a de pulo se um pulo foi solicitado e o personagem est� no ch�o
        if (jumpRequested || isWallSliding)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
            isGrounded = false; // Ao pular o personagem n�o est� mais no ch�o
            jumpRequested = false; // Reseta a solicita��o de pulo
        }
    }

    // M�todo para receber dano
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Die();
        }
    }

    // M�todo para verificar se o personagem est� no ch�o
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o personagem est� no ch�o
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Se sim, permite pular novamente
            isGrounded = true;
            doubleJump = true;
        }
    }


    #region Metodos de Movimento
    // M�todo para mover o personagem modificando a posi��o diretamente
    void MoveByTransform()
    {        
        Vector3 newPosition = transform.position + inputVector * speed * Time.deltaTime;
        transform.position = newPosition;
    }

    // M�todo para mover o personagem usando o m�todo Translate
    void MoveByTransformTranslate()
    {
        Vector3 movement = inputVector * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    // M�todo para mover o personagem usando a for�a do Rigidbody2D
    void MoveByRigidbody2dForce()
    {
        Vector3 forceVector = inputVector * speed;
        rb2d.AddForce(forceVector);
    }
    #endregion


    #region M�todos de Dash
    public void StartDash()
    {
        // Verifica qual o tipo de dash selecionado
        switch (dashType)
        {
            case DashType.ModoImpulso:
                StartCoroutine(Dash1(15, dashDuration, rb2d, inputVector));
                break;
            case DashType.ModoAlteracaoTemporariaVelocidade:
                StartCoroutine(Dash2(2, dashDuration, inputVector));
                break;
            case DashType.ModoMovimentoLinearInterpolado:
                StartCoroutine(Dash3(2, dashDuration, inputVector));
                break;
        }
    }

    public IEnumerator Dash1(float force, float duration, Rigidbody2D rb2d, Vector3 direction)
    {
        Vector3 originalVelocity = rb2d.velocity;
        rb2d.velocity = Vector3.zero;
        rb2d.AddForce(direction * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        rb2d.velocity = originalVelocity;
    }

    public IEnumerator Dash2(float speedMultiplier, float duration, Vector3 direction)
    {
        float originalSpeed = speed;
        speed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }

    public IEnumerator Dash3(float distance, float duration, Vector3 direction)
    {
        float time = 0;
        Vector3 originalPosition = transform.position;
        Vector3 endPosition = transform.position + (direction * distance);
        while (time < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }
    #endregion

    #region M�todos de Wall Slide
    public void WallSlide()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if(isTouchingWall && !isGrounded)
        {
            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, slideVelocity);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(wallCheck != null)
        {
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }


    #endregion
}
