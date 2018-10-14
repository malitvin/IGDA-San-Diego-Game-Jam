//Unity
using UnityEngine;
using Common.Util;

//Game
using Gameplay.Loot;
using GhostGen;

//C#
using System;
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

        private IEventDispatcher _dispatcher;

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

            _dispatcher = Singleton.instance.notificationDispatcher;
        }

        public void Init()
        {
            _dispatcher.AddListener(GameplayEventType.LOOT_PICKED_UP, onLootPickedUp);
        }

        public void Dispose()
        {
            _dispatcher.RemoveListener(GameplayEventType.LOOT_PICKED_UP, onLootPickedUp);
        }
        #endregion

        #region Events
        private void onLootPickedUp(GhostGen.GeneralEvent e)
        {
            LootConfig.LootItemDef itemDef = e.data as LootConfig.LootItemDef;
            if(itemDef != null)
            {
                AddItem(itemDef.type, itemDef.GetQuantitiy());
                _dispatcher.DispatchEvent(GameplayEventType.INVENTORY_UPDATED,false,itemDef.type);
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

        public void AddItem(Storeable.Type type,int amount)
        {
            int currentAmount = _inventory[type];
            _inventory[type] = currentAmount + amount;
        }

        public void RemoveItem(Storeable.Type type,int amount)
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
            Coin = 0,
            HealthPotion = 1
        }

        private static Dictionary<Type, string> sStoreableLookup;

        public static string GetCachedStoreableKey(Type type)
        {
            if(sStoreableLookup == null)
            {
                sStoreableLookup = new Dictionary<Type, string>();
                var storeAbleTypes = Enum.GetValues(typeof(Type));
                foreach (Type storeable in storeAbleTypes)
                {
                    sStoreableLookup[storeable] = storeable.ToString();
                }
            }

            return sStoreableLookup[type];
        }
    }
}
