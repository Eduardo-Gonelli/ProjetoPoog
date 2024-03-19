using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushing : MonoBehaviour
{
    PlayerBase player;
    GameObject playerObject;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player movendo a caixa
            PlayerBase player = collision.gameObject.GetComponent<PlayerBase>();
            this.player = player;
            playerObject = collision.gameObject;
        }
    }

    private void Update()
    {
        if(player != null)
        {
            if (player.podePuxarCaixa)
            {
                rb.isKinematic = true;
                this.transform.SetParent(playerObject.transform);
            }
            else
            {
                rb.isKinematic = false;
                this.transform.SetParent(null);
            }
        }
    }
}
