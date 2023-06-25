using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class ExtendablePart : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] CartBuildingHUD cartBuildingHUD;

    public Vector2 startPoint;
    public Vector2 endPoint;

    public GameObject objAttachedToStart;
    public GameObject objAttachedToEnd;

    [SerializeField] GameObject ropePrefab; 

    void OnEnable()
    {
        cartBuildingHUD = FindObjectOfType<CartBuildingHUD>();
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

        hingeJoint.anchor = transform.InverseTransformPoint(anchor);
        hingeJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }

    void CreateRope()
    {
        float segmentLength = 0.25f;
        int segmentCount = 0;
        float ropeDist = Vector2.Distance(startPoint, endPoint);
        Vector2 ropeDir = (endPoint - startPoint).normalized;
        Vector2 segmentPosition = startPoint;

        List<GameObject> segments = new List<GameObject> ();
        
        if(objAttachedToStart != null) segments.Add(objAttachedToStart);
        else segments.Add(new GameObject());

        for(float i = 0; i < ropeDist; i += segmentLength)
        {
            segmentCount++;
            segmentPosition += (ropeDir * segmentLength);

            GameObject ropeSegment = Instantiate(ropePrefab, segmentPosition, Quaternion.identity);

            segments.Add(ropeSegment);
            ropeSegment.transform.localScale = new Vector2(segmentLength, ropeSegment.transform.localScale.y);

            GameObject prevSegment = segments[segmentCount - 1];
            Vector2 prevSegmentEndPos = (Vector2)prevSegment.transform.position + (ropeDir * prevSegment.transform.localScale.x / 2);

            Rigidbody2D ropeRB = ropeSegment.GetComponent<Rigidbody2D>();
            CreateHingeJoint(prevSegment, prevSegmentEndPos, ropeRB);
        }
        Destroy(gameObject);
    }

    /*void CreateDistanceJoint(Vector2 anchor, Rigidbody2D body, float breakForce)
    {
        DistanceJoint2D distanceJoint = gameObject.AddComponent<DistanceJoint2D>();

        distanceJoint.autoConfigureDistance = false;
        distanceJoint.distance = 0;

        distanceJoint.breakForce = breakForce;

        distanceJoint.maxDistanceOnly = true;
        distanceJoint.autoConfigureConnectedAnchor = false;

        distanceJoint.connectedBody = body;
        distanceJoint.anchor = transform.InverseTransformPoint(anchor);
        distanceJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }*/
}
