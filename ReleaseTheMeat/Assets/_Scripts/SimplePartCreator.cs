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

        switch (partType)
        {
            case PartButtons.PartType.NULL:
                currentHeldPart = null;
                selectedPartIcon = null;
                return;

            case PartButtons.PartType.PLANK:
                currentHeldPart = null;
                selectedPartIcon = null;
                return;

            case PartButtons.PartType.WHEEL:
                currentHeldPart = wheelPrefab;
                selectedPartIcon = wheelIcon;
                break;
        }

        selectedPartObj.GetComponent<SpriteRenderer>().sprite = selectedPartIcon;


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
        if(placingPart && Input.GetMouseButtonDown(0))
        {
            GameObject newPart = Instantiate(currentHeldPart, placementPos, Quaternion.identity);
            newPart.name = currentHeldPart.name;
        }
    }
}
