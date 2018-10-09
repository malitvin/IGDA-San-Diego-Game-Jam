using UnityEngine;
using System.Collections;

public class StartState : FlowState {

    public StartState(MonsterGenerator generator, LevelConfig.LevelDef levelDef) : base(generator, levelDef)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Singleton.instance.gui.screenFader.StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(_levelDef.waveStartWaitTime);
        _generator.SetFlowState(State.Monster);
    }

}
