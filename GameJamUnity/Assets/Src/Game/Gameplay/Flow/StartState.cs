using UnityEngine;
using System.Collections;

public class StartState : FlowState {

    public StartState(MonsterGenerator generator, LevelConfig config) : base(generator, config)
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

        yield return new WaitForSeconds(_generator._levelDef.waveStartWaitTime);
        _generator.SetFlowState(State.Monster);
    }

}
