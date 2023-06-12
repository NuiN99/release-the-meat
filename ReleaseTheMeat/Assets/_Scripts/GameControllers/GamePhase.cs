using UnityEngine;
using UnityEngine.Events;

public class GamePhase : MonoBehaviour
{
    public static GamePhase instance;

    public UnityEvent ChangeToMenuPhase;
    public UnityEvent ChangeToBuildingPhase;
    public UnityEvent ChangeToLevelPhase;

    public Phase currentPhase;

    public enum Phase
    {
        MENU,
        BUILDING,
        LEVEL,
    }

    void ChangePhase(Phase phase)
    {
        currentPhase = phase;
        switch (currentPhase)
        {
            case Phase.MENU:
                ChangeToMenuPhase.Invoke();
                break;

            case Phase.BUILDING:
                ChangeToBuildingPhase.Invoke();
                break;

            case Phase.LEVEL:
                ChangeToLevelPhase.Invoke();
                break;
        }
    }

    public void GoToBuilding()
    {
        ChangePhase(Phase.BUILDING);
    }

    public void GoToLevel()
    {
        ChangePhase(Phase.LEVEL);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
