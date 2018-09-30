using GhostGen;
using UnityEngine;
using Zenject;
using Gameplay.Particles;


[CreateAssetMenu(menuName = "IDGA/Game Installer")]
public class GameInstaller : ScriptableObjectInstaller
{
    public const string GLOBAL_DISPATCHER = "global_dispatcher";
    
    public GameplayResources gameplayResources;
    public GameConfig gameConfig;

    public override void InstallBindings()
    {
        JamStateFactory gameStateInstaller = Container.Instantiate<JamStateFactory>();
        gameStateInstaller.InstallBindings();
        
        GameSystemInstaller gameSystemsInstaller = Container.Instantiate<GameSystemInstaller>();
        gameSystemsInstaller.InstallBindings();

        Container.Bind<IEventDispatcher>().WithId(GLOBAL_DISPATCHER).To<EventDispatcher>().AsSingle();

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

        //Container.Bind<IStateFactory<JameStateType>>().To<JamStateFactory>().FromInstance(gameStateInstaller).AsSingle();
        Container.Bind<SessionFlags>().AsSingle();
        Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
        Container.Bind<GameplayResources>().FromInstance(gameplayResources).AsSingle();
        Container.BindInterfacesAndSelfTo<GuiManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStateMachine<JameStateType>>().AsSingle().WithArguments(gameStateInstaller);
        Container.BindInterfacesAndSelfTo<NetworkManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<Singleton>().AsSingle();
        Container.Bind<ParticleGOD>().AsSingle();
    }
}
