using UnityEngine;
using GhostGen;
using Zenject;

public enum JameStateType
{
    NO_STATE = -1,

    INTRO = 1,
    MAIN_MENU,
    LOAD_GAMEPLAY,
    GAMEPLAY,
    CREDITS
}


public class JamStateFactory : ScriptableObjectInstaller, IStateFactory<JameStateType>
{
    public override void InstallBindings()
    {
        Container.Bind<IntroState>().AsTransient();
        Container.Bind<GameplayState>().AsTransient();
    }

    public IGameState CreateState(JameStateType stateId)
    {
        switch (stateId)
        {
            case JameStateType.INTRO:                     return Container.Resolve<IntroState>();
            case JameStateType.MAIN_MENU:                 break;
            case JameStateType.LOAD_GAMEPLAY:             break;
            case JameStateType.GAMEPLAY:                  return Container.Resolve<GameplayState>();
            case JameStateType.CREDITS:                   break;
        }

        Debug.LogError("Error: state ID: " + stateId + " does not exist!");
        return null;
    }
}
