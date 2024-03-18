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
        player.transform.position = startPoint.position;
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
