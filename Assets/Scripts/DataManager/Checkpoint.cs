using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    public TextMeshPro checkpointText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Salva os dados do jogo no PlayerPrefs
            GameDataManager.instance.SaveDataAsPlayerPrefs(collision.transform.position, collision.GetComponent<PlayerBase>().playerData.health, Camera.main.orthographicSize);
            // Salva os dados do jogo em um arquivo JSON
            GameDataManager.instance.SaveDataAsJson(collision.transform.position, collision.GetComponent<PlayerBase>().playerData.health, Camera.main.orthographicSize);
            // Exibe uma mensagem na tela
            checkpointText.text = "Checkpoint salvo!";
        }
    }
}
