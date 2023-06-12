using UnityEngine;

public class CurrentHeldPart : MonoBehaviour
{
    public static CurrentHeldPart instance;

    public GameObject part;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
