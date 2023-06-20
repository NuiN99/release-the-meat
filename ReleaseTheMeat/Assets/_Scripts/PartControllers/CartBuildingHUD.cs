using UnityEngine;

public class CartBuildingHUD : MonoBehaviour
{
    public PartSelection.PartType selectedPartType;

    public void SelectPlank()
    {
        selectedPartType = PartSelection.PartType.PLANK;
    }

    public void SelectRod()
    {
        selectedPartType = PartSelection.PartType.ROD;
    }

    public void SelectRope()
    {
        selectedPartType = PartSelection.PartType.ROPE;
    }

    public void SelectWheel()
    {
        selectedPartType = PartSelection.PartType.WHEEL;
    }

    public void DeleteAllParts()
    {
        foreach(Part part in FindObjectsOfType<Part>())
        {
            Destroy(part.gameObject);
        }
    }
}
