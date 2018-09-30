using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;
using Gameplay.Building;
using Gameplay.Inventory;


[CreateAssetMenu(menuName = "IDGA/Game Config")]
public class GameConfig : ScriptableObject, IPostInit
{
    public JameStateType initialState;
    public PlayerConfig playerConfig;
    public BuildConfig bulidConfig;
    public InventoryConfig inventoryConfig;
    public EnemyConfig enemyConfig;

    public void PostInit()
    {

    }
}
