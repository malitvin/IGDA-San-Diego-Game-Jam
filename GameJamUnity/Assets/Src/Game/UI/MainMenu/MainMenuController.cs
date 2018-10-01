using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;


public class MainMenuController : BaseController
{
    private Action<JamStateType> _onComplete;
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
        _onComplete(JamStateType.LOAD_GAMEPLAY);
    }

    private void onShowFTUE(GeneralEvent e)
    {
        mainMenu.ShowFTUE();
    }
    private void onGotoCredits(GeneralEvent e)
    {
        mainMenu.ShowCredits();
    }

    private void onBack(GeneralEvent e)
    {
        mainMenu.ShowMainMenu();
    }

    private void onQuit(GeneralEvent e)
    {
        Application.Quit();
    }

    private MainMenu mainMenu { get { return view as MainMenu; } }
}






