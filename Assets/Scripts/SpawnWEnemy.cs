using UnityEngine;

public class SpawnWEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform target1;
    public Transform target2;
    private WanderingEnemy wanderingEnemy;
    
    void Start()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        wanderingEnemy = enemy.GetComponent<WanderingEnemy>();
        wanderingEnemy.target1 = target1;
        wanderingEnemy.target2 = target2;
        wanderingEnemy.StartEnemy();
    }
}
