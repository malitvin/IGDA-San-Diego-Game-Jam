//Unity
using UnityEngine;
using Common.Util;

//Game
using GhostGen;

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
        private EnemyConfig _enemyConfig;

        private IEventDispatcher _dispatcher;

        #region Init
        public InventorySystem(GameConfig gameConfig)
        {
            _inventoryConfig = gameConfig.inventoryConfig;
            _enemyConfig = gameConfig.enemyConfig;
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

            // TODO: Klean it up!
            _dispatcher = Singleton.instance.notificationDispatcher;
            _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, onEnemyKilled);
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

        private void onEnemyKilled(GhostGen.GeneralEvent e)
        {
            int rewardCoins = _enemyConfig.basicEnemy.rewardsForKill;
            AddItem(Storeable.Type.Coin, rewardCoins);
            int totalCoins = GetAmount(Storeable.Type.Coin);
            _dispatcher.DispatchEvent(GameplayEventType.ITEM_PICKED_UP, false, totalCoins);
        }
    }

    public class Storeable
    {
        public enum Type
        {
            Coin,
            HealthPotion
        }
    }
}
