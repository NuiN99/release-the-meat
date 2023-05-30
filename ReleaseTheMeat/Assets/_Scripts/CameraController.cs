using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    List<Transform> partPositions = new List<Transform>();


    [SerializeField] float minSize, maxSize;

    [SerializeField] float sizeDivider;
    [SerializeField] float verticalOffsetPercent;

    private void Update()
    {
        if (GamePhase.instance.currentPhase == GamePhase.Phase.LEVEL)
        {
            MoveCamToCart(MiddleOfCart());
            SizeCamToCart(CartSize());
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

    public void GetPartPositions()
    {
        Part[] parts = FindObjectsOfType<Part>();
        foreach(Part part in parts)
        {
            partPositions.Add(part.gameObject.transform);
        }
    }

    Vector3 MiddleOfCart()
    {
        Vector3 sum = new Vector3(0, 0);
        foreach(Transform partPos in partPositions)
        {
            sum += partPos.position;
        }

        if (partPositions.Count == 0) return mainCamera.transform.position;

        else return sum / partPositions.Count;
    }

    float CartSize()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (Transform partPos in partPositions)
        {
            if (Vector2.Distance(MiddleOfCart(), partPos.position) > 50) 
            {
                partPositions.Remove(partPos);
                continue;
            } 

            Vector3 position = partPos.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        float distX = maxX - minX;
        float distY = maxY - minY;

        float cartSize = (distX + distY) / sizeDivider;

        if (cartSize < minSize) return minSize;
        else if (cartSize > maxSize) return maxSize;

        else return cartSize;
    }
}
