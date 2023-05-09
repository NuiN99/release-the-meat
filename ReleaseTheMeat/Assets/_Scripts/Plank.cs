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

    public void SelectNearestCell(Vector2 mousePos)
    {
        float closestDist = Mathf.Infinity;
        Transform closestCell = null;

        foreach(Transform cell in cellPoints) 
        {
            float distFromMouse = Vector2.Distance(mousePos, cell.position);
            if(distFromMouse < closestDist) 
            {
                closestDist = distFromMouse;
                closestCell = cell;
            }
        }
        if (closestCell == null) return;


        closestCell.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
