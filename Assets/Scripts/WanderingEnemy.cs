using UnityEngine;

enum EnemyState
{
    IDLE,
    WANDERING,
    DYING
}

public class WanderingEnemy : MonoBehaviour, IEnemy
{    
    public float Health { get; set; }
    public float Speed { get; set; }
    public string Name { get; set; }
    public float Damage { get; set; }

    private EnemyState state;
    public Transform target1;
    public Transform target2;
    public Transform actualTarget;

    public void StartEnemy()
    {
        Health = 100;
        Speed = 10f;
        Name = "Wandering Enemy";
        Damage = 10f;
        state = EnemyState.WANDERING;
        actualTarget = target1;
    }

    public void ReceiveDamage(float damage)
    {
        Health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.WANDERING:
                Wander();
                break;
            case EnemyState.DYING:
                Die();
                break;
        }
    }

    private void Wander()
    {
        if (state != EnemyState.DYING)
        {
            if (Vector3.Distance(transform.position, actualTarget.position) < 0.1f)
            {
                if (actualTarget == target1)
                {
                    actualTarget = target2;
                }
                else
                {
                    actualTarget = target1;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, actualTarget.position, Speed * Time.deltaTime);
        }
    }

    private void Die()
    {
        Destroy(this.gameObject, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBase player = collision.gameObject.GetComponent<PlayerBase>();
            player.TakeDamage(Damage);
        }
    }
}
