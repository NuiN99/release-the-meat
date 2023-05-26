using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if(gameObject == CurrentHeldPart.instance.part)
        {
            print(gameObject.name);
        }
    }
}
