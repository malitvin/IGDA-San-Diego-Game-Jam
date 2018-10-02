using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;


public class MainMenuController : BaseController
{
    private ScreenFader _fader;
    private Action<JamStateType> _onComplete;
    private const float FADE_TIME = 0.25f;

    public MainMenuController()
    {
        _fader = Singleton.instance.gui.screenFader;
    }

    public void Start(Action onViewCreated, Action<JamStateType> onComplete)
    {
        _onComplete = onComplete;
        viewFactory.CreateAsync<MainMenu>("GUI/MainMenu", (v) =>
        {
            view = v;
            view.AddListener(MainMenu.EventType.START_GAME, onStartGame);
            view.AddListener(MainMenu.EventType.SHOW_FTUE, onShowFTUE);
            view.AddListener(MainMenu.EventType.CREDITS, onGotoCredits);
            view.AddListener(MainMenu.EventType.BACK, onBack);
            view.AddListener(MainMenu.EventType.QUIT, onQuit);

            mainMenu.ShowMainMenu();

            onViewCreated();
        });
 
    }

    private void onStartGame(GeneralEvent e)
    {
        _fader.FadeOut(FADE_TIME, () =>
        {
            _onComplete(JamStateType.LOAD_GAMEPLAY);
        });
    }

    private void onShowFTUE(GeneralEvent e)
    {
        _fader.FadeOut(FADE_TIME, () =>
        {
            mainMenu.ShowFTUE();
            _fader.FadeIn();
        });
    
    }
    private void onGotoCredits(GeneralEvent e)
    {
        _fader.FadeOut(FADE_TIME, () =>
        {
            mainMenu.ShowCredits();
            _fader.FadeIn();
        });
    }

    private void onBack(GeneralEvent e)
    {
        _fader.FadeOut(FADE_TIME, () =>
        {
            mainMenu.ShowMainMenu();
            _fader.FadeIn();
        });
    }

    private void onQuit(GeneralEvent e)
    {
        Application.Quit();
    }

    private MainMenu mainMenu { get { return view as MainMenu; } }
}






