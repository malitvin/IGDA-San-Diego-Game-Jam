using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class EndState : FlowState
{
    private bool gameOver = false;
    private GameCompleteController _gameCompleteController;

    public EndState(MonsterGenerator generator,LevelConfig config) : base(generator,config)
    {

    }

    public override void Enter()
    {
        base.Enter();
        if(!gameOver)
        {
            HandleGameOver();
            gameOver = true;
        }
    }


    private void HandleGameOver()
    {
        Singleton.instance.audioSystem.PlaySound(SoundBank.Type.Lose);
        _gameCompleteController = new GameCompleteController(_generator.gameWon);
        _gameCompleteController.Start(() =>
        {
            _gameCompleteController.RemoveView();
        });
    }
}
