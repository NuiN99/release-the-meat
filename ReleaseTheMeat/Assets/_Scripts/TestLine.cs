using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDrawing = false;
    private LineRenderer lineRenderer;

    private void Start()
    {
        // Create a new game object to hold the LineRenderer component
        CreateNewLineObject();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isDrawing)
            {
                startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, startPoint);
                isDrawing = true;
            }
            else
            {
                endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(1, endPoint);
                isDrawing = false;

                // Create a new game object to hold the LineRenderer component
                CreateNewLineObject();
            }
        }
        else if (isDrawing)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    private void CreateNewLineObject()
    {
        GameObject lineGO = new GameObject();
        lineRenderer = lineGO.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = 2;
    }
}
