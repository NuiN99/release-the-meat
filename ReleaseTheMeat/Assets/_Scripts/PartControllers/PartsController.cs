using UnityEngine;

public class PartsController : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDisable;

    [SerializeField] GameObject[] parts;


    void OnEnable()
    {
        GamePhase.OnMenu += TurnOffPartsController;
        GamePhase.OnLevel += TurnOffPartsController;
    }

    void OnDisable()
    {
        GamePhase.OnMenu -= TurnOffPartsController;
        GamePhase.OnLevel -= TurnOffPartsController;
    }

    public void TurnOffPartsController()
    {
        gameObject.SetActive(false);
        foreach(var obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }


    public void SaveCart()
    {

    }
}
