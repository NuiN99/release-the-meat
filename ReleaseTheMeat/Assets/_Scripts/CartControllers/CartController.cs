using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public static CartController instance;

    public float plankBreakForce;
    public float rodBreakForce;
    public float wheelBreakForce;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    void Update()
    {
        
    }
}
