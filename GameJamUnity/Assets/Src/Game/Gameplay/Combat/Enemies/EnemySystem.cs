using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Pooling;

public class EnemySystem : GhostGen.EventDispatcher
{
    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;

    private GhostGen.IEventDispatcher _dispatcher;

    private GenericPooler _enemyPool;
    private List<EnemyController> _enemyList = new List<EnemyController>();
    private Dictionary<int, EnemyController> _lookup = new Dictionary<int, EnemyController>();

    private LinkedList<EnemyController> _liveEnemies;
    private Dictionary<int, LinkedListNode<EnemyController>> _liveEnemyLookup;

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

        _liveEnemies = new LinkedList<EnemyController>();
        _liveEnemyLookup = new Dictionary<int, LinkedListNode<EnemyController>>();
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

        LinkedListNode<EnemyController> liveNode = _liveEnemies.AddFirst(enemy);
        _liveEnemyLookup[id] = liveNode;
    }

    #region Events
    private void OnEnemyDestroyed(GhostGen.GeneralEvent e)
    {
        EnemyController enemy = e.data as EnemyController;
        if (enemy != null)
        {
            int id = enemy.instanceID;
            if (_liveEnemyLookup.ContainsKey(id))
            {
                LinkedListNode<EnemyController> enemyNode = _liveEnemyLookup[id];
                _liveEnemies.Remove(enemyNode);
                _liveEnemyLookup.Remove(id);
            }
            else
            {
                Debug.LogWarning("Somehow an enemy was destroyed but not in the live enemy lookup");
            }
        }
    }
    #endregion

    public void Tick(float deltaTime)
    {
        LinkedListNode<EnemyController> currentEnemyNode = _liveEnemies.First;
        EnemyController currentEnemyNodeData = null;
        while (currentEnemyNode != null)
        {
            currentEnemyNodeData = currentEnemyNode.Value;
            if (currentEnemyNodeData != null)
            {
                currentEnemyNodeData.RefreshTarget();
                currentEnemyNodeData.Tick(deltaTime);
            }
            currentEnemyNode = currentEnemyNode.Next;
        }
    }
}
