using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Remove unused statements

//namesapce
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
    //Consider adding comments to your methods
    //You can add readable comments by hitting the / three times like /// 
    // Visual Studio will then setup a cool premade comments spot that can then be read when hovering over the method

    //I'm going to do the next one as an example


    /// <summary>
    /// This was setup automatically for me after typeing ///
    /// </summary>
    /// <param name="pos">You can give context to parameters</param>
    void MoveCamToCart(Vector3 pos)
    {
        mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
    }

    void SizeCamToCart(float cartSize)
    {
        mainCamera.orthographicSize = cartSize;
    }
}
