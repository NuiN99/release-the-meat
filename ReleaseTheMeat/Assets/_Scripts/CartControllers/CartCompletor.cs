using UnityEngine;

public class CartCompletor : MonoBehaviour
{
    public void CompleteCart()
    {
        Part[] parts = FindObjectsOfType<Part>();
        foreach (Part part in parts)
        {
            Rigidbody2D partRB = part.gameObject.GetComponent<Rigidbody2D>();
            partRB.bodyType = RigidbodyType2D.Dynamic;
        }                         
    }
}
