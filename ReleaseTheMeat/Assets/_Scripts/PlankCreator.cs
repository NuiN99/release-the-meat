using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCreator : MonoBehaviour
{
    Vector2 mousePos;
    Vector2 startPoint;
    bool draggingPlank;
    [SerializeField] GameObject plankPrefab;
    GameObject currentPlank;

    void Start()
    {
        
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0) && !draggingPlank)
        {
            PlaceStartPoint();
        }

        if (Input.GetMouseButtonDown(1) && draggingPlank)
        {
            Destroy(currentPlank);
            draggingPlank = false;
        }

        if (Input.GetMouseButton(0) && draggingPlank)
        {
            ScaleToMouse();
            RotateToMouse();
        }

        if (Input.GetMouseButtonUp(0) && draggingPlank)
        {
            currentPlank = null;
            draggingPlank = false;
        }


        
    }
    

    void PlaceStartPoint()
    {
        draggingPlank = true;
        startPoint = mousePos;
        currentPlank = Instantiate(plankPrefab, startPoint, Quaternion.identity);
    }

    void RotateToMouse()
    {
        Vector2 mouseDir = (mousePos - startPoint).normalized;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        currentPlank.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void ScaleToMouse()
    {
        float scaleX = Vector2.Distance(mousePos, startPoint);
        Vector2 mouseDir = (mousePos - startPoint).normalized;
        currentPlank.transform.localScale = new Vector2(scaleX, currentPlank.transform.localScale.y);
        currentPlank.transform.position = startPoint + mouseDir * (scaleX / 2);
    }
}
