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
        if (PartButtons.instance.partType != PartButtons.PartType.NULL) return;
        CheckType(PartButtons.instance.partType);
    }

    void CheckType(PartButtons.PartType partType)
    {
        if(partType == PartButtons.PartType.WHEEL)
        {
            CreateWheelPart(wheel);
        }
    }

    void CreateWheelPart(GameObject wheelPrefab)
    {
        GameObject newWheel = Instantiate(wheelPrefab);

    }
}
