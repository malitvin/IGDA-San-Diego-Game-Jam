﻿using GhostGen;
using UnityEngine;
using Zenject;


[CreateAssetMenu(menuName = "IDGA/Game System Installer")]
public class GameSystemInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerCombatSystem>().AsSingle();
        //Container.Bind<GameState>().AsSingle();
        //Container.Bind<GameTimerManager>().AsSingle();
        //Container.Bind<WaveAISystem>().AsSingle();
        //Container.Bind<CreepViewSystem>().AsSingle();
        //Container.Bind<WaveSpawnerSystem>().AsSingle();
        //Container.Bind<GameSystemManager>().AsSingle();
        //Container.Bind<CreepSystem>().AsSingle();
        //Container.Bind<TowerSystem>().AsSingle();
        //Container.Bind<TowerDictionary>().FromInstance(towerDictionary).AsSingle();
        //Container.Bind<CreepDictionary>().FromInstance(creepDictionary).AsSingle();
        //Container.Bind<CreepHealthUISystem>().FromNewComponentOnNewGameObject().AsSingle();
        //Container.BindFactory<string, TowerSpawnInfo, Tower, Tower.Factory>();
        //Container.BindFactory<string, CreepSpawnInfo, Creep, Creep.Factory>();
        //Container.BindFactory<SyncStepper, SyncStepper.Factory>().WithFactoryArguments<GameObject>(syncCommanderPrefab);

     }
}