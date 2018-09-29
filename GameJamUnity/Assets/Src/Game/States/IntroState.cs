using UnityEngine;
using System.Collections;
using GhostGen;
using DG.Tweening;

public class IntroState : IGameState
{
    private GameStateMachine<JameStateType> _gameStateMachine;
    private NetworkManager _networkManager;
    private GuiManager _gui;

    public IntroState(
        NetworkManager networkManager,
        GameStateMachine<JameStateType> gameStateMachine,
        GuiManager gui)
    {
        _gameStateMachine = gameStateMachine;
        _gui = gui;
    }
    
    public void Init(Hashtable changeStateData)
	{
		Debug.Log ("Entering In Intro State");
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);

        Singleton.instance.networkManager.StartCoroutine(delayedCall(0.5f));

    }
    
    public void Step( float p_deltaTime )
	{

    }

    public void Exit( )
	{
		Debug.Log ("Exiting In Intro State");
        
    }    

    private IEnumerator delayedCall(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _gui.screenFader.FadeIn();
    }
}
