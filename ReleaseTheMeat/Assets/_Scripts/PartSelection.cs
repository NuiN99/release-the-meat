using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartSelection : MonoBehaviour
{
    public enum PartType
    {
        NULL, PLANK, WHEEL
    }
    public PartType partType;

    public static PartSelection instance;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attachable[] parts = FindObjectsOfType<Attachable>();
            foreach(Attachable part in parts)
            {
                Rigidbody2D partRB = part.gameObject.GetComponent<Rigidbody2D>();
                partRB.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
