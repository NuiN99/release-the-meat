using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsController : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDisable;
    public void TurnOffPartsController()
    {
        gameObject.SetActive(false);
        foreach(var obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
