using UnityEngine;
using GhostGen;
using Gameplay.Building;
using Gameplay.Inventory;
using Gameplay.Particles;
using Gameplay.Loot;
using UI.HUD;
using Audio;


[CreateAssetMenu(menuName = "IDGA/Game Config")]
public class GameConfig : ScriptableObject, IPostInit
{
    public JamStateType initialState;
    public PlayerConfig playerConfig;
    public BuildConfig bulidConfig;
    public InventoryConfig inventoryConfig;
    public ParticleConfig particleConfig;
    public EnemyConfig enemyConfig;
    public AudioConfig audioConfig;
    public LevelConfig levelConfig;
    public LootConfig lootConfig;

    [Header("UI")]
    public HUDConfig hudConfig;

    public void PostInit()
    {

    }
}
