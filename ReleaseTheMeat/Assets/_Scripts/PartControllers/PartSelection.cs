using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartSelection : MonoBehaviour
{
    
    public static PartSelection instance;

    Vector2 mousePos;
    [SerializeField] float rayRadius;
    [SerializeField] LayerMask selectionMask;
    [SerializeField] GameObject selectionPointPrefab;
    public GameObject selectionPoint;
    public GameObject selectedPart;

    public bool selectingPart;

    public enum PartType 
    {
        NULL, 
        PLANK,
        ROD,
        ROPE,
        WHEEL,
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

        selectingPart = false;

        ChangeEnum();


        SelectNearestPart();

        if (!selectingPart)
            ResetSelectionPoint();
    }

    void SelectNearestPart()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] rayHits = Physics2D.CircleCastAll(mousePos, rayRadius, Vector3.zero, 0, selectionMask);

        selectedPart = null;

        ResetSelectionPoint();


        if (ExtendablePartCreator.instance.IsMaxLength()) 
        {
            return;
        }

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
            }
        }

        if (selectedPart == null) return;

        selectingPart = true;

        if (selectedPart.GetComponent<ExtendablePart>() )
        {
            SelectExtendablePart();
        }

        else if (selectedPart.GetComponent<SimplePart>())
        {
            SelectSimplePart(selectedPart);
        }
    }

    void SelectExtendablePart()
    {
        ExtendablePart extendablePart = selectedPart.GetComponent<ExtendablePart>();
        Vector2 startPoint = extendablePart.startPoint;
        Vector2 endPoint = extendablePart.endPoint;

        selectionPoint = Instantiate(selectionPointPrefab, LengthSelection(startPoint, endPoint, mousePos), Quaternion.identity);
    }

    void SelectSimplePart(GameObject part)
    {
        selectionPoint = Instantiate(selectionPointPrefab, part.transform.position, Quaternion.identity);
    }


    Vector2 LengthSelection(Vector3 startPoint, Vector3 endPoint, Vector3 point)
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

    public void ResetSelectionPoint()
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
        else if (selectedPart.GetComponent<Rod>()) hoveredPartType = PartType.ROD;
        else if (selectedPart.GetComponent<Rope>()) hoveredPartType = PartType.ROPE;
        else if (selectedPart.GetComponent<Wheel>()) hoveredPartType = PartType.WHEEL;
    }
}
