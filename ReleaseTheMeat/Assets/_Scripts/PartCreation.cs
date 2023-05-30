using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCreation : MonoBehaviour
{
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
