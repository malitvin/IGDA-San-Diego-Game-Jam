using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;

public class IntroState : IGameState
{
    private GameStateMachine<JamStateType> _gameStateMachine;
    private GuiManager _gui;

    public IntroState(
        GameStateMachine<JamStateType> gameStateMachine,
        GuiManager gui)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
    }
    
    public void Init(Hashtable changeStateData)
	{
		//Debug.Log ("Entering In Intro State");
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
        // More initialization

        _gameStateMachine.ChangeState(JamStateType.MAIN_MENU);
        _gui.screenFader.FadeIn();
    }
    
    public void Step( float p_deltaTime )
	{

    }

    public void FixedStep(float fixedDeltaTime)
    {

    }

    public void Exit( )
	{
        
    }    
    
}
