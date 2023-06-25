using UnityEngine;

public class CartCompletor : MonoBehaviour
{
    private void OnEnable()
    {
        GamePhase.OnLevel += CompleteCart;
    }

    private void OnDisable()
    {
        GamePhase.OnLevel -= CompleteCart;
    }

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
