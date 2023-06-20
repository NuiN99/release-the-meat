using UnityEngine;

public class PartCreation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ExtendablePartCreator extendablePartCreator;
    [SerializeField] SimplePartCreator simplePartCreator;
    [SerializeField] PartSelection partSelection;
    [SerializeField] CurrentHeldPart currentHeldPart;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (extendablePartCreator.extendingPart)
            {
                extendablePartCreator.ResetExtendablePart();
            }

            else if (partSelection.selectedPart == null)
            {
                if (simplePartCreator.placingPart)
                {
                    simplePartCreator.CancelPartPlacement();
                }

                currentHeldPart.part = null;
            }
            else
            {
                partSelection.DeleteSelectedPart();
            }
        }
    }
}
