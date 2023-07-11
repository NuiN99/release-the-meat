using UnityEngine;

public class CartBuildingHUD : MonoBehaviour
{
    public PartTypes.Type selectedPartType;

    public void SelectPlank()
    {
        selectedPartType = PartTypes.Type.PLANK;
    }

    public void SelectRod()
    {
        selectedPartType = PartTypes.Type.ROD;
    }

    public void SelectRope()
    {
        selectedPartType = PartTypes.Type.ROPE;
    }

    public void SelectWheel()
    {
        selectedPartType = PartTypes.Type.WHEEL;
    }

    public void DeleteAllParts()
    {
        foreach(Part part in FindObjectsOfType<Part>())
        {
            Destroy(part.gameObject);
        }
    }
}
