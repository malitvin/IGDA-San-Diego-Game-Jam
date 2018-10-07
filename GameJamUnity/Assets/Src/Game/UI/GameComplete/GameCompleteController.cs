//Unity
using UnityEngine;
using System;

public class GameCompleteController : BaseController
{
    private bool didWin;
    private Action onRetry;

    public GameCompleteController(bool win)
    {
        didWin = win;
    }

    public void Start(Action onComplete)
    {
        onRetry = onComplete;

        viewFactory.CreateAsync<GameCompleteView>("GUI/EndGameView", (v) =>
        {
            view = v;
            view.AddListener("on_retry", onRetryGame);
            OnCreationComplete(didWin);
        });
    }

    private GameCompleteView _gameCompleteView { get { return view as GameCompleteView; } }

    private void OnCreationComplete(bool win)
    {
        _gameCompleteView.OnCreationComplete(win);
    }

    private void onRetryGame(GhostGen.GeneralEvent e)
    {
        view.RemoveListener("on_retry", onRetryGame);
        RemoveView();

        Singleton.instance.notificationDispatcher.DispatchEvent(GameplayEventType.GAME_RETRY);
    }

}
