using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Pooling;

public class EnemySystem : GhostGen.EventDispatcher
{
    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;
    private PlayerCombatController _playerController;


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
        _playerController = playerCombatSystem.playerController;

        _enemyDef = _gameConfig.enemyConfig.basicEnemy;

        GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
        if (!pool)
        {
            Debug.LogError("NO SCENE POOL TRANSFORM FOUND IN SCENE");
        }
        _enemyPool = new GenericPooler(pool.transform);
        _enemyPool.InitPool(ENEMY, 0, _gameplayResources.basicEnemyView);

        /*
        for(int i = 0; i < 6; ++i)
        {
            AddEnemy(_playerController.transform, Vector3.zero);
        }
        */
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
        enemyView.transform.position = position;
    }

    public void Tick(float deltaTime)
    {
        if(_playerController != null)
        {
            for(int i = 0; i < _enemyList.Count; ++i)
            {
                _enemyList[i].RefreshTarget(); 
                _enemyList[i].Tick(deltaTime);
            }
        }
    }
}
