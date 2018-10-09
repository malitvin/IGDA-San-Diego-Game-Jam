﻿using System.Collections;
using UnityEngine;

public class BetweenWaveState : FlowState
{
    public BetweenWaveState(MonsterGenerator generator, LevelConfig.LevelDef levelDef) : base(generator, levelDef)
    {
 
    }

    public override void Enter()
    {
        base.Enter();
        Singleton.instance.gui.screenFader.StartCoroutine(GoBackToKillingMonsters());
    }

    private IEnumerator GoBackToKillingMonsters()
    {
        yield return new WaitForSeconds(_generator._currentWave.waveCompleteWaitTime);
        _generator.SetFlowState(State.Monster);
    }
}
