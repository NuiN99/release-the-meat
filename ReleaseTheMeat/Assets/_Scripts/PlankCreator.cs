using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCreator : MonoBehaviour
{
    [SerializeField] float minLength;
    [SerializeField] float maxLength;
    [SerializeField] float massByScaleDivider;
    Vector2 mousePos;
    Vector2 startPoint;
    Vector2 endPoint;
    public bool draggingPlank;
    [SerializeField] GameObject plankPrefab;
    public GameObject currentPlank;

    public static PlankCreator instance;
    private void Awake()
    {
        if(instance == null) 
            instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        PlankPlacement();
    }
    
    void PlankPlacement()
    {
        GameObject selectedPart = PartSelectionController.instance.selectedPart;
        GameObject selectionPointObj = PartSelectionController.instance.selectionPoint;

        if (Input.GetMouseButtonDown(0) && !draggingPlank)
        {
            PlaceStartPoint(selectedPart, selectionPointObj);
        }

        if (Input.GetMouseButtonDown(1) && draggingPlank)
        {
            ResetPlank();
        }

        if (Input.GetMouseButton(0) && draggingPlank)
        {
            DragPlank(selectedPart, selectionPointObj);
        }

        if (Input.GetMouseButtonUp(0) && draggingPlank)
        {
            PlaceEndPoint(selectedPart, selectionPointObj);
        }
    }

    void PlaceStartPoint(GameObject selectedPart, GameObject selectionPointObj)
    {
        draggingPlank = true;

        
        if (selectedPart != null)
            startPoint = selectionPointObj.transform.position;
        else
            startPoint = mousePos;

        currentPlank = Instantiate(plankPrefab, startPoint, Quaternion.identity);
        currentPlank.name = "Plank";

        if (selectedPart != null)
            currentPlank.GetComponent<Plank>().objAttachedToStart = selectedPart;
    }

    void DragPlank(GameObject selectedPart, GameObject selectionPointObj)
    {
        if (selectedPart != null)
        {
            ScaleToPoint(selectionPointObj.transform.position);
            RotateToPoint(selectionPointObj.transform.position);
        }
        else
        {
            ScaleToPoint(mousePos);
            RotateToPoint(mousePos);
        }
    }

    void PlaceEndPoint(GameObject selectedPart, GameObject selectionPointObj)
    {
        if (currentPlank.transform.localScale.x < minLength)
        {
            ResetPlank();
            return;
        }

        Rigidbody2D plankRB = currentPlank.GetComponent<Rigidbody2D>();
        plankRB.mass = currentPlank.transform.localScale.x / massByScaleDivider;

        if (selectedPart != null)
        {
            endPoint = selectionPointObj.transform.position;
            currentPlank.GetComponent<Plank>().objAttachedToEnd = selectedPart;
        }
        else
        {
            endPoint = mousePos;
        }

        if (currentPlank.TryGetComponent(out Plank plank))
        {
            plank.startPoint = startPoint;
            plank.endPoint = endPoint;

            plank.CheckForAttachedParts();
        }

        currentPlank = null;
        draggingPlank = false;
    }

    void ResetPlank()
    {
        Destroy(currentPlank);
        currentPlank = null;
        draggingPlank = false;
    }

    void RotateToPoint(Vector2 point)
    {
        Vector2 pointDir = (point - startPoint).normalized;
        float angle = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg;
        currentPlank.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void ScaleToPoint(Vector2 point)
    {
        float scaleX = Vector2.Distance(point, startPoint);
        scaleX = Mathf.Clamp(scaleX, 0, maxLength);
        Vector2 pointDir = (point - startPoint).normalized;
        currentPlank.transform.position = startPoint + (pointDir * (scaleX / 2));
        currentPlank.transform.localScale = new Vector2(scaleX, currentPlank.transform.localScale.y);
    }
}
