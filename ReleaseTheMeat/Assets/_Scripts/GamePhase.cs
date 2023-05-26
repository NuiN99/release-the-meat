using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePhase : MonoBehaviour
{
    public static GamePhase instance;

    public UnityEvent ChangeToMenuPhase;
    public UnityEvent ChangeToBuildingPhase;
    public UnityEvent ChangeToLevelPhase;

    public CurrentPhase currentPhase;

    public enum CurrentPhase
    {
        MENU,
        BUILDING,
        LEVEL,
    }

    void Start()
    {

    }

    void ChangePhase(CurrentPhase phase)
    {
        currentPhase = phase;
        switch (currentPhase)
        {
            case CurrentPhase.MENU:
                ChangeToMenuPhase.Invoke();
                break;

            case CurrentPhase.BUILDING:
                ChangeToBuildingPhase.Invoke();
                break;

            case CurrentPhase.LEVEL:
                ChangeToLevelPhase.Invoke();
                break;
        }
    }

    public void GoToBuilding()
    {
        ChangePhase(CurrentPhase.BUILDING);
    }

    public void GoToLevel()
    {
        ChangePhase(CurrentPhase.LEVEL);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        
    }
}
