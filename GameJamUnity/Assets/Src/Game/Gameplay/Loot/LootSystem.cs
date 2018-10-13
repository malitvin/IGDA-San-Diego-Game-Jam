//Unity
using UnityEngine;

//Game
using Common.Pooling;

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

        public LootSystem(GameConfig gameConfig)
        {
            _lootConfig = gameConfig.lootConfig;
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

            _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, OnEnemyDestroyed);
        }

        public void Dispose()
        {
            _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, OnEnemyDestroyed);
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

        }
        #endregion

    }
}