using UnityEngine;
using UnityEngine.EventSystems;

public class IsMouseOverUI : MonoBehaviour
{
    public bool overUI;

    void Update()
    {
        overUI = EventSystem.current.IsPointerOverGameObject();
    }
}
