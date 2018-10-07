using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;
using Zenject;
using Gameplay.Building;
using Gameplay.Particles;
using Gameplay.Inventory;
using UI.HUD;

public class GameplayState : IGameState
{
    public enum GameMode
    {
        NONE,

        BUILD,
        COMBAT
    }

    private GameStateMachine<JamStateType> _gameStateMachine;
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
    private MonsterGenerator _monsterGenerator;
    private HUDController _hudController;
    private GameMode _gameMode;

    private PlayerCombatController playerCombatController;

    public GameplayState(
        GameStateMachine<JamStateType> gameStateMachine,
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
        _monsterGenerator = _diContainer.Resolve<MonsterGenerator>();
        
        Singleton.instance.audioSystem.GenerateAudioLookupForLevel();
        _particleGod.InitParticlePool();

        //HUD
        _hudController = new HUDController(_gameConfig.playerConfig);
        _hudController.Start(() =>
        {
            _monsterGenerator.Init(_hudController, _playerCombatSystem);
        });

        _playerCombatSystem.Init(_hudController);
        _buildSystem.Init();
        _enemySystem.Init();

        // Get CombatPlayerView
        //_playerCombatSystem.isEnabled = true;
        _dispatcher.AddListener(GameplayEventType.DAMAGE_TAKEN, onDamageTaken);
        _dispatcher.AddListener(GameplayEventType.GAME_COMPLETE, onGameComplete);
        _dispatcher.AddListener(GameplayEventType.GAME_RETRY, onGameRetry);
    }

    public void Step(float p_deltaTime)
    {
        if (_playerCombatSystem != null)
        {
            _playerCombatSystem.Tick(p_deltaTime);
        }
        if (_buildSystem != null)
        {
            _buildSystem.Tick(p_deltaTime);
        }
        if (_enemySystem != null)
        {
            _enemySystem.Tick(p_deltaTime);
        }
        if (_monsterGenerator != null)
        {
            _monsterGenerator.OnUpdate(p_deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gameMode = _gameMode == GameMode.COMBAT ? GameMode.BUILD : GameMode.COMBAT;
        }

        if (_gameMode == GameMode.COMBAT)
        {
            _playerCombatSystem.isEnabled = true;
            _buildSystem.EnableSystem(true);
        }
        else if (_gameMode == GameMode.BUILD)
        {
            _playerCombatSystem.isEnabled = false;
            _buildSystem.EnableSystem(true);
        }
        else if (_gameMode == GameMode.NONE)
        {
            _playerCombatSystem.isEnabled = false;
            _buildSystem.EnableSystem(false);
        }
    }

    public void FixedStep(float fixedDeltaTime)
    {
        if (_playerCombatSystem != null)
        {
            _playerCombatSystem.FixedTick(fixedDeltaTime);
        }
        if (_buildSystem != null)
        {
            _buildSystem.FixedTick(fixedDeltaTime);
        }
    }

    public void Exit()
    {
        _dispatcher.RemoveListener(GameplayEventType.DAMAGE_TAKEN, onDamageTaken);
        _dispatcher.RemoveListener(GameplayEventType.GAME_COMPLETE, onGameComplete);
        _dispatcher.RemoveListener(GameplayEventType.GAME_RETRY, onGameRetry);

        _hudController.RemoveView();
        _buildSystem.CleanUp();

    }

    private void onDamageTaken(GeneralEvent e)
    {
        DamageResult data = e.data as DamageResult;
        if (data != null)
        {

        }
    }

    private void onGameComplete(GeneralEvent e)
    {
        _gameMode = GameMode.NONE;
    }

    private void onGameRetry(GeneralEvent e)
    {
        _gameStateMachine.ChangeState(JamStateType.MAIN_MENU);
    }

}
