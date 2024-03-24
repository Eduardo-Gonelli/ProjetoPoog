using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum serve como um seletor
// onde só pode ser escolhido um valor
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
    // Variável para controlar a vida do personagem
    const int MAX_HEALTH = 100;
    private float health;

    // Variável para controlar o tipo de movimento
    [Header("Movimento")]
    public MoveType moveType = MoveType.Translate;
    [SerializeField] float speed = 5.0f;
    Vector3 inputVector;
    public Rigidbody2D rb2d;

    // Variável para controlar o tipo de dash
    [Header("Dash")]
    public DashType dashType = DashType.ModoImpulso;
    DashState dashState = DashState.Ready;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashForce = 10.0f;
    [SerializeField] float dashCooldown = 1.0f;
    float dashCooldownAux;



    // Variáveis para controle do pulo
    [Header("Pulo")]
    private bool canJump = true;
    public float jumpForce = 500f;
    private bool isGrounded = true;      // Flag para indicar se o personagem está no chão    
    private bool jumpRequested = false;  // Flag para indicar se o pulo foi solicitado    
    bool doubleJump = true;              // Flag para indicar se o personagem pode realizar um segundo pulo


    // Variáveis para controlar o wall slide
    public float slideVelocity = -2f;     // velocidade do slide
    public LayerMask whatIsWall;          // camada que representa a parede
    public Transform wallCheck;           // ponto de verificação da parede
    public float wallCheckRadius = 0.2f;  // raio do ponto de verificação
    bool isTouchingWall;                  // flag para indicar se está tocando a parede
    bool isWallSliding = false;           // flag para indicar se está deslizando na parede
    float wallSliderJumpTimer = 0.2f;
    bool canSlide = true;

    // Variável para controlar o puxar caixa
    public bool podePuxarCaixa = false;

    // Variável para controlar as chaves
    public List<int> chaves = new List<int>();


    void Start()
    {

        health = MAX_HEALTH;                 // inicializa a vida do personagem com o valor máximo
        rb2d = GetComponent<Rigidbody2D>();  // Inicializa o componente Rigidbody2D
    }

    void Update()
    {
        // Recebe o input do jogador
        float horizontalInput = Input.GetAxis("Horizontal");
        // Cria um vetor com os valores do input
        inputVector = new Vector3(horizontalInput, 0, 0);
        // Chama o método de movimento de acordo com o tipo selecionado
        // No Update chama os tipos não físicos
        switch (moveType)
        {
            case MoveType.Transform:
                MoveByTransform();
                break;
            case MoveType.Translate:
                MoveByTransformTranslate();
                break;
        }

        // Chama o método de Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashState == DashState.Ready)
            {
                dashState = DashState.Dashing;
                dashCooldownAux = dashCooldown;
                StartDash();
            }
        }

        // cooldown do dash
        if (dashState == DashState.Dashing)
        {
            dashCooldownAux -= Time.deltaTime;
            if (dashCooldownAux <= 0)
            {
                dashCooldownAux = 3.0f;
                dashState = DashState.Ready;
            }
        }

        // Jump
        // Captura a solicitação de pulo
        if (Input.GetButtonDown("Jump"))
        {
            // Verifica se o personagem está no chão
            // Se sim, solicita o pulo
            if (isGrounded || isWallSliding)
            {
                if (isWallSliding)
                {
                    canSlide = false;
                }
                jumpRequested = true;
            }
            // Se não, verifica se o personagem pode realizar um segundo pulo
            else if (doubleJump)
            {
                // Ao realizar um segundo pulo, as forças do Rigidbody2D
                // geralmente são somadas, o que pode resultar
                // em um pulo muito alto. Para evitar isso,
                // a velocidade do personagem é zerada antes
                rb2d.velocity = Vector2.zero;
                jumpRequested = true;
                doubleJump = false;
            }

        }
        if (wallSliderJumpTimer > 0 && !canSlide)
        {
            wallSliderJumpTimer -= Time.deltaTime;
            if (wallSliderJumpTimer <= 0)
            {
                canSlide = true;
                wallSliderJumpTimer = 0.2f;
            }
        }
        else if (canSlide)
        {
            WallSlide();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            podePuxarCaixa = !podePuxarCaixa;
            if (podePuxarCaixa) canJump = false;
            else canJump = true;
        }

    }

    private void FixedUpdate()
    {
        // No FixedUpdate chama os tipos físicos
        if (moveType == MoveType.Rigidbody2D)
        {
            MoveByRigidbody2dForce();
        }

        // Aplica a força de pulo se um pulo foi solicitado e o personagem está no chão
        if (jumpRequested && canJump)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(0, jumpForce));
            isGrounded = false; // Ao pular o personagem não está mais no chão
            jumpRequested = false; // Reseta a solicitação de pulo
        }
        else if (jumpRequested && !canJump)
        {
            jumpRequested = false;
        }
    }

    // Método para receber dano
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Vida: " + health);
        if (health <= 0)
        {
            // Die();
            Debug.Log("Morreu");
        }
    }

    // Método para verificar se o personagem está no chão
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o personagem está no chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Se sim, permite pular novamente
            isGrounded = true;
            doubleJump = true;
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


    #region Metodos de Movimento
    // Método para mover o personagem modificando a posição diretamente
    void MoveByTransform()
    {
        Vector3 newPosition = transform.position + inputVector * speed * Time.deltaTime;
        transform.position = newPosition;
    }

    // Método para mover o personagem usando o método Translate
    void MoveByTransformTranslate()
    {
        Vector3 movement = inputVector * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    // Método para mover o personagem usando a força do Rigidbody2D
    void MoveByRigidbody2dForce()
    {
        Vector3 forceVector = inputVector * speed;
        rb2d.AddForce(forceVector);
    }
    #endregion


    #region Métodos de Dash
    public void StartDash()
    {
        // Verifica qual o tipo de dash selecionado
        switch (dashType)
        {
            case DashType.ModoImpulso:
                StartCoroutine(Dash1(dashForce, dashDuration, rb2d, inputVector));
                break;
            case DashType.ModoAlteracaoTemporariaVelocidade:
                StartCoroutine(Dash2(dashForce, dashDuration, inputVector));
                break;
            case DashType.ModoMovimentoLinearInterpolado:
                StartCoroutine(Dash3(dashForce, dashDuration, inputVector));
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

    #region Métodos de Wall Slide
    public void WallSlide()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if (isTouchingWall && !isGrounded)
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
        if (wallCheck != null)
        {
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }


    #endregion
}
