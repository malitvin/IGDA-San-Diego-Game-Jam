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
            public Inventory.Type key;
            public int startAmount;
        }

        public InventoryItem[] inventoryItems;
    }
}
