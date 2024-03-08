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

public class PlayerBase : MonoBehaviour
{
    // Variável para controlar a vida do personagem
    const int MAX_HEALTH = 100;
    private int health;
    
    // Variável para controlar o tipo de movimento
    public MoveType moveType = MoveType.Translate;
    [SerializeField] float speed = 5.0f;
    Vector3 inputVector;
    Rigidbody2D rb2d;
   
    void Start()
    {
        // inicializa a vida do personagem com o valor máximo
        health = MAX_HEALTH;
        // Inicializa o componente Rigidbody2D
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Recebe o input do jogador
        float horizontalInput = Input.GetAxis("Horizontal");        
        // Cria um vetor com os valores do input
        inputVector = new Vector3(horizontalInput, 0, 0);
        // Chama o método de movimento de acordo com o tipo selecionado
        // No Update chama os tipos não físicos
        switch(moveType)
        {
            case MoveType.Transform:
                MoveByTransform();
                break;
            case MoveType.Translate:
                MoveByTransformTranslate();
                break;
        }        
    }

    private void FixedUpdate()
    {
        // No FixedUpdate chama os tipos físicos
        if (moveType == MoveType.Rigidbody2D)
        {
            MoveByRigidbody2dForce();
        }
    }

    // Método para receber dano
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Die();
        }
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
}
