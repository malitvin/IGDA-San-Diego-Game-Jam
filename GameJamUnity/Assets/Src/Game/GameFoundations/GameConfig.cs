using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;
using Gameplay.Building;
using Gameplay.Inventory;
using Gameplay.Particles;
using Audio;


[CreateAssetMenu(menuName = "IDGA/Game Config")]
public class GameConfig : ScriptableObject, IPostInit
{
    public JameStateType initialState;
    public PlayerConfig playerConfig;
    public BuildConfig bulidConfig;
    public InventoryConfig inventoryConfig;
    public ParticleConfig particleConfig;
    public EnemyConfig enemyConfig;
    public AudioConfig audioConfig;

    public void PostInit()
    {

    }
}
