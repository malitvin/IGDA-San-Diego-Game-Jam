using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;

public class GameplayState : IGameState
{
    private GameStateMachine<JameStateType> _gameStateMachine;
    private GuiManager _gui;
    private PlayerCombatController playerCombatController;

    public GameplayState(
        GameStateMachine<JameStateType> gameStateMachine,
        GuiManager gui)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
    }

    public void Init(Hashtable changeStateData)
	{

        // Get CombatPlayerView
        playerCombatController = new PlayerCombatController();
    }
    
    public void Step( float p_deltaTime )
	{
        playerCombatController.Tick(p_deltaTime);
    }

    public void Exit( )
	{

        
    }    
    
}
