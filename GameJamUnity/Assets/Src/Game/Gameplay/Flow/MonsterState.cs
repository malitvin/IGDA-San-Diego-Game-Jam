using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : FlowState {

    private LevelConfig.WaveDef _currentWave;
    private LevelConfig.WaveDef[] _waves;

    private int _currentWaveIndex;
    private int _maxWaveCount;

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
            _currentWave = _waves[_currentWaveIndex];
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(Input.GetKeyDown(KeyCode.V))
        {
            _generator.GenerateEnemy();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _currentWaveIndex++;
    }
}
