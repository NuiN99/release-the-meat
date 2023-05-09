using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UI;
using UnityEngine;
using System;

public class MouseSelection : MonoBehaviour
{
    public PartType hoveredPartType;

    Vector2 mousePos;
    [SerializeField] float rayRadius;
    [SerializeField] LayerMask selectionMask;

    
    public enum PartType 
    {
        Nothing, Plank, Wheel
    }
    void Start()
    {
        
    }

    void Update()
    {
        CircleCast();
    }

    void CircleCast()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D selection = Physics2D.CircleCast(mousePos, rayRadius, Vector3.zero, 0, selectionMask);
        if (selection.collider == null)
        {
            hoveredPartType = PartType.Nothing;
            return;
        }
        

        GameObject selectedPart = selection.collider.gameObject;
        //var plankScript = selectedObject.GetComponent<Plank>();

        if (selectedPart.TryGetComponent(out Plank plankScript))
        {
            hoveredPartType = PartType.Plank;
            plankScript.SelectNearestCell(mousePos);
        }
    }

}
