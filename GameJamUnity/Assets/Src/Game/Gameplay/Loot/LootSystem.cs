//Unity
using UnityEngine;

//Game
using Gameplay.Inventory;
using Common.Pooling;

//C#
using System;
using System.Collections.Generic;

namespace Gameplay.Loot
{
    public class LootSystem : GhostGen.EventDispatcher
    {
        //Loot Pool
        private GenericPooler _lootPool;

        //Config
        private LootConfig _lootConfig;

        //dispatcher
        private GhostGen.IEventDispatcher _dispatcher;

        private PlayerCombatSystem _playerCombatSystem;

        public LootSystem(GameConfig gameConfig, PlayerCombatSystem playerCombatSystem)
        {
            _lootConfig = gameConfig.lootConfig;
            _playerCombatSystem = playerCombatSystem;
            _dispatcher = Singleton.instance.notificationDispatcher;
        }

        public void Init()
        {
            //Init Loot Pooler
            GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
            if (!pool)
            {
                Debug.LogError("NO SCENE POOL TRANSFORM FOUND IN SCENE");
            }
            _lootPool = new GenericPooler(pool ? pool.transform : null);

            
            List<LootConfig.LootDef> lootDefs = _lootConfig.lootDefs;
            for (int i=0; i < lootDefs.Count; i++)
            {
                LootConfig.LootDef def = lootDefs[i];
                List<LootConfig.LootItemDef> items = def.lootItems;
                int warmAmount = def.pooledWarmAmount;
                for (int j=0; j < items.Count; j++)
                {
                    LootConfig.LootItemDef item = items[j];
                    string type = Storeable.GetCachedStoreableKey(item.type);
                    _lootPool.InitPool(type, warmAmount, item.prefab);
                }
            }

            _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, OnEnemyDestroyed);
        }

        public void Dispose()
        {
            _dispatcher.RemoveListener(GameplayEventType.ENEMY_KILLED, OnEnemyDestroyed);
        }

        #region Events
        private void OnEnemyDestroyed(GhostGen.GeneralEvent e)
        {
            ILootable lootable = e.data as ILootable;
            if (lootable != null)
            {
                GenerateLoot(lootable);
            }
        }
        #endregion

        #region Loot Generation
        private void GenerateLoot(ILootable lootable)
        {
            LootConfig.LootDropDef lootDropDef = lootable._lootDropDef;

            List<LootConfig.ProbabilityDef> probabilities = lootDropDef.probabilities;
            probabilities.Sort(LootUtil.CompareLootByProbability);

            int lootToDrop = LootUtil.GetLootDropCount(lootDropDef);
            float totalSum = LootUtil.GetTotalSum(probabilities);

            for(int i=0; i < lootToDrop; i++)
            {
                Loot.Rarity rarity = LootUtil.GetWeightedRarity(probabilities, totalSum);
                LootConfig.LootDef lootDef = _lootConfig.GetLootDef(rarity);
                int bucket = UnityEngine.Random.Range(0,lootDef.lootItems.Count);
                LootConfig.LootItemDef itemDef = lootDef.lootItems[bucket];

                string key = Storeable.GetCachedStoreableKey(itemDef.type);
                ILootItem item = _lootPool.GetPooledObject(key).GetComponent<ILootItem>();
                if (item != null)
                {
                    item.Init(lootable,_playerCombatSystem.playerController.transform, lootDef,_lootConfig,itemDef);
                }else
                {
                    Debug.LogError("Item of type " + key + " does not implement  ILootItem");
                }

            }
        }
        #endregion

    }
}