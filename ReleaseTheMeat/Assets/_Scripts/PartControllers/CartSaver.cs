using System.Collections.Generic;
using UnityEngine;

public class CartSaver : MonoBehaviour
{
    [SerializeField] GameObject cartContainer;
    [SerializeField] GameObject newCartContainer;

    private void OnEnable()
    {
        GamePhase.OnLevel += SaveCart;
        GamePhase.OnLevelReset += LoadCart;
    }
    private void OnDisable()
    {
        GamePhase.OnLevel -= SaveCart;
        GamePhase.OnLevelReset -= LoadCart;
    }

    void SaveCart()
    {
        newCartContainer = Instantiate(cartContainer);
        newCartContainer.name = "CartContainer";
        foreach(Part part in FindObjectsOfType<Part>())
        {
            if(part.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
        newCartContainer.SetActive(false);
    }

    void LoadCart()
    {
        Destroy(cartContainer);
        newCartContainer.SetActive(true);
        cartContainer = newCartContainer;
    }
}
