using UnityEngine;
using UnityEngine.Events;

public class GamePhase : MonoBehaviour
{
    public delegate void ChangedToMenu();
    public static event ChangedToMenu OnMenu;

    public delegate void ChangedToBuilding();
    public static event ChangedToBuilding OnBuilding;


    public delegate void ChangedToLevel();
    public static event ChangedToBuilding OnLevel;

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
                if (OnMenu != null) OnMenu();
                break;

            case Phase.BUILDING:
                if (OnBuilding != null) OnBuilding();
                break;

            case Phase.LEVEL:
                if (OnLevel != null) OnLevel();
                break;
        }
    }

    public void GoToMenu()
    {
        ChangePhase(Phase.MENU);
    }

    public void GoToBuilding()
    {
        ChangePhase(Phase.BUILDING);
    }

    public void GoToLevel()
    {
        ChangePhase(Phase.LEVEL);
    }
}
