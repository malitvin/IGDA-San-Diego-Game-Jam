using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;


[CreateAssetMenu(menuName = "IDGA/Game Config")]
public class GameConfig : ScriptableObject, IPostInit
{
    public JameStateType initialState;
    public PlayerConfig playerConfig;

    public void PostInit()
    {

    }
}
