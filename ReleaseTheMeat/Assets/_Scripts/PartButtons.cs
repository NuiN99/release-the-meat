using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartButtons : MonoBehaviour
{
    public static PartButtons instance;

    public enum PartType
    {
        NULL, 
        PLANK, 
        WHEEL,
    }
    public PartType partType;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void SelectPart(string partID)
    {
        switch (partID)
        {
            case "PLANK":
                partType = PartType.PLANK;
                break;
            case "WHEEL":
                partType = PartType.WHEEL;
                break;
        }
    }
}
