using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//remove unused statements

//namespace
public class CurrentHeldPart : MonoBehaviour
{
    //This does not need to be a singleton
    //This singelton needs to be able to handle GameObjects
    public static CurrentHeldPart instance;

    public GameObject part;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    //Remove this if not needed
    void Update()
    {
        
    }
}
