using UnityEngine;

public class SimplePartCreator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PartSelection partSelection;
    [SerializeField] IsMouseOverUI isMouseOverUI;
    [SerializeField] PartButtons partButtons;
    [SerializeField] CurrentHeldPart currentPartScript;


    [SerializeField] GameObject selectedPartObj;

    [SerializeField] Sprite wheelIcon;

    public bool placingPart;

    GameObject currentPart;

    [SerializeField] GameObject wheelPrefab;
    Vector2 placementPos;

    

    void Update()
    {
        CheckType(partButtons.selectedPartType);
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

                currentPart = null;
                placingPart = false;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.PLANK:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.ROD:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;

            case PartSelection.PartType.ROPE:

                currentPart = null;
                placingPart = true;
                selectedPartObjSR.sprite = null;

                return;


            case PartSelection.PartType.WHEEL:

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
        if (placingPart)
        {
            if (isMouseOverUI.overUI) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (currentPart == null) return;
                GameObject newPart = Instantiate(currentPart, placementPos, Quaternion.identity);
                newPart.name = currentPart.name;

                /*if (PartSelection.instance.selectedPart != null) 
                {
                    if(newPart.GetComponent<SimplePart>() && PartSelection.instance.selectedPart.GetComponent<SimplePart>())
                    {
                        Destroy(newPart);
                        return;
                    }
                }*/

                GameObject selection = partSelection.selectedPart;
                if (selection != null)
                {
                    Rigidbody2D selectionRB = selection.GetComponent<Rigidbody2D>();

                    switch (partButtons.selectedPartType)
                    {
                        case PartSelection.PartType.NULL:
                            break;
                        case PartSelection.PartType.PLANK:
                            break;
                        case PartSelection.PartType.ROD:
                            break;
                        case PartSelection.PartType.ROPE:
                            break;
                        case PartSelection.PartType.WHEEL:
                            newPart.GetComponent<SimplePart>().SetWheelJoint(selectionRB);
                            break;
                    }

                    currentPartScript.part = null;
                }
            }
        }
    }
    
    public void CancelPartPlacement()
    {
        placingPart = false;
        currentPart = null;
    }


    private void OnEnable()
    {
        GamePhase.OnLevel += DisablePartIcon;   
    }

    private void OnDisable()
    {
        GamePhase.OnLevel -= DisablePartIcon;
    }

    void DisablePartIcon()
    {
        selectedPartObj.SetActive(false);
    }
}
