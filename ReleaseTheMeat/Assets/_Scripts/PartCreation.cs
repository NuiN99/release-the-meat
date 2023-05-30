using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCreation : MonoBehaviour
{
    public static PartCreation instance;

    public float plankBreakForce;
    public float rodBreakForce;
    public float wheelBreakForce;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

            if (ExtendablePartCreator.instance.extendingPart)
            {
                ExtendablePartCreator.instance.ResetExtendablePart();
                //PartButtons.instance.selectedPartType = PartSelection.PartType.NULL;
            }

            else if (PartSelection.instance.selectedPart == null)
            {
                if (SimplePartCreator.instance.placingPart)
                {
                    SimplePartCreator.instance.CancelPartPlacement();
                    //PartButtons.instance.selectedPartType = PartSelection.PartType.NULL;
                }

                CurrentHeldPart.instance.part = null;
            }
            else
            {
                PartSelection.instance.DeleteSelectedPart();
            }
        }
    }
}
