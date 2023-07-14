using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExtendablePart : Part
{
    [Header("Dependencies")]
    [SerializeField] CartBuildingHUD cartBuildingHUD;
    [SerializeField] ExtendablePartCreator extendablePartCreator;

    [Header("Part Creation")]
    public float maxLength;
    public float massByScaleDivider;

    public Vector2 startPoint;
    public Vector2 endPoint;

    public GameObject objAttachedToStart;
    public GameObject objAttachedToEnd;

    [Header("Part Specific Values")]
    [SerializeField] GameObject ropeSegmentPrefab;
    [SerializeField] float springFrequency;
    [SerializeField] float springDampingRatio;

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

        if (partType == PartTypes.Type.ROPE)
        {
            CreateRope();
        }
        if (partType == PartTypes.Type.SPRING)
        {
            CreateSpringJoint(objAttachedToEnd.GetComponent<Rigidbody2D>(), objAttachedToStart.GetComponent<Rigidbody2D>());
        }
    }

    void CheckTypeAndCreateJoint(Rigidbody2D attachedRB, Vector2 point)
    {
        switch (cartBuildingHUD.selectedPartType)
        {
            case PartTypes.Type.PLANK:
                CreateHingeJoint(gameObject, point, attachedRB);
                break;

            case PartTypes.Type.ROD:
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

    void CreateSpringJoint(Rigidbody2D startRB, Rigidbody2D endRB)
    {
        //working on this
        SpringJoint2D springJoint = startRB.gameObject.AddComponent<SpringJoint2D>();

        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.autoConfigureDistance = false;

        springJoint.connectedBody = endRB;
        springJoint.distance = Vector2.Distance(startPoint, endPoint);
        springJoint.dampingRatio = springDampingRatio;
        springJoint.frequency = springFrequency;

        springJoint.anchor = gameObject.transform.InverseTransformPoint(startPoint);
        springJoint.connectedAnchor = endRB.transform.InverseTransformPoint(endPoint);
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

        GameObject objAttachedToEndBefore = objAttachedToEnd;

        GameObject ropeContainer = new GameObject();
        ropeContainer.name = "Rope";
        ropeContainer.transform.parent = FindObjectOfType<CartContainer>().transform;
        ExtendablePart ropeContainerPart = ropeContainer.AddComponent<ExtendablePart>();
        ropeContainerPart.partOfCamera = false;

        float angle = Mathf.Atan2(ropeDir.y, ropeDir.x) * Mathf.Rad2Deg;

        for (float i = 0; i < ropeDist - segmentLength; i += segmentLength)
        {
            segmentCount++;

            if (segmentCount > 1)
                segmentPosition += ropeDir * segmentLength;

            GameObject ropeSegment = Instantiate(ropeSegmentPrefab, segmentPosition, Quaternion.identity);
            ropeSegment.transform.parent = ropeContainer.transform;
            segments.Add(ropeSegment);

            ropeSegment.transform.localScale = new Vector2(segmentLength, ropeSegment.transform.localScale.y);
            ropeSegment.transform.eulerAngles = new Vector3(0, 0, angle);

            ropeSegment.GetComponent<RopeSegment>().parentContainer = ropeContainer;

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

            if(prevSegment.TryGetComponent(out ExtendablePart prevExtendablePart))
            {
                prevExtendablePart.objAttachedToEnd = ropeSegment;
            }
            if(ropeSegment.TryGetComponent(out ExtendablePart curExtendablePart))
            {
                if(prevSegment != null)
                curExtendablePart.objAttachedToStart = prevSegment;
            }
        }

        Vector2 secondLastSegPos = (Vector2)segments[segments.Count - 1].transform.position + (ropeDir * segmentLength / 2);
        Vector2 between = (secondLastSegPos + endPoint) / 2;
        GameObject endSegment = Instantiate(ropeSegmentPrefab, between, Quaternion.identity);
        endSegment.transform.parent = ropeContainer.transform;
        endSegment.transform.localScale = new Vector2(Vector2.Distance(secondLastSegPos, endPoint), endSegment.transform.localScale.y);
        endSegment.transform.eulerAngles = new Vector3(0, 0, angle);

        if(endSegment.TryGetComponent(out RopeSegment endSegmentScript))
        {
            endSegmentScript.parentContainer = ropeContainer;
            endSegmentScript.isEndSegment = true;
        }
        if (segments[0].TryGetComponent(out RopeSegment startSegmentScript))
        {
            startSegmentScript.parentContainer = ropeContainer;
            startSegmentScript.isStartSegment = true;
        }

        segments.Add(endSegment);
        segmentCount++;

        foreach(GameObject segment in segments)
        {
            if (objAttachedToEnd == segment)
            {
                objAttachedToEnd = objAttachedToEndBefore;
            }
        }

        if (endSegment.TryGetComponent(out ExtendablePart endExtendablePart))
        {
            endExtendablePart.startPoint = (Vector2)endSegment.transform.position - (ropeDir * endSegment.transform.localScale.x / 2);
            endExtendablePart.endPoint = (Vector2)endSegment.transform.position + (ropeDir * endSegment.transform.localScale.x / 2);
            endExtendablePart.objAttachedToEnd = objAttachedToEndBefore;
        }

        if (objAttachedToEnd != null && objAttachedToEnd.TryGetComponent(out Rigidbody2D objAttachedToEndRB))
        {
            CreateHingeJoint(endSegment, endPoint, objAttachedToEndRB);
            CreateHingeJoint(segments[segments.Count - 2], secondLastSegPos, endSegment.GetComponent<Rigidbody2D>());
        }
        else if(objAttachedToEnd == null)
        {
            CreateHingeJoint(segments[segments.Count - 2], secondLastSegPos, endSegment.GetComponent<Rigidbody2D>());
        }

        if (segmentCount < 2)
        {
            Destroy(ropeContainer);
            Destroy(gameObject);
            return;
        }
        IgnoreCollisionBetweenRopeSegments(segments);

        Destroy(gameObject);
    }

    void IgnoreCollisionBetweenRopeSegments(List<GameObject> segments)
    {
        if (objAttachedToStart != null)
        {
            foreach (var segment in segments)
            {
                if (segment != objAttachedToStart && segment != objAttachedToEnd)
                {
                    if (segment.TryGetComponent(out Collider2D segmentCollider) && objAttachedToStart.TryGetComponent(out Collider2D objAttachedToStartCollider))
                        Physics2D.IgnoreCollision(segmentCollider, objAttachedToStartCollider);
                }
            }
        }
        if (objAttachedToEnd != null)
        {
            foreach (var segment in segments)
            {
                if (segment != objAttachedToStart && segment != objAttachedToEnd)
                {
                    if (segment.TryGetComponent(out Collider2D segmentCollider) && objAttachedToEnd.TryGetComponent(out Collider2D objAttachedToEndCollider))
                        Physics2D.IgnoreCollision(segmentCollider, objAttachedToEndCollider);
                }
            }
        }
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        int numJoints = 0;
        foreach(Joint2D j in GetComponents<Joint2D>()) numJoints++;

        //if there will be 0 left when this joint breaks
        if(numJoints == 1)
        {
            foreach (Collider2D collider in ignoredColliders)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collider, false);
            }
        }
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
