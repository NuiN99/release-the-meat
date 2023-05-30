using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimplePartCreator : MonoBehaviour
{
    
    [SerializeField] GameObject selectedPartObj;
    Sprite selectedPartIcon;

    [SerializeField] Sprite wheelIcon;


    public bool placingPart;

    GameObject currentHeldPart;

    [SerializeField] GameObject wheelPrefab;
    Vector2 placementPos;

    public static SimplePartCreator instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        CheckType(PartButtons.instance.selectedPartType);
        MoveSelectedPartIcon();
        PlacePart();
    }

    void CheckType(PartSelection.PartType partType)
    {
        if (partType != PartSelection.PartType.NULL)
        {
            placingPart = true;
        }
        else if(partType == PartSelection.PartType.NULL)
        {
            placingPart = false;
        }

        SpriteRenderer selectedPartObjSR = selectedPartObj.GetComponent<SpriteRenderer>();

        switch (partType)
        {
            case PartSelection.PartType.NULL:

                currentHeldPart = null;
                placingPart = false;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.PLANK:

                currentHeldPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.ROD:

                currentHeldPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.ROPE:

                currentHeldPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.ELASTIC:

                currentHeldPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.WHEEL:

                currentHeldPart = wheelPrefab;
                placingPart = true;
                selectedPartObjSR.sprite = wheelIcon;

                break;
        }
    }

    void MoveSelectedPartIcon()
    {
        if(PartSelection.instance.selectionPoint == null)
        {
            placementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPartObj.transform.position = placementPos;
        }
        else
        {
            placementPos = PartSelection.instance.selectionPoint.transform.position;
            selectedPartObj.transform.position = placementPos;
        }
    }

    void PlacePart()
    {
        if (placingPart)
        {
            if (IsMouseOverUI.instance.overUI) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (currentHeldPart == null) return;
                GameObject newPart = Instantiate(currentHeldPart, placementPos, Quaternion.identity);
                newPart.name = currentHeldPart.name;

                /*if (PartSelection.instance.selectedPart != null) 
                {
                    if(newPart.GetComponent<SimplePart>() && PartSelection.instance.selectedPart.GetComponent<SimplePart>())
                    {
                        Destroy(newPart);
                        return;
                    }
                }*/

                GameObject selection = PartSelection.instance.selectedPart;
                if (selection != null)
                {
                    Rigidbody2D selectionRB = selection.GetComponent<Rigidbody2D>();

                    switch (PartButtons.instance.selectedPartType)
                    {
                        case PartSelection.PartType.NULL:
                            break;
                        case PartSelection.PartType.PLANK:
                            break;
                        case PartSelection.PartType.ROD:
                            break;
                        case PartSelection.PartType.ROPE:
                            break;
                        case PartSelection.PartType.ELASTIC:
                            break;
                        case PartSelection.PartType.WHEEL:
                            SetWheelJoint(newPart, selectionRB, PartCreation.instance.wheelBreakForce);
                            break;
                    }

                    CurrentHeldPart.instance.part = null;
                }
            }
        }
    }
    
    public void CancelPartPlacement()
    {
        placingPart = false;
        currentHeldPart = null;
    }


    public void SetWheelJoint(GameObject wheel, Rigidbody2D connectedBody, float breakForce)
    {
        if (PartSelection.instance.selectedPart == null) return;
        if (PartSelection.instance.selectedPart.GetComponent<Wheel>()) return;

        WheelJoint2D joint = wheel.GetComponent<WheelJoint2D>();

        joint.breakForce = breakForce;

        joint.connectedBody = connectedBody;
        joint.anchor = wheel.transform.InverseTransformPoint(wheel.transform.position);
        joint.connectedAnchor = connectedBody.transform.InverseTransformPoint(wheel.transform.position);
    }
}
