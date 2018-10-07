//Unity
using UnityEngine;

//C#
using System;

//Game
using Common.Pooling;
using UI.Building;
using Gameplay.Inventory;
using Audio;

namespace Gameplay.Building
{
    public class BuildingSystem : GhostGen.EventDispatcher
    {
        //is build mode enabled
        private bool _buildModeEnabled = false;
        private float buildTimer;
        private bool _builderRecharging;

        //Components
        private GameplayCamera _gameplayCam;
        private GameBoard _gameBoard;
        private BuildHologram _buildHologram;

        public const float ABOVE_SEA_LEVEL = 0.5f;
        private Vector3 seaLevelPos = new Vector3(0, ABOVE_SEA_LEVEL, 0);

        //cached build hit results
        private RaycastHit[] _hitResults = new RaycastHit[5];

        private GenericPooler _buildingGenerator;
        public Buildable.TYPE _currentBuildType;
        private bool _collisionDetectionEnabled;
        private float _buildRechargeRate;

        //build config
        private BuildConfig _buildConfig;
        private BuildConfig.BuildableBlueprint[] _buildables;

        //UI
        private InventorySystem _inventorySystem;
        private BuildViewController _buildViewController;

        //cam
        private BuildCam _buildCam;

        private GhostGen.IEventDispatcher _dispatcher;

        #region Init
        public BuildingSystem(GameConfig gameConfig, InventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _buildConfig = gameConfig.bulidConfig;
            _buildables = _buildConfig.buildables;
            
            // TODO: Klean it up!
            _dispatcher = Singleton.instance.notificationDispatcher;
            _dispatcher.AddListener(GameplayEventType.ENEMY_KILLED, onItemPickedUp);
        }

        public void Init()
        {
            //init building pool
            _gameplayCam = GameObject.FindObjectOfType<GameplayCamera>();
            _gameBoard = GameObject.FindObjectOfType<GameBoard>();
            _buildHologram = GameObject.Instantiate<BuildHologram>(_buildConfig.buildHologram);

            GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
            if (!pool)
            {
                Debug.LogError("NO SCENE POOL TRANSFORM FOUND IN SCENE");
            }
            _buildingGenerator = new GenericPooler(pool ? pool.transform : null);
            BuildConfig.BuildableBlueprint[] blueprints = _buildConfig.buildables;
            for (int i = 0; i < blueprints.Length; i++)
            {
                BuildConfig.BuildableBlueprint blueprint = blueprints[i];
                _buildingGenerator.InitPool(blueprint.GetKey(), 10, blueprint.prefab);
            }

            //UI
            _buildViewController = new BuildViewController(this,_inventorySystem, _buildConfig);

            //Build Cam
            //_buildCam = new BuildCam(_buildConfig,_gameplayCam);

            //init currenty build type
            SetBuildType(_buildConfig.startingBuildType);

        }
        #endregion

        public void CleanUp()
        {
            _buildViewController.RemoveView();
        }

        public void EnableSystem(bool on)
        {
            if(_buildModeEnabled != on)
            {
                _buildModeEnabled = on;
                _buildViewController.SetVisible(on);
                if(_buildHologram != null)
                {
                    _buildHologram.gameObject.SetActive(on);
                }
            }
        }

        public void SetBuildType(Buildable.TYPE type)
        {
            _currentBuildType = type;

            //morph hologram
            BuildConfig.BuildableBlueprint blueprint = _buildConfig.GetBuildableBlueprint(type);
            _buildHologram.SetMesh(blueprint.hollogramMesh);
            _buildHologram.SetMaterial(blueprint.hologramMat);
            _buildHologram.SetScale(blueprint.hologramScale);
            _buildHologram.UpdateHologram(false, _buildConfig.hologramData);

            _collisionDetectionEnabled = blueprint.enableCollisionDetection;
            _buildRechargeRate = blueprint.buildRechargeRate;

            _buildViewController.OnBuildTypeChange(type);
            Singleton.instance.audioSystem.PlaySound(SoundBank.Type.BuildSwitch);
        }

        #region Update
        public void Tick(float deltaTime)
        {
            if (_buildModeEnabled)
            {
                GetChangeBuildTypeInput();
                UpdateBuildRecharge(deltaTime);
                BuildCoreLoop();
                BuildCameraMovement();
            }
        }

        public void FixedTick(float deltaTime)
        {
            if (_buildModeEnabled)
            {
                BuildCameraMovementFixed();
            }
        }

        private void UpdateBuildRecharge(float deltaTime)
        {
            //Build recharge timer
            if (_builderRecharging)
            {
                buildTimer += deltaTime;
                if (buildTimer > _buildRechargeRate)
                {
                    _builderRecharging = false;
                    buildTimer = 0;
                }
            }
        }

        private void BuildCoreLoop()
        {
            if(_builderRecharging)
            {
                return;
            }
            int cost = _buildConfig.GetCost(_currentBuildType);
            bool canBuy = _inventorySystem.CanBuy(Storeable.Type.Coin,cost);

            //Set our hologram position
            Ray ray = _gameplayCam.camera.ScreenPointToRay(Input.mousePosition);
            //IF WE ARE ON THE GAMEBOARD
            if (Physics.RaycastNonAlloc(ray, _hitResults, 100, _gameBoard.GetLayer()) > 0)
            {
                Vector3 hitPoint = _hitResults[0].point;
                seaLevelPos.x = hitPoint.x;
                seaLevelPos.z = hitPoint.z;
                GridPosition gridPos = GetHoloGramPosition(hitPoint);
                float yOffset = _buildConfig.GetOffset(_currentBuildType);
                _buildHologram.SetPosition(gridPos,yOffset);
                Debug.DrawRay(hitPoint, Vector3.up, Color.blue);

                bool colliding = false;
                if (_collisionDetectionEnabled)
                {
                    Collider[] overlap = Physics.OverlapBox(_buildHologram.GetPosition(), (_buildHologram.GetScale() / 2.1f), Quaternion.identity, _buildConfig._collisionLayerMask);
                    colliding = overlap.Length > 0;
                    //if(colliding)
                    //{
                    //    Debug.Log(overlap[0].transform.name);
                    //}
                }



                _buildHologram.UpdateHologram(!colliding && canBuy, _buildConfig.hologramData);

                //TRY TO BUILD
                if (canBuy && !colliding && Input.GetKey(KeyCode.Mouse1))
                {
                    //BUILD!
                    BuildConfig.BuildableBlueprint blueprint = _buildConfig.GetBuildableBlueprint(_currentBuildType);
                    Buildable building = _buildingGenerator.GetPooledObject(blueprint.GetKey()) as Buildable;
                    building.Build(_buildHologram.GetPosition(), blueprint.buildTime, blueprint.fallHeight, blueprint.buildEaseType, blueprint);

                    _builderRecharging = true;
                    BuyBuildable(cost);
                }
            }
        }

        private void GetChangeBuildTypeInput()
        {
            for (int i = 0; i < _buildables.Length; i++)
            {
                BuildConfig.BuildableBlueprint blueprint = _buildables[i];
                if (Input.GetKeyDown(blueprint.buildHotKey))
                {
                    SetBuildType(blueprint.key);
                }
            }
        }

        private void BuildCameraMovement()
        {
            if(_buildCam != null)
            {
                _buildCam.OnUpdate();
            }
        }

        private void BuildCameraMovementFixed()
        {
            if(_buildCam != null)
            {
                _buildCam.OnFixedUpdate();
            }
        }
        #endregion

        #region Buy Buildable
        private void BuyBuildable(int cost)
        {
            Storeable.Type coin = Storeable.Type.Coin;
            _inventorySystem.Buy(coin, cost);
            _buildViewController.UpdateInventoryUI(coin, _inventorySystem.GetAmount(coin));
        }
        #endregion

        #region Hologram position helper
        /// <summary>
        /// Gets the correct Hologram position on the grid
        /// </summary>
        /// <param name="hitPoint"></param>
        /// <returns></returns>
        private GridPosition GetHoloGramPosition(Vector3 hitPoint)
        {
            //X
            float x = (hitPoint.x * 2);
            x = (float)Math.Round(x, MidpointRounding.AwayFromZero);
            x = x / 2;

            //Y
            float y = _buildConfig.GetBuildSpace(_currentBuildType) == Buildable.BuildSpace.Above ? ABOVE_SEA_LEVEL : 0;

            //Z
            float z = (hitPoint.z * 2);
            z = (float)Math.Round(z, MidpointRounding.AwayFromZero);
            z = z / 2;

            //RETURN NEW POS
            return GridPosition.Create(x, y, z);

        }
        #endregion

        #region Events
        private void onItemPickedUp(GhostGen.GeneralEvent e)
        {
            Storeable.Type coin = Storeable.Type.Coin;
            _buildViewController.UpdateInventoryUI(coin, _inventorySystem.GetAmount(coin));
        }
        #endregion
    }
}
