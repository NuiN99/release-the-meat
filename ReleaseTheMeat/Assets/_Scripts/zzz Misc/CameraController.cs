using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
   


    [SerializeField] float minSize, maxSize;

    [SerializeField] float sizeDivider;
    [SerializeField] float verticalOffsetPercent;

    private void Update()
    {
        if (GamePhase.instance.currentPhase == GamePhase.Phase.LEVEL)
        {
            MoveCamToCart(CartController.instance.MiddleOfCart());
            SizeCamToCart(CartController.instance.CartSize(sizeDivider, minSize, maxSize));
        }
        
    }

    void MoveCamToCart(Vector3 pos)
    {
        mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
    }

    void SizeCamToCart(float cartSize)
    {
        mainCamera.orthographicSize = cartSize;
    }
}
