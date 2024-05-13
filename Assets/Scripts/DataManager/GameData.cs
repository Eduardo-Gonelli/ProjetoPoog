using UnityEngine;

public class GameData
{
    public Vector2 playerPosition;
    public float playerHealth;
    public float cameraZoom;

    public GameData(Vector2 playerPosition, float playerHealth, float cameraZoom)
    {
        this.playerPosition = playerPosition;
        this.playerHealth = playerHealth;
        this.cameraZoom = cameraZoom;
    }
}
