using Common.Pooling;
using System.Collections.Generic;
using UnityEngine;

using Gameplay.Combat.Enemies;

public class EnemySystem : GhostGen.EventDispatcher
{
    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;

    private GhostGen.IEventDispatcher _dispatcher;

    private GenericPooler _enemyPool;
    private List<EnemyController> _enemyList = new List<EnemyController>();
    private Dictionary<int, EnemyController> _lookup = new Dictionary<int, EnemyController>();

    private EnemyDef _enemyDef;

    private static string ENEMY = "ENEMY";

    public EnemySystem(
        GameplayResources gameplayResources,
        GameConfig gameConfig,
        PlayerCombatSystem playerCombatSystem)
    {

        _gameplayResources = gameplayResources;
        _gameConfig = gameConfig;

        _enemyDef = _gameConfig.enemyConfig.basicEnemy;

        _dispatcher = Singleton.instance.notificationDispatcher;
        _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, OnEnemyDestroyed);

    }

    public void Init()
    {
        GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
        if (!pool)
        {
            Debug.LogError("NO SCENE POOL TRANSFORM FOUND IN SCENE");
        }
        _enemyPool = new GenericPooler(pool.transform);
        _enemyPool.InitPool(ENEMY, 0, _gameplayResources.basicEnemyView);
    }

    public void AddEnemy(Transform target, Vector3 position)
    {
        EnemyView enemyView = _enemyPool.GetPooledObject(ENEMY) as EnemyView;
        int id = enemyView.gameObject.GetInstanceID();
        EnemyController enemy;
        if (!_lookup.ContainsKey(id))
        {
            enemy = new EnemyController(_enemyDef, enemyView, target);

            _enemyList.Add(enemy);
            _lookup.Add(id, enemy);
        }
        else
        {
            enemy = _lookup[id];
        }

        enemy.Init();
        enemy.position = position;

        _enemyList.Sort(EnemyUtil.CompareEnemiesByHealth);
    }

    #region Events
    private void OnEnemyDestroyed(GhostGen.GeneralEvent e)
    {
        _enemyList.Sort(EnemyUtil.CompareEnemiesByHealth);
    }
    #endregion

    public void Tick(float deltaTime)
    {
        int enemyCount = _enemyList.Count;
        int i = 0;
        for (i = 0; i < enemyCount; i++)
        {
            EnemyController enemy = _enemyList[i];
            if (!enemy.isDead)
            {
                _enemyList[i].RefreshTarget();
                _enemyList[i].Tick(deltaTime);
            }
            else
            {
                //Enemy List should be sorted by alive enemies first
                break;
            }
        }
    }
}
