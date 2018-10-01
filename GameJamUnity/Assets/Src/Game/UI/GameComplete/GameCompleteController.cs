//Unity
using UnityEngine;
using System;

public class GameCompleteController : BaseController
{
    public GameCompleteController(bool win,Action onComplete)
    {
        viewFactory.CreateAsync<GameCompleteView>("GUI/EndGameView", (v) =>
        {
            view = v;
            OnCreationComplete(win);
            if (onComplete != null)
            {
                onComplete();
            }
        });
    }

    private GameCompleteView _gameCompleteView { get { return view as GameCompleteView; } }

    private void OnCreationComplete(bool win)
    {
        _gameCompleteView.OnCreationComplete(win);
    }

}
