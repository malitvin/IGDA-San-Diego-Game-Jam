using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GhostGen;

public class MainMenu : UIView
{
    public class EventType
    {
        public const string START_GAME = "start_game";
        public const string SHOW_FTUE = "show_ftue";
        public const string CREDITS = "credits";
        public const string BACK = "back";
        public const string QUIT = "quit";
    }

    public Button _startButton;
    public Button _showFTUEButton;
    public Button _creditButton;
    public Button _quitButton;
    public Button _creditsBackButton;

    public CanvasGroup _viewGroup;

    public CanvasGroup _mainPanel;
    public CanvasGroup _ftuePanel;
    public CanvasGroup _creditPanel;
    
	// Use this for initialization
	void Awake ()
    {
        _startButton.onClick.AddListener(onStartGame);
        _showFTUEButton.onClick.AddListener(onShowFTUE);
        _creditButton.onClick.AddListener(onGotoCredits);
        _creditsBackButton.onClick.AddListener(onCreditsBack);
        _quitButton.onClick.AddListener(onQuit);
    }

    public override void OnViewDispose()
    {
        base.OnViewDispose();

        _startButton.onClick.RemoveListener(onStartGame);
        _showFTUEButton.onClick.RemoveListener(onShowFTUE);
        _creditButton.onClick.RemoveListener(onGotoCredits);
        _creditsBackButton.onClick.RemoveListener(onCreditsBack);
        _quitButton.onClick.RemoveListener(onQuit);
    }

    public void ShowMainMenu()
    {
        _mainPanel.gameObject.SetActive(true);
        _ftuePanel.gameObject.SetActive(false);
        _creditPanel.gameObject.SetActive(false);
    }

    public void ShowFTUE()
    {

        _mainPanel.gameObject.SetActive(false);
        _ftuePanel.gameObject.SetActive(true);
        _creditPanel.gameObject.SetActive(false);
    }

    public void ShowCredits()
    {
        _mainPanel.gameObject.SetActive(false);
        _ftuePanel.gameObject.SetActive(false);
        _creditPanel.gameObject.SetActive(true);
    }


    private void onStartGame()
    {
        DispatchEvent(EventType.START_GAME);
    }

    private void onGotoCredits()
    {
        DispatchEvent(EventType.CREDITS);
    }

    private void onCreditsBack()
    {
        DispatchEvent(EventType.BACK);
    }

    private void onShowFTUE()
    {
        DispatchEvent(EventType.SHOW_FTUE);
    }

    private void onQuit()
    {
        DispatchEvent(EventType.QUIT);
    }
}
