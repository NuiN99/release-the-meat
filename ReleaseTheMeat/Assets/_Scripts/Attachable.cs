using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
    public Vector2 startPoint;
    public Vector2 endPoint;

    public GameObject objAttachedToStart;
    public GameObject objAttachedToEnd;

    HingeJoint2D startHinge;
    HingeJoint2D endHinge;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void CheckForAttachedParts()
    {
        if(objAttachedToStart != null)
        {
            Rigidbody2D objAttachedToStartRB = objAttachedToStart.GetComponent<Rigidbody2D>();
            SetHingeJointValues(startHinge, startPoint, objAttachedToStartRB);
        }

        if(objAttachedToEnd != null)
        {
            Rigidbody2D objAttachedToEndRB = objAttachedToEnd.GetComponent<Rigidbody2D>();
            SetHingeJointValues(endHinge, endPoint, objAttachedToEndRB);
        }
    }

    void SetHingeJointValues(HingeJoint2D hingeJoint, Vector2 anchor, Rigidbody2D body)
    {
        hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.autoConfigureConnectedAnchor = false;
        hingeJoint.connectedBody = body;
        hingeJoint.anchor = transform.InverseTransformPoint(anchor);
        hingeJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }
}
