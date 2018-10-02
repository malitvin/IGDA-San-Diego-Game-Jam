using UnityEngine;
using GhostGen;
using Common.Util;
using Zenject;
using UI.HUD;

using System.Collections.Generic;

public class MonsterGenerator : EventDispatcher
{
    private EnemySystem _enemySystem;
    private PlayerCombatSystem _playerSystem;
    private LevelConfig _levelConfig;
    private IFlowState _currentFlowState;

    public LevelConfig.LevelDef _levelDef;

    public LevelConfig.WaveDef _currentWave;
    
    private IEventDispatcher _dispatcher;

    public HUDController _hudController;

    public bool gameWon = false;

    private Dictionary<FlowState.State, IFlowState> _stateMachine = new Dictionary<FlowState.State, IFlowState>(new FastEnumIntEqualityComparer<FlowState.State>());

    public MonsterGenerator(GameConfig gameConfig,EnemySystem enemySystem)
    {
        _dispatcher = Singleton.instance.notificationDispatcher;
        // TODO: Klean it up!
        _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, onEnemyKilled);
        _dispatcher.AddListener(GameplayEventType.GAME_COMPLETE, OnGameComplete);

        _levelConfig = gameConfig.levelConfig;
        _enemySystem = enemySystem;

        if (_levelConfig.levels.Count == 0)
        {
            Debug.LogError("NO LEVELS LOADED IN LEVEL CONFIG!");
        }else
        {
            _levelDef = _levelConfig.levels[0];
        }
    }

    public void Init(HUDController hudController, PlayerCombatSystem playerSystem)
    {
        _hudController = hudController;
        _playerSystem = playerSystem;

        InitMonsterStateMachine();
    }

    #region State Machine
    private void InitMonsterStateMachine()
    {
        _stateMachine[FlowState.State.Start] = new StartState(this,_levelConfig);
        _stateMachine[FlowState.State.Monster] = new MonsterState(this, _levelConfig);
        _stateMachine[FlowState.State.End] = new EndState(this, _levelConfig);
        _stateMachine[FlowState.State.BetweenWaveState] = new BetweenWaveState(this, _levelConfig);
        //set to start state
        SetFlowState(FlowState.State.Start);
    }

    public void SetFlowState(FlowState.State state)
    {
        Debug.Log("NEW MONSTER GENERATOR STATE " + state.ToString());
        if (_currentFlowState != null)
        {
            _currentFlowState.Exit();
        }

        if (_stateMachine.ContainsKey(state))
        {
            _currentFlowState = _stateMachine[state];
            _currentFlowState.Enter();
        }
        else
        {
            Debug.LogError("NO flow state: " + state + " exists in flow state map");
        }
    }

    public void GenerateEnemy()
    {
        //get angle around island then get direction of  angle and spawn in that direction by the spawn distance
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 directionalPosition = new Vector3( Mathf.Cos(angle), 0, Mathf.Sin(angle));
        directionalPosition *= _levelDef.spawnDistance;

        int randomGen = Random.Range(0, 4);
        Transform targetPoint = randomGen > 2 ? _playerSystem.playerController.transform : _playerSystem.parentController.transform;
        _enemySystem.AddEnemy(targetPoint, directionalPosition);

    }
    #endregion

    public void OnUpdate(float deltaTime)
    {
        if(_currentFlowState != null)
        {
            _currentFlowState.OnUpdate();
        }
    }

    private void onEnemyKilled(GhostGen.GeneralEvent e)
    {
        _currentFlowState.OnMonsterDestroyed();
    }

    private void OnGameComplete(GhostGen.GeneralEvent e)
    {
        bool win = (bool)e.data;
        _currentFlowState.OnGameOver();
    }
}
