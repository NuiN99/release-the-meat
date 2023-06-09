using System.Collections.Generic;
using UnityEngine;

public class SimplePartCreator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PartSelection partSelection;
    [SerializeField] IsMouseOverUI isMouseOverUI;
    [SerializeField] CartBuildingHUD cartBuildingHUD;
    [SerializeField] CurrentHeldPart currentPartScript;


    [SerializeField] GameObject selectedPartObj;

    [SerializeField] Sprite wheelIcon;

    public bool placingPart;

    GameObject currentPart;

    [SerializeField] GameObject wheelPrefab;
    Vector2 placementPos;

    void Update()
    {
        CheckType(cartBuildingHUD.selectedPartType);
        MoveSelectedPartIcon();
        PlacePart();
    }

    void CheckType(PartTypes.Type partType)
    {
        if (partType != PartTypes.Type.NULL)
        {
            placingPart = true;
        }
        else if(partType == PartTypes.Type.NULL)
        {
            placingPart = false;
        }

        SpriteRenderer selectedPartObjSR = selectedPartObj.GetComponent<SpriteRenderer>();

        switch (partType)
        {
            case PartTypes.Type.NULL:

                currentPart = null;
                placingPart = false;
                selectedPartObjSR.sprite = null;

                return;

            case PartTypes.Type.PLANK:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartTypes.Type.ROD:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartTypes.Type.ROPE:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartTypes.Type.SPRING:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;


            case PartTypes.Type.WHEEL:

                currentPart = wheelPrefab;
                placingPart = true;
                selectedPartObjSR.sprite = wheelIcon;

                break;
        }
    }

    void MoveSelectedPartIcon()
    {
        if(partSelection.selectionPoint == null)
        {
            placementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPartObj.transform.position = placementPos;
        }
        else
        {
            placementPos = partSelection.selectionPoint.transform.position;
            selectedPartObj.transform.position = placementPos;
        }
    }

    void PlacePart()
    {
        if (placingPart && Input.GetMouseButtonDown(0))
        {
            if (isMouseOverUI.overUI) return;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.back);
            if (hit && hit.collider.gameObject.CompareTag("Ground")) return;

            if (currentPart == null) return;
            if (partSelection.selectedPart != null && partSelection.selectedPart.GetComponent<SimplePart>()) return;

            GameObject newPart = Instantiate(currentPart, placementPos, Quaternion.identity);
            newPart.name = currentPart.name;
            newPart.transform.parent = FindObjectOfType<CartContainer>().gameObject.transform;

            GameObject selection = partSelection.selectedPart;
            if (selection != null)
            {
                Rigidbody2D selectionRB = selection.GetComponent<Rigidbody2D>();

                switch (cartBuildingHUD.selectedPartType)
                {
                    case PartTypes.Type.NULL:
                        break;
                    case PartTypes.Type.PLANK:
                        break;
                    case PartTypes.Type.ROD:
                        break;
                    case PartTypes.Type.ROPE:
                        break;
                    case PartTypes.Type.WHEEL:
                        newPart.GetComponent<SimplePart>().SetWheelJoint(selectionRB);
                        break;
                }

                currentPartScript.part = null;
            }
        }
    }
    
    public void CancelPartPlacement()
    {
        placingPart = false;
        currentPart = null;
    }

    bool IsOverlapping()
    {
        if(currentPart == null) return false;

        Collider2D currentCollider = currentPart.GetComponent<Collider2D>();

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        Collider2D[] results = new Collider2D[1];

        if (currentCollider.OverlapCollider(contactFilter, results) > 0)
        {
            Debug.Log("Overlap detected!");
            return true;
        }
        else
        {
            return false;
        }
    }
}
