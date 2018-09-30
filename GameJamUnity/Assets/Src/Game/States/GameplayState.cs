using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;
using Zenject;
using Gameplay.Building;
using Gameplay.Particles;
using Gameplay.Inventory;

public class GameplayState : IGameState
{
    public enum GameMode
    {
        NONE,

        BUILD,
        COMBAT
    }

    private GameStateMachine<JameStateType> _gameStateMachine;
    private GuiManager _gui;
    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;
    private DiContainer _diContainer;
    private IEventDispatcher _dispatcher;

    private PlayerCombatSystem _playerCombatSystem;
    private BuildingSystem _buildSystem;
    private InventorySystem _inventorySystem;
    private EnemySystem _enemySystem;
    private ParticleGOD _particleGod;
    private GameMode _gameMode;

    private PlayerCombatController playerCombatController;

    public GameplayState(
        GameStateMachine<JameStateType> gameStateMachine,
        GuiManager gui,
        GameConfig gameConfig,
        DiContainer diContainer,
        GameplayResources gameplayResources,
        [Inject(Id = GameInstaller.GLOBAL_DISPATCHER)]
        IEventDispatcher dispatcher)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
        _gameConfig = gameConfig;
        _gameplayResources = gameplayResources;
        _diContainer = diContainer;
        _dispatcher = dispatcher;

    }

    public void Init(Hashtable changeStateData)
	{
        _gameMode = GameMode.COMBAT;

        _playerCombatSystem = _diContainer.Resolve<PlayerCombatSystem>();
        _buildSystem = _diContainer.Resolve<BuildingSystem>();
        _inventorySystem = _diContainer.Resolve<InventorySystem>();
        _enemySystem = _diContainer.Resolve<EnemySystem>();
        _particleGod = _diContainer.Resolve<ParticleGOD>();

        // Get CombatPlayerView
        //_playerCombatSystem.isEnabled = true;
        _dispatcher.AddListener(GameplayEventType.DAMAGE_TAKEN, onDamageTaken);
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
        if(_enemySystem != null)
        {
            _enemySystem.Tick(p_deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            _gameMode = _gameMode == GameMode.COMBAT ? GameMode.BUILD : GameMode.COMBAT;
        }

        if(_gameMode == GameMode.COMBAT)
        {
            _playerCombatSystem.isEnabled = true;
            _buildSystem.EnableSystem(false);
        }
        else if(_gameMode == GameMode.BUILD)
        {
            _playerCombatSystem.isEnabled = false;
            _buildSystem.EnableSystem(true);
        }
    }

    public void FixedStep( float fixedDeltaTime)
    {
        if(_playerCombatSystem != null)
        {
            _playerCombatSystem.FixedTick(fixedDeltaTime);
        }
        if(_buildSystem != null)
        {
            _buildSystem.FixedTick(fixedDeltaTime);
        }
    }

    public void Exit( )
	{
        _dispatcher.RemoveListener(GameplayEventType.DAMAGE_TAKEN, onDamageTaken);

    }    

    private void onDamageTaken(GeneralEvent e)
    {
        DamageResult data = e.data as DamageResult;
        if(data != null)
        {
            Debug.Log("new health: " + data.newHealth);
            Debug.Log("Damge Taken: " + data.attacker.ToString() + ", " + data.victim.ToString());

        }
    }
    
}
