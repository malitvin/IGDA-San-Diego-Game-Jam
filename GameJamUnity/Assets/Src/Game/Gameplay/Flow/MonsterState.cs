using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class MonsterState : FlowState {

    private LevelConfig.WaveDef _currentWave;
    private LevelConfig.WaveDef[] _waves;

    private int _currentWaveIndex;
    private int _maxWaveCount;

    private int _monstersToDestroyCount;
    private int _monstersGenerated;
    private int _monsterDestroyedCount;

    private float _timer;
    private float _currentGenTime;

    public MonsterState(MonsterGenerator generator, LevelConfig config) : base(generator, config)
    {
        _currentWaveIndex = 0;
        _waves = _generator._levelDef.waves;
        _maxWaveCount = _waves.Length;
    }

    public override void Enter()
    {
        base.Enter();
        if(_currentWaveIndex >= _maxWaveCount)
        {
            _generator.SetFlowState(State.End);
        }else
        {
            WaveInit();
        }
    }

    private void WaveInit()
    {
        //init sound to let player know monsters are coming
        Singleton.instance.audioSystem.PlaySound(SoundBank.Type.MonsterRoar);

        //refresh hud
        _generator._hudController.OnWaveChanged(_currentWaveIndex + 1);

        //wave init
        _currentWave = _waves[_currentWaveIndex];
        _generator._currentWave = _currentWave;
        _monsterDestroyedCount = 0;
        _monstersToDestroyCount = _currentWave.enemyCount;
        Debug.Log(_currentWave.enemyCount);
        _timer = 0;
        _currentGenTime = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(_timer >= _currentGenTime && _monstersGenerated < _monstersToDestroyCount)
        {
            Debug.Log("SPAWN " + _monstersGenerated);
            _currentGenTime = Random.Range(_currentWave.minSpawnIntervalTime, _currentWave.maxSpawnIntervalTime);
            _monstersGenerated++;
            _generator.GenerateEnemy(); //GENERATE MONSTER
        }
        _timer += Time.deltaTime;
    }

    public override void OnMonsterDestroyed()
    {
        base.OnMonsterDestroyed();
        _monsterDestroyedCount++;
        if (_monsterDestroyedCount >= _monstersToDestroyCount)
        {
            _generator.SetFlowState(State.BetweenWaveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _currentWaveIndex++;
        _monstersGenerated = 0;
    }
}
