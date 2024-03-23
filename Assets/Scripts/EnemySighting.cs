using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SightState
{
    justWatching,
    playerDetected,
    playerLost
}

public class EnemySighting : MonoBehaviour
{
    private GameObject player;          // Refer�ncia ao jogador
    private GameObject enemy;           // Refer�ncia ao inimigo
    private float exitCooldown = 2.0f;  // Tempo de espera ap�s sair da �rea de detec��o    
    private SightState sightState = SightState.justWatching;

    private void Update()
    {
        ExitCooldown();
    }

    private void ExitCooldown()
    {
        if (sightState == SightState.playerLost)
        {
            exitCooldown -= Time.deltaTime;
            if (exitCooldown <= 0)
            {
                sightState = SightState.justWatching;
                exitCooldown = 2.0f;
                player = null;
                enemy = null;
                Debug.Log("Player lost!");
            }
        }        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (sightState == SightState.playerLost)
            {
                sightState = SightState.playerDetected;
                exitCooldown = 2.0f;
            }
            player = collision.gameObject;
            enemy = transform.parent.gameObject;
            // Tendo uma refer�ncia para o player e
            // para o inimigo, podemos obter o c�digo associado a ele
            // e chamar a fun��o de alerta, persegui��o ou ataque do inimigo
            Debug.Log("Player detected!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sightState = SightState.playerLost;
            Debug.Log("Searching for the player!");
        }
    }
}
