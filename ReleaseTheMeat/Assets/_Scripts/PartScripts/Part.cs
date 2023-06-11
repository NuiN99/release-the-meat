using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    Color ogColor;
    SpriteRenderer sr;


    private void Awake()
    {
        if(GamePhase.instance.currentPhase == GamePhase.Phase.BUILDING)
        {
            //DontDestroyOnLoad(this);
        }
    }
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ogColor = sr.color;
    }

    void Update()
    {
        //if (GamePhase.instance.currentPhase != GamePhase.Phase.BUILDING) return;

        //ChangeSelectionColor(IsSelected());
    }

    bool IsSelected()
    {
        if (gameObject == PartSelection.instance.selectedPart) return true;
        else return false;
    }

    void ChangeSelectionColor(bool selected)
    {
        if (selected) sr.color = Color.green;
        else sr.color = ogColor;
    }
}
