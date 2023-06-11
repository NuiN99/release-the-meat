using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Remove the unused using statements

//Where is the namespace? Add a namespace
public class CartCompletor : MonoBehaviour
{
    //This is an example of Action


    /*
    public Action<Part> SendPart;
    List<Part> allParts = new List<Part>();

    private void Start()
    {
        SendPart += SetPart;
    }
    void SetPart(Part part) { 
        allParts.Add(part);
    }
    */

    //With the above code, the script can create a part, then call the action to immediately set the part reference.
    // This completely removes the FindObjectsOfType part

    public void CompleteCart()
    {
        //You can pass these when they are created using an Action delegate
        Part[] parts = FindObjectsOfType<Part>();
        foreach (Part part in parts)
        {
            //You should include null checks here. A cool way to do this is with a TryGet or a try/catch
            // TryGet -> https://medium.com/@MJQuinn/unity-how-to-reference-another-object-cac9f7363e56\
            //Try/Catch -> https://medium.com/@MJQuinn/unity-debugging-part-3-try-catch-eaf2cf59d88
            Rigidbody2D partRB = part.gameObject.GetComponent<Rigidbody2D>();
            partRB.bodyType = RigidbodyType2D.Dynamic;
        }                         
    }
}
