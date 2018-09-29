using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;


[CreateAssetMenu(menuName = "IDGA/Game Config")]
public class GameConfig : ScriptableObject, IPostInit
{
    public JameStateType initialState;

    public GuiManager guiManager;
    //public GameplayResources gameplayResources;

    public void PostInit()
    {

    }
}
