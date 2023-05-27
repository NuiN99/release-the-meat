using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UI;
using UnityEngine;
using System;
using System.Net;

public class PartSelection : MonoBehaviour
{
    
    public static PartSelection instance;

    Vector2 mousePos;
    [SerializeField] float rayRadius;
    [SerializeField] LayerMask selectionMask;
    [SerializeField] GameObject selectionPointPrefab;
    public GameObject selectionPoint;
    public GameObject selectedPart;

    bool selectingObject;

    public enum PartType 
    {
        NULL, PLANK, WHEEL
    }
    public PartType hoveredPartType;

    void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    void Update()
    {
        if (GamePhase.instance.currentPhase != GamePhase.Phase.BUILDING) return;

        selectingObject = false;

        ChangeEnum();


        SelectNearestPart();

        if (!selectingObject)
            ResetSelectionPoint();
    }

    void SelectNearestPart()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] rayHits = Physics2D.CircleCastAll(mousePos, rayRadius, Vector3.zero, 0, selectionMask);

        selectedPart = null;
        ResetSelectionPoint();

        float maxDist = Mathf.Infinity;
        foreach(RaycastHit2D rayHit in rayHits)
        {
            if (!ReferenceEquals(rayHit.collider.gameObject, CurrentHeldPart.instance.part))
            {
                float distFromMouse = Vector2.Distance(mousePos, rayHit.point);
                if (distFromMouse < maxDist)
                {
                    maxDist = distFromMouse;
                    selectedPart = rayHit.collider.gameObject;
                }

                if(rayHit.collider.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    Color ogColor = spriteRenderer.color;
                    if (selectedPart != null && rayHit.collider.gameObject != selectedPart)
                    {
                        spriteRenderer.color = ogColor;
                    }
                    else if(selectedPart != null)
                    {
                        spriteRenderer.color = Color.green;
                    }
                }
            }
        }

        if (selectedPart == null) return;
        selectingObject = true;

        if (selectedPart.GetComponent<Plank>())
        {
            SelectPlank();
        }

        else if (selectedPart.GetComponent<SimplePart>())
        {
            SelectSimplePart(selectedPart);
        }
    }

    void ChangeSelectedPartColor()
    {

    }

    void SelectPlank()
    {
        Plank plank = selectedPart.GetComponent<Plank>();
        Vector2 startPoint = plank.startPoint;
        Vector2 endPoint = plank.endPoint;

        selectionPoint = Instantiate(selectionPointPrefab, PlankLengthSelection(startPoint, endPoint, mousePos), Quaternion.identity);
    }

    void SelectSimplePart(GameObject part)
    {
        selectionPoint = Instantiate(selectionPointPrefab, part.transform.position, Quaternion.identity);
    }


    Vector2 PlankLengthSelection(Vector3 startPoint, Vector3 endPoint, Vector3 point)
    {
        var vVector1 = point - startPoint;
        var vVector2 = (endPoint - startPoint).normalized;

        var d = Vector3.Distance(startPoint, endPoint);
        var t = Vector3.Dot(vVector2, vVector1);

        if (t <= 0)
            return startPoint;

        if (t >= d)
            return endPoint;

        var vVector3 = vVector2 * t;

        var vClosestPoint = startPoint + vVector3;

        return vClosestPoint;
    }

    void ResetSelectionPoint()
    {
        Destroy(selectionPoint);
        selectionPoint = null;
    }

    public void DeleteSelectedPart()
    {
        if(selectedPart == null) return;
        //if (PartButtons.instance.partType != PartButtons.PartType.NULL) return;
        Destroy(selectedPart);
    }

    void ChangeEnum()
    {
        if (selectedPart == null) 
        {
            hoveredPartType = PartType.NULL;
            return;
        }
        

        else if (selectedPart.GetComponent<Plank>()) hoveredPartType = PartType.PLANK;
        else if (selectedPart.GetComponent<Wheel>()) hoveredPartType = PartType.WHEEL;
    }
}
