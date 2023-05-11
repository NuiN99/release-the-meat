using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankCreator : MonoBehaviour
{
    [SerializeField] float maxLength;
    Vector2 mousePos;
    Vector2 startPoint;
    Vector2 endPoint;
    public bool draggingPlank;
    [SerializeField] GameObject plankPrefab;
    public GameObject currentPlank;

    public static PlankCreator instance;
    private void Awake()
    {
        if(instance == null) instance = this;
    }

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
            if(MouseSelection.instance.selectedPart)
            {
                ScaleToPoint(MouseSelection.instance.selectionPoint.transform.position);
                RotateToPoint(MouseSelection.instance.selectionPoint.transform.position);
            }
            else
            {
                ScaleToPoint(mousePos);
                RotateToPoint(mousePos);
            }
        }

        if (Input.GetMouseButtonUp(0) && draggingPlank)
        {
            if (MouseSelection.instance.selectedPart)
            {
                endPoint = MouseSelection.instance.selectionPoint.transform.position;
            }
            else
            {
                endPoint = mousePos;
            }

            if (currentPlank.TryGetComponent(out Attachable attachable))
            {
                attachable.startPoint = startPoint;
                attachable.endPoint = endPoint;
            }
            currentPlank = null;
            draggingPlank = false;
        }
    }
    

    void PlaceStartPoint()
    {
        draggingPlank = true;

        if (MouseSelection.instance.selectedPart)
        {
            startPoint = MouseSelection.instance.selectionPoint.transform.position;
        }
        else
        {
            startPoint = mousePos;
        }

        currentPlank = Instantiate(plankPrefab, startPoint, Quaternion.identity);
        currentPlank.name = "Plank";
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
