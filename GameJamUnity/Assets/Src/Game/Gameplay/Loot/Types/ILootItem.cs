//Unity
using UnityEngine;

namespace Gameplay.Loot
{
    public interface ILootItem
    {
        void Init(ILootable origin,LootConfig.LootDef def);
        void RefreshColor();
    }
}
