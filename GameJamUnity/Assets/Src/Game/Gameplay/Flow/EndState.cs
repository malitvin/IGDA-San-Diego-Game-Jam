using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class EndState : FlowState
{
    private bool gameOver = false;
    public EndState(MonsterGenerator generator,LevelConfig config) : base(generator,config)
    {

    }

    public override void Enter()
    {
        base.Enter();
        if(!gameOver)
        {
            Singleton.instance.audioSystem.PlaySound(SoundBank.Type.Lose);
        }
    }
}
