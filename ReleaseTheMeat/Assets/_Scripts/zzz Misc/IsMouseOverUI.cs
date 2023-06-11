using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Remove unused statements

//Namespace
public class IsMouseOverUI : MonoBehaviour
{
    //This doesn't need to be a singleton. Singleton needs to be setup to handle GameObjects
    public static IsMouseOverUI instance;

    public bool overUI;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        overUI = EventSystem.current.IsPointerOverGameObject();
    }
}
