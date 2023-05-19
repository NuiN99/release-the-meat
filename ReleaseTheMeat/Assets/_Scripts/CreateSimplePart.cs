using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSimplePart : MonoBehaviour
{
    [SerializeField] GameObject wheel;

    void Start()
    {
        
    }

    void Update()
    {
        if (PartSelection.instance.partType != PartSelection.PartType.NULL) return;
        CheckType(PartSelection.instance.partType);
    }

    void CheckType(PartSelection.PartType partType)
    {
        if(partType == PartSelection.PartType.WHEEL)
        {
            CreateWheelPart(wheel);
        }
    }

    void CreateWheelPart(GameObject wheelPrefab)
    {
        GameObject newWheel = Instantiate(wheelPrefab);

    }
}
