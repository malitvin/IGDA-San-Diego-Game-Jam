using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;

public class GameplayState : IGameState
{
    private GameStateMachine<JameStateType> _gameStateMachine;
    private GuiManager _gui;
    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;

    private PlayerCombatController playerCombatController;

    public GameplayState(
        GameStateMachine<JameStateType> gameStateMachine,
        GuiManager gui,
        GameConfig gameConfig,
        GameplayResources gameplayResources)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
        _gameConfig = gameConfig;
        _gameplayResources = gameplayResources;
    }

    public void Init(Hashtable changeStateData)
	{
        // Get CombatPlayerView
        PlayerCombatView view = GameObject.Instantiate<PlayerCombatView>(_gameplayResources.playerCombatView);
        playerCombatController = new PlayerCombatController(view, _gameConfig.playerConfig);
    }
    
    public void Step( float p_deltaTime )
	{
        if(playerCombatController != null)
        {
            
            playerCombatController.Tick(p_deltaTime);
        }
    }

    public void FixedStep( float fixedDeltaTime)
    {
        if(playerCombatController != null)
        {
            playerCombatController.FixedTick(fixedDeltaTime);
        }
    }

    public void Exit( )
	{

        
    }    
    
}
