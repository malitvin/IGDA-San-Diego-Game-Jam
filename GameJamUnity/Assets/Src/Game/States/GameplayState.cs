using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;
using Zenject;
using Gameplay.Building;
using Gameplay.Inventory;

public class GameplayState : IGameState
{
    private GameStateMachine<JameStateType> _gameStateMachine;
    private GuiManager _gui;
    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;
    private DiContainer _diContainer;

    private PlayerCombatSystem _playerCombatSystem;
    private BuildingSystem _buildSystem;
    private InventorySystem _inventorySystem;

    private PlayerCombatController playerCombatController;

    public GameplayState(
        GameStateMachine<JameStateType> gameStateMachine,
        GuiManager gui,
        GameConfig gameConfig,
        DiContainer diContainer,
        GameplayResources gameplayResources)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
        _gameConfig = gameConfig;
        _gameplayResources = gameplayResources;
        _diContainer = diContainer;
    }

    public void Init(Hashtable changeStateData)
	{
        //_playerCombatSystem = _diContainer.Resolve<PlayerCombatSystem>();
        _inventorySystem = _diContainer.Resolve<InventorySystem>();
        _buildSystem = _diContainer.Resolve<BuildingSystem>();
        // Get CombatPlayerView
    }
    
    public void Step( float p_deltaTime )
	{
        if(_playerCombatSystem != null)
        {
            _playerCombatSystem.Tick(p_deltaTime);
        }
        if(_buildSystem != null)
        {
            _buildSystem.Tick(p_deltaTime);
        }
    }

    public void FixedStep( float fixedDeltaTime)
    {
        if(_playerCombatSystem != null)
        {
            _playerCombatSystem.FixedTick(fixedDeltaTime);
        }
    }

    public void Exit( )
	{

        
    }    
    
}
