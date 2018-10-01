using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GhostGen;
using DG.Tweening;
using TMPro;
public class GameCompleteView : UIView
{
    public Button retry;
    public TextMeshProUGUI endBanner;
    public CanvasGroup grid;

    private IEventDispatcher _dispatcher;

    public void OnCreationComplete(bool win)
    {

        grid.alpha = 0;
        grid.DOFade(1, 1);
        endBanner.color = win ? Color.green : Color.red;
        endBanner.text = win ? "WIN!" : "DEFEAT";
        retry.onClick.AddListener(() => RETRY());

        _dispatcher = Singleton.instance.notificationDispatcher;
    }

    private void RETRY()
    {
        _dispatcher.DispatchEvent(GameplayEventType.GAME_RETRY);
    }
}
