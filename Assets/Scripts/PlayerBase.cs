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
    public PlayerData playerData;

    // Variável para controlar o tipo de movimento
    [Header("Movimento")]
    public MoveType moveType = MoveType.Translate;    
    Vector3 inputVector;
    public Rigidbody2D rb2d;
    bool facingRight = true;

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


    // Variáveis para controle do Wall Slide
    // baseado no tutorial do canal Blackthornprod: https://www.youtube.com/watch?v=KCzEnKLaaPc
    // É dividido em algumas partes.
    // Parte 1 - Declaração de variáveis
    LayerMask whatIsWall;
    bool isTouchingWall;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlideSpeed;

    // Variável para controlar o puxar caixa
    public bool podePuxarCaixa = false;

    // Variável para controlar as chaves
    public List<int> chaves = new List<int>();


    void Start()
    {
        // Observar que para acessar uma constante de um ScriptableObject
        // é necessário acessar a classe e não a instância
        playerData.health = PlayerData.MAX_HEALTH;
        // Inicializa o componente Rigidbody2D
        rb2d = GetComponent<Rigidbody2D>();
        // Inicializa a máscara de camadas para o Wall Slide
        whatIsWall = LayerMask.GetMask("whatIsWall");
    }

    void Update()
    {        
        // Recebe o input do jogador
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // Cria um vetor com os valores do input
        inputVector = new Vector2(horizontalInput, 0);
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
            if (isGrounded)
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
                rb2d.velocity = Vector2.zero;
                jumpRequested = true;
                doubleJump = false;
            }

        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            podePuxarCaixa = !podePuxarCaixa;
            if (podePuxarCaixa) canJump = false;
            else canJump = true;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            CreateTempPlayerData();
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
        }
        else
        {
            wallSliding = false;
        }

        // Controle do flip
        if(horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if(horizontalInput < 0 && facingRight)
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
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
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
        playerData.health -= damage;
        Debug.Log("Vida: " + playerData.health);
        // Atualiza a UI
        UIManager.instance.UpdateHealthText();
        if (playerData.health <= 0)
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
        Vector3 newPosition = transform.position + inputVector * playerData.speed * Time.deltaTime;
        transform.position = newPosition;
    }

    // Método para mover o personagem usando o método Translate
    void MoveByTransformTranslate()
    {
        Vector3 movement = inputVector * playerData.speed * Time.deltaTime;
        transform.Translate(movement);
    }

    // Método para mover o personagem usando a força do Rigidbody2D
    void MoveByRigidbody2dForce()
    {
        Vector2 forceVector = new Vector2(inputVector.x * playerData.speed, rb2d.velocity.y);
        rb2d.velocity = new Vector2(forceVector.x, rb2d.velocity.y);
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
        float originalSpeed = playerData.speed;
        playerData.speed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        playerData.speed = originalSpeed;
    }

    public IEnumerator Dash3(float distance, float duration, Vector3 direction)
    {
        float time = 0;
        Vector3 originalPosition = transform.position;

        // Realiza o Raycast para frente na direção do dash
        RaycastHit2D hit = Physics2D.Raycast(originalPosition, direction, distance, whatIsWall);

        Vector3 endPosition;
        if (hit.collider != null)
        {
            // Se um obstáculo for detectado, ajusta o ponto final para antes do impacto
            endPosition = originalPosition + (direction * (hit.distance - 0.05f)); // Subtrai um pequeno valor para garantir que não fique "dentro" do obstáculo
        }
        else
        {
            // Se nenhum obstáculo for detectado, procede normalmente
            endPosition = originalPosition + (direction * distance);
        }

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




    #endregion

    #region Exemplo de criação de PlayerData em tempo de execução
    void CreateTempPlayerData()
    {
        StartCoroutine(CreateTemplPlayerDataC());
    }

    IEnumerator CreateTemplPlayerDataC()
    {
        PlayerData originalPlayerData = playerData;
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.health = 400;
        playerData.attack = 200;
        playerData.speed = 2;
        Debug.Log($"Health temporário: {playerData.health}");
        yield return new WaitForSeconds(5);
        playerData = originalPlayerData;
    }
    #endregion
}
