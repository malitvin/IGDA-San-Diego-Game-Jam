using UnityEngine;
using System.Collections;

public class StartState : FlowState {

    public StartState(MonsterGenerator generator, LevelConfig config) : base(generator, config)
    {
        Singleton.instance.gui.screenFader.StartCoroutine(StartGame());
    }

    public override void Enter()
    {
        base.Enter();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_generator._levelDef.waveStartWaitTime);
        _generator.SetFlowState(State.Monster);
    }

}
