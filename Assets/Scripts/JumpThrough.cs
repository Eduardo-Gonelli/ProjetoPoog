using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrough : MonoBehaviour
{
    private GameObject player;
    private float halfY;
    private float playerHalfY;
    Collider2D colliderBox;
    Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<Collider2D>();
        colliderBox = GetComponent<BoxCollider2D>();
        halfY = GetComponent<BoxCollider2D>().size.y / 2;
        playerHalfY = playerCollider.bounds.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y - playerHalfY < transform.position.y + halfY && colliderBox.isTrigger == false)
        {
            colliderBox.isTrigger = true;
        }
        else if (player.transform.position.y - playerHalfY > transform.position.y + halfY && colliderBox.isTrigger == true)
        {
            colliderBox.isTrigger = false;
        }
    }
}
