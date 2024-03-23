using UnityEngine;

public class Chave : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerBase>().chaves.Add(id);
            Destroy(gameObject);
        }
    }
}
