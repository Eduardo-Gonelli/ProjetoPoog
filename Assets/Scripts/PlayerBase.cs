using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    const int MAX_HEALTH = 100;
    private int health;
    [SerializeField] float speed = 5.0f;
    
    //Vector3 inputVector;
    Rigidbody2D rb2d;
   
    void Start()
    {
        health = MAX_HEALTH;
        rb2d = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // inputVector = new Vector3(horizontalInput, verticalInput, 0);
        MoveByTransformTranslate(horizontalInput, verticalInput);
        //MoveByTransform(horizontalInput, verticalInput);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Die();
        }
    }
    // Método para mover o personagem modificando a posição diretamente
    void MoveByTransform(float horizontalInput, float verticalInput)
    {
        // Removendo a gravidade do personagem podemos experimentar o verticalInput
        Vector3 newPosition = transform.position + new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.position = newPosition;
    }
    // Método para mover o personagem usando o método Translate
    void MoveByTransformTranslate(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
    private void FixedUpdate()
    {
        //MoveByRigidbody2dForce();
    }
    //// Método para mover o personagem usando a força do Rigidbody2D
    //void MoveByRigidbody2dForce()
    //{
    //    Vector3 forceVector = inputVector * speed;
    //    rb2d.AddForce(forceVector);
    //}

    void Jump()
    {
        // Código para fazer o personagem pular
    }
}
