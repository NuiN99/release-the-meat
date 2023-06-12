using UnityEngine;
using UnityEngine.EventSystems;

public class IsMouseOverUI : MonoBehaviour
{
    public static IsMouseOverUI instance;

    public bool overUI;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        overUI = EventSystem.current.IsPointerOverGameObject();
    }
}
