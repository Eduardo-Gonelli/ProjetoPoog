public interface IEnemy
{
    float Health { get; set; }
    float Speed { get; set; }
    string Name { get; set; }
    float Damage { get; set; }

    void ReceiveDamage(float damage);
}
