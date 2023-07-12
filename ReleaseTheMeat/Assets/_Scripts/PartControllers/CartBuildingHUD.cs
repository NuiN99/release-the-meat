using System;
using UnityEngine;

public class CartBuildingHUD : MonoBehaviour
{
    public PartTypes.Type selectedPartType;

    public void SelectPart(SelectPartButton button)
    {
        selectedPartType = button.partType;
        if (selectedPartType == PartTypes.Type.NULL)
            Debug.LogWarning($"{button.name} has a null PartType");
    }

    public void DeleteAllParts()
    {
        foreach(Part part in FindObjectsOfType<Part>())
        {
            Destroy(part.gameObject);
        }
    }
}
