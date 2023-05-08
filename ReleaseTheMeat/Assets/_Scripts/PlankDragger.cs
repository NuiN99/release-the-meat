using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlankDragger : MonoBehaviour
{
    [SerializeField] GameObject plankCellPrefab;

    [SerializeField] float cellDistance;

    bool startPointPlaced;

    List<Vector2> cellPoints = new List<Vector2>();
    List<GameObject> plankCells = new List<GameObject>();

    Vector2 mousePos;
    Vector2 startPoint = Vector2.zero;
    GameObject startCell;
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        FinalizePlank();
        PlaceStartPoint();

        RotateToMouse();
        PopulatePlankLength();
    }

    void PlaceStartPoint()
    {
        //RaycastHit2D mouseRay = Physics2D.Raycast(mousePos, -Vector3.forward);

        if (Input.GetMouseButtonDown(0) && !startPointPlaced) 
        {
            startPointPlaced = true;
            startPoint = mousePos;
            startCell = Instantiate(plankCellPrefab, startPoint, Quaternion.identity);
        }
    }

    void FinalizePlank()
    {
        if(Input.GetMouseButtonDown(0) && startPointPlaced)
        {
            GameObject finishedPlank = new("FinishedPlank");

            foreach(GameObject plankCell in plankCells) 
            {
                GameObject finishedPlankCell = Instantiate(plankCell, plankCell.transform.position, plankCell.transform.rotation);
                finishedPlankCell.transform.parent = finishedPlank.transform;
            }

            foreach (GameObject plankCell in plankCells) Destroy(plankCell);

            Destroy(startCell); 

            cellPoints.Clear();
            plankCells.Clear();

            startPointPlaced = false;
        }
    }

    void RotateToMouse()
    {
        if (!startPointPlaced) return;

        Vector2 mouseDir = (mousePos - startPoint).normalized;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        startCell.transform.eulerAngles = new Vector3(0,0,angle);
    }

    void PopulatePlankLength()
    {
        if (!startPointPlaced) return;

        float mouseDist = Vector2.Distance(startPoint, mousePos);
        Vector2 mouseDir = (mousePos - startPoint).normalized;

        foreach(GameObject plankCell in plankCells) Destroy(plankCell);

        cellPoints.Clear();
        plankCells.Clear();

        for (float i = 0; i < mouseDist; i += cellDistance)
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
