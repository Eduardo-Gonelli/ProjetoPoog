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
    private GameObject player;          // Referência ao jogador
    private GameObject enemy;           // Referência ao inimigo
    private float exitCooldown = 2.0f;  // Tempo de espera após sair da área de detecção    
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
            // Tendo uma referência para o player e
            // para o inimigo, podemos obter o código associado a ele
            // e chamar a função de alerta, perseguição ou ataque do inimigo
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
