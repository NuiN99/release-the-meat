using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankDragger : MonoBehaviour
{
    [SerializeField] GameObject plankCellPrefab;
    [SerializeField] GameObject finishedPlankPrefab;

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
                FinalizePlank();
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

 
    void PlaceStartPoint()
    {
        startPoint = mousePos;
        startCell = Instantiate(plankCellPrefab, startPoint, Quaternion.identity);
    }

    void FinalizePlank()
    {
        int cellCount = plankCells.Count;
        if (cellCount <= 1) 
        {
            ResetCurrentPlank(false);
            return;
        } 

        Vector2 endPoint = cellPoints[cellCount - 1];
        
        GameObject finishedPlank = Instantiate(finishedPlankPrefab, (startPoint + endPoint) / 2, startCell.transform.rotation);
        finishedPlank.name = "Plank";
        BoxCollider2D plankCol = finishedPlank.GetComponent<BoxCollider2D>();
        Rigidbody2D plankRB = finishedPlank.GetComponent<Rigidbody2D>();
        Plank plankScript = finishedPlank.GetComponent<Plank>();

        for(int i = 0; i < plankCells.Count; i++)
        {
            GameObject finishedPlankCell = Instantiate(plankCells[i], plankCells[i].transform.position, plankCells[i].transform.rotation);
            finishedPlankCell.name = $"PlankCell{i + 1}";
            finishedPlankCell.transform.parent = finishedPlank.transform;
        }
        
        plankScript.isPlaced = true;
        plankScript.SetCells();

        SetColliderSize(plankCol);
        SetMass(plankRB);
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

        for (float i = 0; (i < mouseDist) && (i < cellDistance * (maxLength - 1)); i += cellDistance)
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

    void ResetCurrentPlank(bool keepStartCell)
    {
        foreach (GameObject plankCell in plankCells) Destroy(plankCell);

        if (!keepStartCell) Destroy(startCell);

        cellPoints.Clear();
        plankCells.Clear();
    }


    void SetColliderSize(BoxCollider2D col)
    {
        float sizeX = plankCells.Count * cellDistance;
        float sizeY = plankCellPrefab.transform.localScale.x;
        col.size = new Vector2(sizeX, sizeY);
    }

    void SetMass(Rigidbody2D rb)
    {
        rb.mass = plankCells.Count / 10f;
    }
}
