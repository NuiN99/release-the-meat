using System;
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
                CreateHingeJoint(point, attachedRB, CartController.instance.plankBreakForce);
                //CreateDistanceJoint(point, attachedRB, CartController.instance.plankBreakForce);
                break;

            case PartSelection.PartType.ROD:
                CreateHingeJoint(point, attachedRB, CartController.instance.rodBreakForce);
                //CreateDistanceJoint(point, attachedRB, CartController.instance.rodBreakForce);
                break;
        }
    }

    void CreateHingeJoint(Vector2 anchor, Rigidbody2D body, float breakForce)
    {
        HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();

        hingeJoint.autoConfigureConnectedAnchor = false;

        hingeJoint.connectedBody = body;

        hingeJoint.breakForce = breakForce;

        hingeJoint.anchor = transform.InverseTransformPoint(anchor);
        hingeJoint.connectedAnchor = body.transform.InverseTransformPoint(anchor);
    }

    void CreateDistanceJoint(Vector2 anchor, Rigidbody2D body, float breakForce)
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
    }
}
