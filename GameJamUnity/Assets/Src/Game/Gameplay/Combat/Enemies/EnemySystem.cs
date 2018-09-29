using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : GhostGen.EventDispatcher
{
    private List<EnemyController> _enemyList = new List<EnemyController>(20);

    private GameplayResources _gameplayResources;
    private GameConfig _gameConfig;
    private PlayerCombatController _playerController;


    public EnemySystem(
        GameplayResources gameplayResources,
        GameConfig gameConfig,
        PlayerCombatSystem playerCombatSystem)
    {

        _gameplayResources = gameplayResources;
        _gameConfig = gameConfig;
        _playerController = playerCombatSystem.playerController;

        //Create a 
        for(int i = 0; i < 10; ++i)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-12, 12), 0, Random.Range(-12, 12));
            EnemyController enemy = AddEnemy(_playerController.transform, spawnPos);
            enemy.speed = Random.Range(2.3f, 5.0f);
        }
    }
    public EnemyController AddEnemy(Transform target, Vector3 position)
    {
        EnemyView enemyView = GameObject.Instantiate(_gameplayResources.basicEnemyView);
        EnemyController enemy = new EnemyController(enemyView, target);
        _enemyList.Add(enemy);
        return enemy;
    }

    public void Tick(float deltaTime)
    {
        if(_playerController != null)
        {
            for(int i = 0; i < _enemyList.Count; ++i)
            {
                _enemyList[i].RefreshTarget(); // Delay this later
            }
        }
    }
}
