//Unity
using UnityEngine;

namespace Gameplay.Inventory
{
    [CreateAssetMenu(menuName = "IDGA/Inventory Config")]
    public class InventoryConfig : ScriptableObject
    {
        [System.Serializable]
        public class InventoryItem
        {
            public string displayName;
            public Storeable.Type key;
            public int startAmount;
            public Sprite inventoryUISprite;
        }

        [System.Serializable]
        public class UISpawnData
        {
            public BuildCurrencyItem _buildItem;
        }

        public InventoryItem[] inventoryItems;

        public UISpawnData uiSpawnData;
    }
}
