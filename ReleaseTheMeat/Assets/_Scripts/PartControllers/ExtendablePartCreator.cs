using UnityEngine;

public class ExtendablePartCreator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PartSelection partSelection;
    [SerializeField] IsMouseOverUI isMouseOverUI;
    [SerializeField] CurrentHeldPart currentHeldPart;
    [SerializeField] CartBuildingHUD cartBuildingHUD;


    [SerializeField] float minLength;
    [SerializeField] float maxLength;
    [SerializeField] float massByScaleDivider;
    Vector2 mousePos;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public bool extendingPart;

    [SerializeField] float ignoreIntersectionRadius;

    [SerializeField] GameObject plankPrefab;
    [SerializeField] GameObject rodPrefab;
    [SerializeField] GameObject ropePrefab;

    GameObject currentPrefab;

    GameObject currentExtendablePart;

    Vector2 pointDir;
    float scaleX;

    void Update()
    {
        if (!ExtendableIsSelected()) 
        {
            extendingPart = false;
            return;
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        PlankPlacement();
    }

    bool ExtendableIsSelected()
    {
        switch(cartBuildingHUD.selectedPartType) 
        { 
            case PartSelection.PartType.PLANK:
                currentPrefab = plankPrefab;
                return true;

            case PartSelection.PartType.ROD:
                currentPrefab = rodPrefab;
                return true;

            case PartSelection.PartType.ROPE:
                currentPrefab = ropePrefab;
                return true;

            default:
                currentPrefab = null;
                return false;
        }
    }
    
    void PlankPlacement()
    {
        GameObject selectedPart = partSelection.selectedPart;
        GameObject selectionPointObj = partSelection.selectionPoint;

        if (Input.GetMouseButtonDown(0) && !extendingPart)
        {
            PlaceStartPoint(selectedPart, selectionPointObj);
        }

        if (Input.GetMouseButton(0) && extendingPart)
        {
            ExtendPart(selectedPart, selectionPointObj);

            if (IsPartIntersecting())
            {
                currentExtendablePart.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                currentExtendablePart.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (Input.GetMouseButtonUp(0) && extendingPart)
        {
            currentHeldPart.part = null;

            if (IsPartIntersecting())
            {
                ResetExtendablePart();
                return;
            }

            PlaceEndPoint(selectedPart, selectionPointObj);
        }
    }

    void PlaceStartPoint(GameObject selectedPart, GameObject selectionPointObj)
    {
        if (isMouseOverUI.overUI) return;

        extendingPart = true;
        
        if (selectedPart != null)
            startPoint = selectionPointObj.transform.position;
        else
            startPoint = mousePos;

        currentExtendablePart = Instantiate(currentPrefab, startPoint, Quaternion.identity);
        currentExtendablePart.transform.parent = FindObjectOfType<CartContainer>().gameObject.transform;

        currentHeldPart.part = currentExtendablePart;

        if (selectedPart != null)
        {
            currentExtendablePart.GetComponent<ExtendablePart>().objAttachedToStart = selectedPart;
        }
            
    }

    void ExtendPart(GameObject selectedPart, GameObject selectionPointObj)
    {
        if (selectedPart != null && selectionPointObj != null)
        {
            RotateToPoint(selectionPointObj.transform.position);
            ScaleToPoint(selectionPointObj.transform.position);
        }
        else
        {
            RotateToPoint(mousePos);
            ScaleToPoint(mousePos);   
        }
    }

    void PlaceEndPoint(GameObject selectedPart, GameObject selectionPointObj)
    {
        if (currentExtendablePart.transform.localScale.x < minLength)
        {
            ResetExtendablePart();
            return;
        }

        Rigidbody2D extendablePartRB = currentExtendablePart.GetComponent<Rigidbody2D>();
        extendablePartRB.mass = currentExtendablePart.transform.localScale.x * currentExtendablePart.transform.localScale.y / massByScaleDivider;

        if (selectedPart != null && selectionPointObj != null)
        {
            endPoint = selectionPointObj.transform.position;
            currentExtendablePart.GetComponent<ExtendablePart>().objAttachedToEnd = selectedPart;
        }
        else
        {
            endPoint = mousePos;
        }

        if (currentExtendablePart.TryGetComponent(out ExtendablePart extendablePart))
        {
            extendablePart.startPoint = startPoint;
            extendablePart.endPoint = endPoint;

            if (extendablePart.objAttachedToStart != null && extendablePart.objAttachedToStart.TryGetComponent(out SimplePart simplePartStart))
            {
                if (!simplePartStart.attached)
                {
                    if(simplePartStart.TryGetComponent(out Wheel wheel))
                    {
                        extendablePart.objAttachedToStart = null;
                        simplePartStart.SetWheelJoint(extendablePartRB);
                    }
                }
            }
            if (partSelection.selectedPart != null && partSelection.selectedPart.TryGetComponent(out SimplePart simplePartEnd))
            {
                if (!simplePartEnd.attached)
                {
                    if (simplePartEnd.TryGetComponent(out Wheel wheel))
                    {
                        extendablePart.objAttachedToEnd = null;
                        simplePartEnd.SetWheelJoint(extendablePartRB);
                    }
                }
            }
            
            extendablePart.CheckForAttachedParts();
        }

        currentExtendablePart = null;
        extendingPart = false;
    }

    public void ResetExtendablePart()
    {
        Destroy(currentExtendablePart);
        currentExtendablePart = null;
        currentHeldPart.part = null;
        extendingPart = false;
    }

    void RotateToPoint(Vector2 point)
    {
        pointDir = (point - startPoint).normalized;
        float angle = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg;
        currentExtendablePart.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void ScaleToPoint(Vector2 point)
    {
        scaleX = Vector2.Distance(point, startPoint);
        scaleX = Mathf.Clamp(scaleX, 0, maxLength);

        currentExtendablePart.transform.position = startPoint + (pointDir * (scaleX / 2));
        currentExtendablePart.transform.localScale = new Vector2(scaleX, currentExtendablePart.transform.localScale.y);

        IsPartIntersecting();
    }

    bool IsPartIntersecting()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint + (pointDir * scaleX), -pointDir, scaleX);
        
        Debug.DrawRay(startPoint + (pointDir * scaleX), -pointDir * scaleX, Color.green);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == currentExtendablePart)continue;
            if (!hit.collider.gameObject.GetComponent<Part>()) continue;
            if (hit.collider.gameObject == currentExtendablePart.GetComponent<ExtendablePart>().objAttachedToStart) continue;
            if (hit.collider.gameObject == partSelection.selectedPart) continue;
            if (hit.collider.gameObject.GetComponent<Wheel>()) continue;
            if (Vector2.Distance(hit.point, startPoint) <= ignoreIntersectionRadius) continue;
            if (partSelection.selectingPart && Vector2.Distance(hit.point, partSelection.selectionPoint.transform.position) <= ignoreIntersectionRadius) continue;
            if (currentExtendablePart.GetComponent<Rod>() && hit.collider.gameObject.GetComponent<Rod>()) continue;

            if (extendingPart)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsMaxLength()
    {
        if (extendingPart == false) return false;

        if (scaleX >= maxLength)
        {
            return true;
        }

        return false;
    }
}
