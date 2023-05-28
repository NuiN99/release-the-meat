using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartButtons : MonoBehaviour
{
    public static PartButtons instance;

    public PartSelection.PartType selectedPartType;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

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

    public void SelectElastic()
    {
        selectedPartType = PartSelection.PartType.ELASTIC;
    }

    public void SelectWheel()
    {
        selectedPartType = PartSelection.PartType.WHEEL;
    }
}
