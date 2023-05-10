using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UI;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class MouseSelection : MonoBehaviour
{
    public PartType hoveredPartType;

    Vector2 mousePos;
    [SerializeField] float rayRadius;
    [SerializeField] LayerMask selectionMask;
    [SerializeField] GameObject selectionPointPrefab;
    GameObject selectionPoint;
    GameObject selectedPart;

    bool selectingObject;

    public enum PartType 
    {
        Nothing, Plank, Wheel
    }
    void Start()
    {
        
    }

    void Update()
    {
        selectingObject = false;
        CircleCast();

        if(!selectingObject) Destroy(selectionPoint);
    }

    void CircleCast()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D selection = Physics2D.CircleCast(mousePos, rayRadius, Vector3.zero, 0, selectionMask);

        if (selection.collider == null) return;
        if (PlankCreator.instance.draggingPlank) return;
        selectingObject = true;
        selectedPart = selection.collider.gameObject;

        Destroy(selectionPoint);

        selectionPoint = Instantiate(selectionPointPrefab, mousePos, Quaternion.identity);

        float selectionX = Mathf.Clamp(mousePos.x, selectedPart.transform.position.x - (selectedPart.transform.localScale.x / 2), selectedPart.transform.position.x + (selectedPart.transform.localScale.x / 2));
        float selectionY = Mathf.Clamp(mousePos.y, selectedPart.transform.position.y, selectedPart.transform.position.y);
        selectionPoint.transform.position = new Vector2(selectionX, selectionY);
    }

}
