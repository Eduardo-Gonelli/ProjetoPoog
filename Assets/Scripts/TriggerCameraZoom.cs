using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum ZoomType
//{
//    ZoomIn,
//    ZoomOut
//}

public class TriggerCameraZoom : MonoBehaviour
{
    Camera mainCamera;
    CameraController cc;
    //[SerializeField] ZoomType zoomType;
    [SerializeField] [Range(5, 20)] float zoomAmount = 2.0f;
    [SerializeField] [Range(1, 5)] float zoomDuration = 1.0f;

    private void Start()
    {
        mainCamera = Camera.main;
        cc = mainCamera.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(cc.isZooming == false)
                cc.ZoomStart(zoomAmount, zoomDuration);
        }
    }    
}
