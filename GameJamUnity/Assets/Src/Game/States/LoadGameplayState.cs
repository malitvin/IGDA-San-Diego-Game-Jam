using UnityEngine;
using System.Collections;
using GhostGen;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadGameplayState : IGameState
{
    private GameStateMachine<JamStateType> _gameStateMachine;
    private GuiManager _gui;
    private AsyncOperation _asyncLoad;

    public LoadGameplayState(
        GameStateMachine<JamStateType> gameStateMachine,
        GuiManager gui)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
    }
    
    public void Init(Hashtable changeStateData)
	{
        _asyncLoad = SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single);
        
    }
    
    public void Step( float p_deltaTime )
	{
        if(_asyncLoad.isDone)
        {
            _gameStateMachine.ChangeState(JamStateType.GAMEPLAY);
        }
    }

    public void FixedStep(float fixedDeltaTime)
    {

    }

    public void Exit( )
	{
        
    }    
    
}
