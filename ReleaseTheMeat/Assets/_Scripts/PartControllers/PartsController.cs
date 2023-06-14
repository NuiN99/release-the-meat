using System.Collections.Generic;
using UnityEngine;

public class PartsController : MonoBehaviour
{
    [SerializeField] List<GameObject> parts = new List<GameObject>();
    [SerializeField] GameObject partContainer;
    [SerializeField] GameObject newPartContainer;

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
        newPartContainer = Instantiate(partContainer);
        newPartContainer.SetActive(false);
    }

    void LoadCart()
    {
        Destroy(partContainer);
        newPartContainer.SetActive(true);
        partContainer = newPartContainer;
    }
}
