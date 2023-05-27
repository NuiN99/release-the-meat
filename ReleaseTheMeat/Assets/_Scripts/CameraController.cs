using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    List<Vector3> partPositions = new List<Vector3>();


    [SerializeField] float minSize, maxSize;

    [SerializeField] float sizeDivider;
    [SerializeField] float verticalOffsetPercent;

    private void Update()
    {
        if (GamePhase.instance.currentPhase == GamePhase.Phase.LEVEL)
        {
            GetPartPositions();
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

    void GetPartPositions()
    {
        partPositions.Clear();
        foreach (Part partType in FindObjectsOfType<Part>())
        {
            partPositions.Add(partType.transform.position);
        }
    }

    Vector3 MiddleOfCart()
    {
        Vector3 sum = new Vector3(0, 0);
        foreach(Vector3 partPos in partPositions)
        {
            sum += partPos;
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

        foreach (Vector3 partPos in partPositions)
        {
            Vector3 position = partPos;

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
