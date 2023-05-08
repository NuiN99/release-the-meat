using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlankDragger : MonoBehaviour
{
    [SerializeField] GameObject plankCellPrefab;

    [SerializeField] int maxLength;
    [SerializeField] float cellDistance;

    bool isPlacingPlank;

    List<Vector2> cellPoints = new List<Vector2>();
    List<GameObject> plankCells = new List<GameObject>();

    Vector2 mousePos;
    Vector2 startPoint = Vector2.zero;
    GameObject startCell;
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        PlacePlank();
    }

    void PlacePlank()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPlacingPlank)
            {
                PlaceStartPoint();
                isPlacingPlank = true;
            }
            else
            {
                PlaceEndPoint();
                isPlacingPlank = false;
            }
        }
        else if (isPlacingPlank)
        {
            if (Input.GetMouseButtonDown(1))
            {
                ResetCurrentPlank(false);
                isPlacingPlank = false;
                return;
            }

            RotateToMouse();
            PopulatePlankLength();
        }
    }

    void ResetCurrentPlank(bool keepStartCell)
    {
        foreach (GameObject plankCell in plankCells) Destroy(plankCell);

        if (!keepStartCell) Destroy(startCell);

        cellPoints.Clear();
        plankCells.Clear();
    }

    void PlaceStartPoint()
    {
        startPoint = mousePos;
        startCell = Instantiate(plankCellPrefab, startPoint, Quaternion.identity);
    }

    void PlaceEndPoint()
    {
        GameObject finishedPlank = new("FinishedPlank");
        foreach (GameObject plankCell in plankCells)
        {
            GameObject finishedPlankCell = Instantiate(plankCell, plankCell.transform.position, plankCell.transform.rotation);
            finishedPlankCell.transform.parent = finishedPlank.transform;
        }
        ResetCurrentPlank(false);
    }

    void RotateToMouse()
    {
        if (!isPlacingPlank) return;

        Vector2 mouseDir = (mousePos - startPoint).normalized;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        startCell.transform.eulerAngles = new Vector3(0,0,angle);
    }

    void PopulatePlankLength()
    {
        if (!isPlacingPlank) return;

        float mouseDist = Vector2.Distance(startPoint, mousePos);
        Vector2 mouseDir = (mousePos - startPoint).normalized;

        ResetCurrentPlank(true);

        for (float i = 0; (i < mouseDist) && (i < cellDistance * maxLength); i += cellDistance)
        {
            if (cellDistance <= 0) return;
            cellPoints.Add((startPoint) + (mouseDir * i));
        }

        for(int i = 0; i < cellPoints.Count; i++) 
        {
            Vector2 cell = cellPoints[i];
            GameObject plankCell = Instantiate(plankCellPrefab, cell, startCell.transform.rotation);
            plankCells.Add(plankCell);
        }
    }
}
