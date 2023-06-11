using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//Remove unused statements

//namespace
public class PartButtons : MonoBehaviour
{
    //This doesn't need to be a singleton
    //Singleton needs to be setup for MonoBehaviour
    public static PartButtons instance;

    public PartSelection.PartType selectedPartType;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    //Does this need to be a MonoBehaviour? if you keep this a singleton then this shouldn't be a monoBehaviour. otherwise leave it a MonoBehaviour and remove the singleton
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
}
