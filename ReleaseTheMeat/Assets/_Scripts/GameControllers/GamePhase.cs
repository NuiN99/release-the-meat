using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//Remove unused statements

//namesapce
public class GamePhase : MonoBehaviour
{
    //This doesn't need to be a singleton/ This needs to be setup for MonoBehaviour
    public static GamePhase instance;

    //Consider switching to Action types. UnityEvents are not great
    public UnityEvent ChangeToMenuPhase;
    public UnityEvent ChangeToBuildingPhase;
    public UnityEvent ChangeToLevelPhase;

    public Phase currentPhase;

    //This doesn't need to be in the class, and shouldn't be unless it's private
    public enum Phase
    {
        MENU,
        BUILDING,
        LEVEL,
    }

    //Remove this if not using it. It's still called by Unity
    void Start()
    {

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


    //Remove this if not using it
    void Update()
    {
        
    }
}
