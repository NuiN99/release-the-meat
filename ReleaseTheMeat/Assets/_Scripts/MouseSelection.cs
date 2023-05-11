using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UI;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json.Serialization;

public class MouseSelection : MonoBehaviour
{
    public PartType hoveredPartType;
    public static MouseSelection instance;

    Vector2 mousePos;
    [SerializeField] float rayRadius;
    [SerializeField] LayerMask selectionMask;
    [SerializeField] GameObject selectionPointPrefab;
    public GameObject selectionPoint;
    public GameObject selectedPart;


    bool selectingObject;

    public enum PartType 
    {
        Nothing, Plank, Wheel
    }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        selectingObject = false;
        SelectNearestPlank();

        if(!selectingObject) Destroy(selectionPoint);
    }

    void SelectNearestPlank()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] rayHits = Physics2D.CircleCastAll(mousePos, rayRadius, Vector3.zero, 0, selectionMask);

        selectedPart = null;
        Destroy(selectionPoint);

        float maxDist = Mathf.Infinity;
        foreach(RaycastHit2D rayHit in rayHits)
        {
            if (!ReferenceEquals(rayHit.collider.gameObject, PlankCreator.instance.currentPlank))
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
        selectingObject = true;

        Attachable attachable = selectedPart.GetComponent<Attachable>();
        Vector2 startPoint = attachable.startPoint; 
        Vector2 endPoint = attachable.endPoint;

        selectionPoint = Instantiate(selectionPointPrefab, ClosestPointOnLine(startPoint, endPoint, mousePos), Quaternion.identity);
    }

    Vector2 ClosestPointOnLine(Vector3 startPoint, Vector3 endPoint, Vector3 point)
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


}
