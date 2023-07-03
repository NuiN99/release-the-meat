using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class ExtendablePart : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] CartBuildingHUD cartBuildingHUD;
    [SerializeField] ExtendablePartCreator extendablePartCreator;

    public Vector2 startPoint;
    public Vector2 endPoint;

    public GameObject objAttachedToStart;
    public GameObject objAttachedToEnd;

    [SerializeField] GameObject ropePrefab;

    float segmentLength;

    void OnEnable()
    {
        cartBuildingHUD = FindObjectOfType<CartBuildingHUD>();
        extendablePartCreator = FindObjectOfType<ExtendablePartCreator>();
    }

    void Start()
    {
        segmentLength = extendablePartCreator.ropeSegmentLength;
    }

    public void CheckForAttachedParts()
    {
        if (objAttachedToStart)
        {
            Rigidbody2D objAttachedToStartRB = objAttachedToStart.GetComponent<Rigidbody2D>();

            CheckTypeAndCreateJoint(objAttachedToStartRB, startPoint);
        }
        if (objAttachedToEnd)
        {
            Rigidbody2D objAttachedToEndRB = objAttachedToEnd.GetComponent<Rigidbody2D>();

            CheckTypeAndCreateJoint(objAttachedToEndRB, endPoint);
        }

        if (GetComponent<Rope>())
        {
            CreateRope();
        }
    }

    void CheckTypeAndCreateJoint(Rigidbody2D attachedRB, Vector2 point)
    {
        switch (cartBuildingHUD.selectedPartType)
        {
            case PartSelection.PartType.PLANK:
                CreateHingeJoint(gameObject, point, attachedRB);
                break;

            case PartSelection.PartType.ROD:
                CreateHingeJoint(gameObject, point, attachedRB);
                break;
        }
    }

    void CreateHingeJoint(GameObject parentObject, Vector2 anchor, Rigidbody2D body)
    {
        HingeJoint2D hingeJoint = parentObject.AddComponent<HingeJoint2D>();

        hingeJoint.autoConfigureConnectedAnchor = false;

        hingeJoint.connectedBody = body;

        hingeJoint.anchor = parentObject.transform.InverseTransformPoint(anchor);
        hingeJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }

    void CreateRope()
    {
        int segmentCount = 0;
        float ropeDist = Vector2.Distance(startPoint, endPoint);
        Vector2 ropeDir = (endPoint - startPoint).normalized;
        Vector2 segmentPosition = startPoint + (ropeDir * segmentLength / 2);

        List<GameObject> segments = new List<GameObject> ();
        
        if(objAttachedToStart != null) segments.Add(objAttachedToStart);
        else
        {
            segments.Add(gameObject);
        }

        GameObject ropeContainer = new GameObject();
        ropeContainer.name = "Rope";
        ropeContainer.transform.parent = FindObjectOfType<CartContainer>().transform;

        float angle = Mathf.Atan2(ropeDir.y, ropeDir.x) * Mathf.Rad2Deg;

        for (float i = 0; i < ropeDist - segmentLength; i += segmentLength)
        {
            segmentCount++;

            if (segmentCount > 1)
                segmentPosition += ropeDir * segmentLength;

            GameObject ropeSegment = Instantiate(ropePrefab, segmentPosition, Quaternion.identity);
            ropeSegment.transform.parent = ropeContainer.transform;
            segments.Add(ropeSegment);

            ropeSegment.transform.localScale = new Vector2(segmentLength, ropeSegment.transform.localScale.y);
            ropeSegment.transform.eulerAngles = new Vector3(0, 0, angle);

            if (ropeSegment.TryGetComponent(out ExtendablePart extendablePart))
            {
                extendablePart.startPoint = (Vector2)ropeSegment.transform.position - (ropeDir * ropeSegment.transform.localScale.x / 2);
                extendablePart.endPoint = (Vector2)ropeSegment.transform.position + (ropeDir * ropeSegment.transform.localScale.x / 2);
            }

            GameObject prevSegment = segments[segmentCount - 1];
            Vector2 prevSegmentEndPos = (Vector2)prevSegment.transform.position + (ropeDir * segmentLength / 2);

            if (segmentCount == 1 && objAttachedToStart != null)
            {
                prevSegment = objAttachedToStart;
                prevSegmentEndPos = (Vector2)ropeSegment.transform.position - (ropeDir * segmentLength / 2);
            }

            Rigidbody2D ropeRB = ropeSegment.GetComponent<Rigidbody2D>();

            if(prevSegment != gameObject)
            CreateHingeJoint(prevSegment, prevSegmentEndPos, ropeRB);
        }

        if(objAttachedToStart == null && segmentCount <= 1)
        {
            Destroy(ropeContainer);
            Destroy(gameObject);
            return;
        }
        if (objAttachedToStart != null && segmentCount <= 2)
        {
            Destroy(ropeContainer);
            Destroy(gameObject);
            return;
        }
        Vector2 secondLastSegPos = (Vector2)segments[segments.Count - 1].transform.position + (ropeDir * segmentLength / 2);
        Vector2 between = (secondLastSegPos + endPoint) / 2;
        GameObject endSegment = Instantiate(ropePrefab, between, Quaternion.identity);
        endSegment.transform.parent = ropeContainer.transform;
        endSegment.transform.localScale = new Vector2(Vector2.Distance(secondLastSegPos, endPoint), endSegment.transform.localScale.y);
        endSegment.transform.eulerAngles = new Vector3(0, 0, angle);
        segments.Add(endSegment);

        if (endSegment.TryGetComponent(out ExtendablePart endExtendablePart))
        {
            endExtendablePart.startPoint = (Vector2)endSegment.transform.position - (ropeDir * endSegment.transform.localScale.x / 2);
            endExtendablePart.endPoint = (Vector2)endSegment.transform.position + (ropeDir * endSegment.transform.localScale.x / 2);
        }

        if (objAttachedToEnd != null && objAttachedToEnd.TryGetComponent(out Rigidbody2D objAttachedToEndRB))
        {
            CreateHingeJoint(segments[segments.Count - 1], endPoint, objAttachedToEndRB);
            CreateHingeJoint(segments[segments.Count - 2], secondLastSegPos, endSegment.GetComponent<Rigidbody2D>());
        }
        else
        {
            CreateHingeJoint(segments[segments.Count - 2], secondLastSegPos, endSegment.GetComponent<Rigidbody2D>());
        }
        Destroy(gameObject);
    }

    /*void CreateDistanceJoint(GameObject parentObject, Vector2 anchor, Rigidbody2D body, float distance)
    {
        DistanceJoint2D distanceJoint = parentObject.AddComponent<DistanceJoint2D>();

        distanceJoint.autoConfigureDistance = false;
        distanceJoint.distance = distance;

        distanceJoint.maxDistanceOnly = true;
        distanceJoint.autoConfigureConnectedAnchor = false;

        distanceJoint.connectedBody = body;
        distanceJoint.anchor = parentObject.transform.InverseTransformPoint(anchor);
        distanceJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }*/
}
