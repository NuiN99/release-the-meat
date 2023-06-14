using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggler : MonoBehaviour
{
    [SerializeField] GameObject[] buildingObjects;

    private void OnEnable()
    {
        GamePhase.OnBuilding += EnableBuilding;
        GamePhase.OnLevel += DisableBuilding;
    }
    private void OnDisable()
    {
        GamePhase.OnBuilding -= EnableBuilding;
        GamePhase.OnLevel -= DisableBuilding;
    }

    void EnableBuilding()
    {
        foreach (GameObject obj in buildingObjects) 
        {
            obj.SetActive(true);
        }
    }
    void DisableBuilding()
    {
        foreach (GameObject obj in buildingObjects)
        {
            obj.SetActive(false);
        }
    }
}
