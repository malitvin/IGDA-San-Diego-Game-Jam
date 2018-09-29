//Unity
using UnityEngine;
using Common.Util;

//C#
using System.Collections.Generic;

namespace Gameplay.Inventory
{
    public class InventorySystem : GhostGen.EventDispatcher
    {
        private Dictionary<Inventory.Type, InventoryConfig.InventoryItem> _inventory = 
            new Dictionary<Inventory.Type, InventoryConfig.InventoryItem>(new FastEnumIntEqualityComparer<Inventory.Type>());

        private InventoryConfig _inventoryConfig;

        #region Init
        public InventorySystem(GameConfig gameConfig)
        {
            _inventoryConfig = gameConfig.inventoryConfig;
            InitInventory();
        }

        private void InitInventory()
        {
            InventoryConfig.InventoryItem[] items = _inventoryConfig.inventoryItems;
            for (int i=0; i < items.Length; i++)
            {
                InventoryConfig.InventoryItem item = items[i];
                _inventory[item.key] = item;
            }
        }
        #endregion

        public Dictionary<Inventory.Type, InventoryConfig.InventoryItem> GetInventory()
        {
            return _inventory;
        }

        public InventoryConfig.UISpawnData GetSpawnData()
        {
            return _inventoryConfig.uiSpawnData;
        }
    }

    public class Inventory
    {
        public enum Type
        {
            Coin
        }
    }
}
