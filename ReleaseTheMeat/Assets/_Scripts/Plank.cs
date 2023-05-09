using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    public List<Transform> cellPoints = new List<Transform>();
    public bool isPlaced;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void SetCells()
    {
        foreach(Transform child in transform)
        {
            cellPoints.Add(child);
        }
    }
}
