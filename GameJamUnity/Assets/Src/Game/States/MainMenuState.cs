using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;

public class MainMenuState : IGameState
{
    private GameStateMachine<JamStateType> _gameStateMachine;
    private GuiManager _gui;
    private MainMenuController _controller;

    public MainMenuState(
        GameStateMachine<JamStateType> gameStateMachine,
        GuiManager gui)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
    }
    
    public void Init(Hashtable changeStateData)
	{
        _controller = new MainMenuController();
        _controller.Start(onViewCreated, onChangeState);
    }

    private void onViewCreated()
    {
        _gui.screenFader.FadeIn();
    }
    
    private void onChangeState(JamStateType type)
    {
        _gameStateMachine.ChangeState(type);
    }

    public void Step( float p_deltaTime )
	{

    }

    public void FixedStep(float fixedDeltaTime)
    {

    }

    public void Exit( )
	{
        if(_controller != null)
        {
            _controller.RemoveView();
        }
    }    
    
}
