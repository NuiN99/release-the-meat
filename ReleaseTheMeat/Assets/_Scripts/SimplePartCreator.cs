using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePartCreator : MonoBehaviour
{
    
    [SerializeField] GameObject selectedPartObj;
    Sprite selectedPartIcon;

    [SerializeField] Sprite wheelIcon;


    bool placingPart;

    GameObject currentHeldPart;

    [SerializeField] GameObject wheelPrefab;
    Vector2 placementPos;

    void Start()
    {
        
    }

    void Update()
    {
        CheckType(PartButtons.instance.partType);
        MoveSelectedPartIcon();
    }

    void CheckType(PartButtons.PartType partType)
    {
        if (partType != PartButtons.PartType.NULL)
        {
            placingPart = true;
        }
        else if(partType == PartButtons.PartType.NULL || partType == PartButtons.PartType.PLANK)
        {
            placingPart = false;
        }

        SpriteRenderer selectedPartObjSR = selectedPartObj.GetComponent<SpriteRenderer>();

        switch (partType)
        {
            case PartButtons.PartType.NULL:

                currentHeldPart = null;
                selectedPartObjSR.sprite = null;

                return;

            case PartButtons.PartType.PLANK:

                currentHeldPart = null;
                selectedPartObjSR.sprite = null;

                return;

            case PartButtons.PartType.WHEEL:

                currentHeldPart = wheelPrefab;
                selectedPartObjSR.sprite = wheelIcon;

                break;
        }


        PlacePart();
    }

    void MoveSelectedPartIcon()
    {
        if(PartCreationSelector.instance.selectionPoint == null)
        {
            placementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPartObj.transform.position = placementPos;
        }
        else
        {
            placementPos = PartCreationSelector.instance.selectionPoint.transform.position;
            selectedPartObj.transform.position = placementPos;
        }
    }

    void PlacePart()
    {
        if(!placingPart) return;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newPart = Instantiate(currentHeldPart, placementPos, Quaternion.identity);
            newPart.name = currentHeldPart.name;

            switch (PartButtons.instance.partType)
            {
                case PartButtons.PartType.NULL:
                    break;
                case PartButtons.PartType.PLANK:
                    break;
                case PartButtons.PartType.WHEEL:
                    SetWheelJoint(newPart);
                    break;
            }
        }
    }


    void SetWheelJoint(GameObject wheel)
    {
        if (PartCreationSelector.instance.selectedPart == null) return;

        WheelJoint2D joint = wheel.GetComponent<WheelJoint2D>();
        Rigidbody2D selectionRB = PartCreationSelector.instance.selectedPart.GetComponent<Rigidbody2D>();

        joint.connectedBody = selectionRB;
        joint.anchor = wheel.transform.InverseTransformPoint(wheel.transform.position);
        joint.connectedAnchor = selectionRB.transform.InverseTransformPoint(wheel.transform.position);
    }
}
