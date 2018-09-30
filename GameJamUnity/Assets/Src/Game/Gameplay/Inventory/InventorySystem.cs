//Unity
using UnityEngine;
using Common.Util;

//C#
using System.Collections.Generic;

namespace Gameplay.Inventory
{
    public class InventorySystem : GhostGen.EventDispatcher
    {
        private Dictionary<Storeable.Type, InventoryConfig.InventoryItem> _inventoryMap = 
            new Dictionary<Storeable.Type, InventoryConfig.InventoryItem>(new FastEnumIntEqualityComparer<Storeable.Type>());

        private Dictionary<Storeable.Type, int> _inventory =
            new Dictionary<Storeable.Type, int>(new FastEnumIntEqualityComparer<Storeable.Type>());

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
                _inventoryMap[item.key] = item;
                _inventory[item.key] = item.startAmount;
            }
        }
        #endregion

        public Dictionary<Storeable.Type, InventoryConfig.InventoryItem> GetInventoryMap()
        {
            return _inventoryMap;
        }

        public InventoryConfig.UISpawnData GetSpawnData()
        {
            return _inventoryConfig.uiSpawnData;
        }

        public void Buy(Storeable.Type type,int amount)
        {
            _inventory[type] -= amount;
        }

        public bool CanBuy(Storeable.Type type,int cost)
        {
            if(_inventory[type] >= cost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetAmount(Storeable.Type type)
        {
            return _inventory[type];
        }
    }

    public class Storeable
    {
        public enum Type
        {
            Coin
        }
    }
}
