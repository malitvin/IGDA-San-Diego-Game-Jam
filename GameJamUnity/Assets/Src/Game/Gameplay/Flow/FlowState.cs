using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlowState : IFlowState
{
    protected MonsterGenerator _generator;
    protected LevelConfig _levelConfig;

    public enum State
    {
        Monster,
        Start,
        End,
        BetweenWaveState
    }

    public FlowState(MonsterGenerator generator,LevelConfig levelConfig)
    {
        _generator = generator;
        _levelConfig = levelConfig;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
       
    }

    public virtual void OnUpdate()
    {
       
    }

    public virtual void OnMonsterDestroyed()
    {

    }
}
