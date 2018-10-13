namespace Gameplay.Loot
{
    public interface ILootable
    {
       LootConfig.LootDropDef _lootDropDef { get;}
       UnityEngine.Vector3 position { get; }
    }
}
