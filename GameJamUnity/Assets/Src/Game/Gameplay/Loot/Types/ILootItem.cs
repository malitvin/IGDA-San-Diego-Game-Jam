//Unity
using UnityEngine;

namespace Gameplay.Loot
{
    public interface ILootItem
    {
        void Init(ILootable origin,Transform follow,LootConfig.LootDef def,LootConfig lootConfig, LootConfig.LootItemDef itemDef);
        void RefreshColor();
        void Pickup();
        Vector3 GetPosition();
    }
}
