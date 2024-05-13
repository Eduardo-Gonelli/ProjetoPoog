using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Salva os dados do jogo no PlayerPrefs
    public void SaveDataAsPlayerPrefs(Vector2 playerPos, float playerHealth, float cameraZoom)
    {
        // Cria uma instância de GameData com os dados do jogador
        GameData gameData = new GameData(playerPos, playerHealth, cameraZoom);
        // Converte o objeto GameData em uma string JSON
        string json = JsonUtility.ToJson(gameData);
        // Salva a string JSON no PlayerPrefs
        PlayerPrefs.SetString("GameData", json);
    }

    // Carrega os dados do jogo do PlayerPrefs
    public GameData LoadDataFromPlayerPrefs()
    {
        // Recupera a string JSON do PlayerPrefs
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            // Converte a string JSON em um objeto GameData
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            // Retorna o objeto GameData
            return gameData;
        }
        else
        {
            // Se a chave não existir, retorna nulo
            return null;
        }
        
    }

    // Salva os dados do jogo em um arquivo JSON
    public void SaveDataAsJson(Vector2 playerPos, float playerHealth, float cameraZoom)
    {
        // Cria uma instância de GameData com os dados do jogador
        GameData gameData = new GameData(playerPos, playerHealth, cameraZoom);
        // Converte o objeto GameData em uma string JSON
        string json = JsonUtility.ToJson(gameData);
        // Salva a string JSON em um arquivo na pasta persistente do jogo
        System.IO.File.WriteAllText(Application.persistentDataPath + "/GameData.json", json);
    }

    // Carrega os dados do jogo de um arquivo JSON
    public GameData LoadDataFromJson()
    {
        // Verifica se o arquivo JSON existe
        if (System.IO.File.Exists(Application.persistentDataPath + "/GameData.json"))
        {
            // Lê o conteúdo do arquivo JSON
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/GameData.json");
            // Converte a string JSON em um objeto GameData
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            // Retorna o objeto GameData
            return gameData;
        }
        else
        {
            // Se o arquivo não existir, retorna nulo
            return null;
        }
    }
}
