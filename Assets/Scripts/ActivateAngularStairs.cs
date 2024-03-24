using UnityEngine;

public class ActivateAngularStairs : MonoBehaviour
{
    public Collider2D stairsCollider;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stairsCollider.isTrigger = false;
        }
    }
}
