using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PlayerData", fileName = "New Player Data")]
public class PlayerData : ScriptableObject
{
    public const float MAX_HEALTH = 100;
    public float health;
    public float attack;
    public float speed;
    public Vector2 currentPosition;
}
