using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainelPorta : MonoBehaviour
{
    public Porta porta;    
    public bool jogadorPerto = false;
    public bool possuiChave = false;
    public int portaId = 1;
    public SpriteRenderer chaveOn;
    private PlayerBase playerBase;
        
    // Update is called once per frame
    void Update()
    {
        if(jogadorPerto && Input.GetKeyDown(KeyCode.F))
        {
            if (possuiChave)
            {
                porta.Destrancar();
                chaveOn.sortingOrder = 0;
            }
            else
            {
                Debug.Log("Preciso de uma chave para abrir");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            jogadorPerto = true;

            if (possuiChave)
            {
                Debug.Log("Pressione 'F' para abrir a porta");
                return;
            }
            playerBase = collision.GetComponent<PlayerBase>();            

            if (playerBase.chaves.Contains(portaId))
            {
                possuiChave = true;
                playerBase.RemoverChave(portaId);
                Debug.Log("Pressione 'F' para abrir a porta");
            }
            else
            {
                Debug.Log("Preciso de uma chave para abrir");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            jogadorPerto = false;            
        }
    }
}
