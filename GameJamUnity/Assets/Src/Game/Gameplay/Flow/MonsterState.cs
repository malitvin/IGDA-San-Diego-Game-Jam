//Unity
using UnityEngine;

//Audio
using Audio;

//C#
using System.Collections.Generic;

public class MonsterState : FlowState
{
    private int _currentWaveIndex;

    private int _monstersToDestroyCount;
    private int _monstersGenerated;
    private int _monsterDestroyedCount;

    private float _waveTimer;
    private float _currentGenTime;

    private Queue<LevelConfig.WaveDef> _waveQueue;
    private LevelConfig.WaveDef _currentWave;

    public MonsterState(MonsterGenerator generator, LevelConfig.LevelDef levelDef) : base(generator, levelDef)
    {
        _currentWaveIndex = 0;

        _waveQueue = new Queue<LevelConfig.WaveDef>();
        for(int i=0; i <levelDef.waves.Length; i++)
        {
            _waveQueue.Enqueue(levelDef.waves[i]);
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

        //refresh hud
        _generator._hudController.OnWaveChanged(_currentWaveIndex + 1);

        //set generator current wave
        _generator._currentWave = _currentWave;

        _monstersToDestroyCount = _currentWave.enemyCount;
        _monstersGenerated = 0;
        _waveTimer = 0;
        _currentGenTime = 0;
        _monsterDestroyedCount = 0;
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
