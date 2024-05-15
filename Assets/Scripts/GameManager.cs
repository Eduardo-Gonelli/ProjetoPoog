using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform startPoint;
    [SerializeField] CameraController cc;
    // Start is called before the first frame update
    void Start()
    {
        // Carrega a posição de acordo com o GameData
        //GameData gameData = GameDataManager.instance.LoadDataFromPlayerPrefs();
        // Carregando do arquivo Json
        GameData gameData = GameDataManager.instance.LoadDataFromJson();
        if (gameData != null)
        {
            player.transform.position = gameData.playerPosition;
            player.GetComponent<PlayerBase>().playerData.health = gameData.playerHealth;
            cc.ZoomStart(gameData.cameraZoom, 1);
        }
        else
        {
            player.transform.position = startPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPos();
        }
    }

    void ResetPlayerPos()
    {
        player.transform.position = startPoint.position;
        cc.ZoomStart(5, 1);
    }
}
