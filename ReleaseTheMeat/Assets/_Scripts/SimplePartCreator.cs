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

    public static SimplePartCreator instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
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
        if(!placingPart) return;

        if (Input.GetMouseButtonDown(0))
        {
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
            if(selection == null) return;
            Rigidbody2D selectionRB = selection.GetComponent<Rigidbody2D>();

            switch (PartButtons.instance.partType)
            {
                case PartButtons.PartType.NULL:
                    break;
                case PartButtons.PartType.PLANK:
                    break;
                case PartButtons.PartType.WHEEL:
                    SetWheelJoint(newPart, selectionRB);
                    break;
            }

            CurrentHeldPart.instance.part = null;
        }
    }


    public void SetWheelJoint(GameObject wheel, Rigidbody2D connectedBody)
    {
        if (PartSelection.instance.selectedPart == null) return;
        if (PartSelection.instance.selectedPart.GetComponent<Wheel>()) return;

        WheelJoint2D joint = wheel.GetComponent<WheelJoint2D>();

        joint.connectedBody = connectedBody;
        joint.anchor = wheel.transform.InverseTransformPoint(wheel.transform.position);
        joint.connectedAnchor = connectedBody.transform.InverseTransformPoint(wheel.transform.position);
    }
}
