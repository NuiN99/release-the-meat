using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Remove unused using statements

//namespace
public class PartCreation : MonoBehaviour
{
    //This class can grab refernces to other objects and send the part references out
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
