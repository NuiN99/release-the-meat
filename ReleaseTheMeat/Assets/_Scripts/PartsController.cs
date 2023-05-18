using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsController : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attachable[] parts = FindObjectsOfType<Attachable>();
            foreach(Attachable part in parts)
            {
                Rigidbody2D partRB = part.gameObject.GetComponent<Rigidbody2D>();
                partRB.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
