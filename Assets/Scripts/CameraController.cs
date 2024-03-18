/// <summary>
/// Exemplo de câmera extraído do tutorial:
/// https://www.codemahal.com/making-the-camera-follow-the-player-2d-unity
/// Adaptado por Eduardo Gonelli para permitir suavização no eixo Y
/// </summary>

using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;       // Referência ao jogador - associar via Inspector
    public float offsetX;           // Offset no eixo X
    public float offsetY;           // Offset no eixo Y
    public float offsetSmoothingX;  // Suavização no eixo X
    public float offsetSmoothingY;  // Suavização no eixo Y
    private Vector3 playerPosition; // Posição do jogador
    public bool isZooming = false;  // Flag para indicar se a câmera está em processo de zoom

    void Update()
    {
        // Posição inicial do jogador combinada com os offsets e com a câmera
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);

        // Se o jogador estiver virado para a direita, o offset é positivo
        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offsetX, playerPosition.y, playerPosition.z);
        }
        // Se o jogador estiver virado para a esquerda, o offset é negativo
        else
        {
            playerPosition = new Vector3(playerPosition.x - offsetX, playerPosition.y, playerPosition.z);
        }

        // Suavização separada para os eixos X e Y
        Vector3 newPosition = Vector3.Lerp(transform.position, playerPosition, offsetSmoothingX * Time.deltaTime);
        newPosition.y = Mathf.Lerp(transform.position.y, playerPosition.y, offsetSmoothingY * Time.deltaTime);
        transform.position = newPosition;
    }

    public void ZoomStart(float zoomAmount, float zoomDuration)
    {
        isZooming = true;
        StartCoroutine(Zoom(zoomAmount, zoomDuration));
    }

    IEnumerator Zoom(float size, float duration)
    {
        float time = 0;
        float orinalSize = Camera.main.orthographicSize;
        float newSize = size;
        if(size == orinalSize)
        {
            isZooming = false;
            yield break;
        }
        while (time < duration)
        {
            Camera.main.orthographicSize = Mathf.Lerp(orinalSize, newSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        isZooming = false;
        Camera.main.orthographicSize = newSize;
    }
}
