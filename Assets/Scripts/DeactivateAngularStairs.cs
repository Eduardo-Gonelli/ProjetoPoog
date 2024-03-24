using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAngularStairs : MonoBehaviour
{
    public Collider2D stairsCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stairsCollider.isTrigger = true;
        }
    }
}
