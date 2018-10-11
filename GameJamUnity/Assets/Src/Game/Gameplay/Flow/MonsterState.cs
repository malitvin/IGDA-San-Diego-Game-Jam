//Unity
using UnityEngine;

//Audio
using Audio;

//C#
using System.Collections.Generic;

//UI
using UI.HUD;

public class MonsterState : FlowState
{
    private int _currentWaveIndex;

    private int _monstersToDestroyCount;
    private int _monstersGenerated;
    private int _monsterDestroyedCount;

    private float _waveTimer;
    private float _currentGenTime;

    private int _maxWaves;

    private Queue<LevelConfig.WaveDef> _waveQueue;
    private LevelConfig.WaveDef _currentWave;

    private HUDController _hudController;

    public MonsterState(MonsterGenerator generator, LevelConfig.LevelDef levelDef,HUDController hudController) : base(generator, levelDef)
    {
        _currentWaveIndex = 0;
        _hudController = hudController;

        _waveQueue = new Queue<LevelConfig.WaveDef>();
        _maxWaves = levelDef.waves.Length;
        for (int i=0; i < _maxWaves; i++)
        {
            LevelConfig.WaveDef def = levelDef.waves[i];
            def.waveIndex = i;
            _waveQueue.Enqueue(def);
        }
    }

    public override void Enter()
    {
        base.Enter();

        if(_waveQueue.Count > 0)
        {
            _currentWave = _waveQueue.Dequeue();
            WaveInit();
        }
        else
        {
            _generator.gameWon = true;
            _generator.SetFlowState(State.End);
        }
    }

    private void WaveInit()
    {
        //init sound to let player know monsters are coming
        Singleton.instance.audioSystem.PlaySound(SoundBank.Type.MonsterRoar);

        //set generator current wave
        _generator._currentWave = _currentWave;

        _monstersToDestroyCount = _currentWave.enemyCount;
        _monstersGenerated = 0;
        _waveTimer = 0;
        _currentGenTime = 0;
        _monsterDestroyedCount = 0;

        //refresh hud
        _hudController.OnWaveChanged(_currentWave, _maxWaves);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(_waveTimer >= _currentGenTime && _monstersGenerated < _monstersToDestroyCount)
        {
            _currentGenTime = Random.Range(_currentWave.minSpawnIntervalTime, _currentWave.maxSpawnIntervalTime);
            _monstersGenerated++;
            _generator.GenerateEnemy(); //GENERATE MONSTER
        }
        _waveTimer += Time.deltaTime;
    }

    public override void OnMonsterDestroyed()
    {
        base.OnMonsterDestroyed();
        _monsterDestroyedCount++;

        //refresh hud
        _hudController.OnEnemyCountChanged(_monstersToDestroyCount - _monsterDestroyedCount);

        if (_monsterDestroyedCount >= _monstersToDestroyCount)
        {
            _generator.SetFlowState(State.BetweenWaveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _currentWaveIndex++;
    }
}
