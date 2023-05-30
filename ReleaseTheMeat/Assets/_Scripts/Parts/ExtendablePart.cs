using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExtendablePart : MonoBehaviour
{

    public Vector2 startPoint;
    public Vector2 endPoint;

    [NonSerialized] public GameObject objAttachedToStart;
    [NonSerialized] public GameObject objAttachedToEnd;



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
    }

    void CheckTypeAndCreateJoint(Rigidbody2D attachedRB, Vector2 point)
    {
        switch (PartButtons.instance.selectedPartType)
        {
            case PartSelection.PartType.PLANK:
                CreateHingeJoint(point, attachedRB);
                CreateDistanceJoint(point, attachedRB);
                break;

            case PartSelection.PartType.ROD:
                CreateHingeJoint(point, attachedRB);
                CreateDistanceJoint(point, attachedRB);
                break;
            case PartSelection.PartType.ROPE:
                CreateHingeJoint(point, attachedRB);
                CreateRope();
                break;
        }
    }

    void CreateHingeJoint(Vector2 anchor, Rigidbody2D body)
    {
        HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.autoConfigureConnectedAnchor = false;
        hingeJoint.connectedBody = body;
        hingeJoint.anchor = transform.InverseTransformPoint(anchor);
        hingeJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }

    void CreateDistanceJoint(Vector2 anchor, Rigidbody2D body)
    {
        DistanceJoint2D distanceJoint = gameObject.AddComponent<DistanceJoint2D>();

        distanceJoint.autoConfigureDistance = false;
        distanceJoint.distance = 0;

        distanceJoint.maxDistanceOnly = true;
        distanceJoint.autoConfigureConnectedAnchor = false;


        distanceJoint.connectedBody = body;
        distanceJoint.anchor = transform.InverseTransformPoint(anchor);
        distanceJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }

    void CreateRope()
    {
        if(objAttachedToStart && objAttachedToEnd)
        {
            Rigidbody2D startRB = objAttachedToStart.GetComponent<Rigidbody2D>();
            Rigidbody2D endRB = objAttachedToEnd.GetComponent<Rigidbody2D>();
            SetRopeValues(startRB, endRB, startPoint, endPoint);
        }
    }

    void SetRopeValues(Rigidbody2D startRB, Rigidbody2D endRB, Vector2 startPoint, Vector2 endPoint)
    {
        DistanceJoint2D distanceJoint = startRB.gameObject.AddComponent<DistanceJoint2D>();

        distanceJoint.autoConfigureDistance = false;
        distanceJoint.distance = transform.localScale.x;

        distanceJoint.maxDistanceOnly = true;
        distanceJoint.autoConfigureConnectedAnchor = false;

        distanceJoint.connectedBody = endRB;
        distanceJoint.anchor = transform.InverseTransformPoint(startPoint);
        distanceJoint.connectedAnchor = endRB.transform.InverseTransformPoint(endPoint);
    }
}
