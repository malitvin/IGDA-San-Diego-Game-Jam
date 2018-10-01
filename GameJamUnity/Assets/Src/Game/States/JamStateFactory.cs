using UnityEngine;
using GhostGen;
using Zenject;

public enum JamStateType
{
    NO_STATE = -1,

    INTRO = 1,
    MAIN_MENU,
    LOAD_GAMEPLAY,
    GAMEPLAY,
    CREDITS
}


public class JamStateFactory : ScriptableObjectInstaller, IStateFactory<JamStateType>
{
    public override void InstallBindings()
    {
        Container.Bind<IntroState>().AsTransient();
        Container.Bind<MainMenuState>().AsTransient();
        Container.Bind<LoadGameplayState>().AsTransient();
        Container.Bind<GameplayState>().AsTransient();
    }

    public IGameState CreateState(JamStateType stateId)
    {
        switch (stateId)
        {
            case JamStateType.INTRO:         return Container.Resolve<IntroState>();
            case JamStateType.MAIN_MENU:     return Container.Resolve<MainMenuState>();
            case JamStateType.LOAD_GAMEPLAY: return Container.Resolve<LoadGameplayState>();
            case JamStateType.GAMEPLAY:      return Container.Resolve<GameplayState>();
            case JamStateType.CREDITS:       break;
        }

        Debug.LogError("Error: state ID: " + stateId + " does not exist!");
        return null;
    }
}
