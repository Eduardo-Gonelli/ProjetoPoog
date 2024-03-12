using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ZoomType
{
    ZoomIn,
    ZoomOut
}

public class TriggerCameraZoom : MonoBehaviour
{
    Camera mainCamera;
    CameraController cc;
    [SerializeField] ZoomType zoomType;

    private void Start()
    {
        mainCamera = Camera.main;
        cc = mainCamera.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (zoomType == ZoomType.ZoomOut)
                cc.ZoomOut();
            else if (zoomType == ZoomType.ZoomIn)
                cc.ZoomIn();            
        }
    }    
}
