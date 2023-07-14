using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PartSelection : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ExtendablePartCreator extendablePartCreator;
    [SerializeField] GamePhase gamePhase;
    [SerializeField] CurrentHeldPart currentHeldPart;

    [SerializeField] float rayRadius;
    [SerializeField] public LayerMask selectionMask;
    [SerializeField] GameObject selectionPointPrefab;

    Vector2 mousePos;
    [NonSerialized] public GameObject selectionPoint;
    [NonSerialized] public GameObject selectedPart;

    public bool selectingPart;

    
    public PartTypes.Type hoveredPartType;

    void Update()
    {
        if (gamePhase.currentPhase != GamePhase.Phase.BUILDING) return;

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
        RaycastHit2D singleHit = Physics2D.Raycast(mousePos, Vector3.back, selectionMask);

        selectedPart = null;
        ResetSelectionPoint();

        if (extendablePartCreator.IsMaxLength()) return;

        float maxDist = Mathf.Infinity;
        foreach (RaycastHit2D rayHit in rayHits)
        {
            if (!ReferenceEquals(rayHit.collider.gameObject, currentHeldPart.part))
            {
                float distFromMouse = Vector2.Distance(mousePos, rayHit.point);
                if (distFromMouse < maxDist)
                {
                    //check if wheel is already connected, if so dont connect to it because that will make that part get rotated by da wheel
                    if(singleHit.collider != null)
                    {
                        //if (rayHit.collider.gameObject.TryGetComponent(out Wheel wheel) && wheel.gameObject.GetComponent<WheelJoint2D>().connectedBody != null) continue;
                    }

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
        CheckMissingParts(selectedPart);
        if (selectedPart.GetComponent<Rope>())
        {
            foreach(Transform ropeSegment in selectedPart.transform.parent)
            {
                CheckMissingParts(ropeSegment.gameObject);
                Destroy(ropeSegment.gameObject);
            }
            Destroy(selectedPart.transform.parent.gameObject);
        }
        Destroy(selectedPart);
    }

    void CheckMissingParts(GameObject partToCheck)
    {
        foreach(Part part in FindObjectsOfType<Part>())
        {
            if (part.gameObject == null) return; //dont need this?

            Joint2D[] joints = part.gameObject.GetComponents<Joint2D>();
            foreach(Joint2D joint in joints)
            {
                if (joint == null) return; //dont need this?

                if (joint.connectedBody != null && joint.connectedBody.gameObject == partToCheck)
                {
                    Destroy(joint);
                }
            }
        }
    }

    void ChangeEnum()
    {
        if (selectedPart == null) 
        {
            hoveredPartType = PartTypes.Type.NULL;
            return;
        }
        hoveredPartType = selectedPart.GetComponent<Part>().partType;
    }
}
