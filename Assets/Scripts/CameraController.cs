/// <summary>
/// Exemplo de c�mera extra�do do tutorial:
/// https://www.codemahal.com/making-the-camera-follow-the-player-2d-unity
/// Adaptado por Eduardo Gonelli para permitir suaviza��o no eixo Y
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;       // Refer�ncia ao jogador - associar via Inspector
    public float offsetX;           // Offset no eixo X
    public float offsetY;           // Offset no eixo Y
    public float offsetSmoothingX;  // Suaviza��o no eixo X
    public float offsetSmoothingY;  // Suaviza��o no eixo Y
    private Vector3 playerPosition; // Posi��o do jogador

    void Update()
    {
        // Posi��o inicial do jogador combinada com os offsets e com a c�mera
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);

        // Se o jogador estiver virado para a direita, o offset � positivo
        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offsetX, playerPosition.y, playerPosition.z);
        }
        // Se o jogador estiver virado para a esquerda, o offset � negativo
        else
        {
            playerPosition = new Vector3(playerPosition.x - offsetX, playerPosition.y, playerPosition.z);
        }

        // Suaviza��o separada para os eixos X e Y
        Vector3 newPosition = Vector3.Lerp(transform.position, playerPosition, offsetSmoothingX * Time.deltaTime);
        newPosition.y = Mathf.Lerp(transform.position.y, playerPosition.y, offsetSmoothingY * Time.deltaTime);
        transform.position = newPosition;
    }
}
