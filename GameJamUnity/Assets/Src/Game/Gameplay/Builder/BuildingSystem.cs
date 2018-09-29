//Unity
using UnityEngine;

//C#
using System;

//Game
using Common.Pooling;

namespace Gameplay.Building
{
    public class BuildingSystem : GhostGen.EventDispatcher
    {
        public bool _buildModeEnabled = true;

        private float buildTimer;
        private bool _builderRecharging;

        private Camera _gameplayCam;
        private GameBoard _gameBoard;
        private BuildHologram _buildHologram;

        public const float ABOVE_SEA_LEVEL = 0.5f;

        private Vector3 seaLevelPos = new Vector3(0, ABOVE_SEA_LEVEL, 0);

        private RaycastHit[] _hitResults = new RaycastHit[5];

        private GenericPooler _buildingGenerator;
        private Buildable.TYPE _currentBuildType;
        private bool _collisionDetectionEnabled;

        private BuildConfig _buildConfig;

        #region Init
        public BuildingSystem(GameConfig gameConfig)
        {
            _gameplayCam = UnityEngine.Object.FindObjectOfType<GameplayCamera>().camera;
            _buildConfig = gameConfig.bulidConfig;
            _gameBoard = GameObject.FindObjectOfType<GameBoard>();
            ////GameObject.FindGameObjectWithTag(_buildConfig._boardTag).GetComponent<GameBoard>();
            if (!_gameBoard)
            {
                Debug.LogError("NO GAME BOARD FOUND ON BUILDINGSYSTEM");
            }
            _buildHologram = GameObject.Instantiate<BuildHologram>(_buildConfig.buildHologram);
            if (!_buildHologram)
            {
                Debug.LogError("NO BUILD HOLOGRAM FOUND ON BUILDINGSYSTEM");
            }
            InitBuilder();
        }

        private void InitBuilder()
        {
            //init building pool
            GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
            if(!pool)
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

            //init builder
            _currentBuildType = _buildConfig.startingBuildType;
            SetBuildType(_currentBuildType);
        }
        #endregion

        private void SetBuildType(Buildable.TYPE type)
        {
            //morph hologram
            BuildConfig.BuildableBlueprint blueprint = _buildConfig.GetBuildableBlueprint(type);
            _buildHologram.SetMesh(blueprint.hollogramMesh);
            _buildHologram.SetScale(blueprint.hologramScale);

            _collisionDetectionEnabled = blueprint.enableCollisionDetection;
        }

        #region Update
        public void Tick(float deltaTime)
        {
            #region Build Recharge Timer
            //Build recharge timer
            if (_builderRecharging)
            {
                buildTimer += deltaTime;
                if (buildTimer > _buildConfig.buildRechargeRate)
                {
                    _builderRecharging = false;
                    buildTimer = 0;
                }
            }
            #endregion

            #region Building Logic
            if (_buildModeEnabled && !_builderRecharging)
            {
                //Set our hologram position
                Ray ray = _gameplayCam.ScreenPointToRay(Input.mousePosition);
                //IF WE ARE ON THE GAMEBOARD
                if (Physics.RaycastNonAlloc(ray, _hitResults, 100, _gameBoard.GetLayer()) > 0)
                {
                    Vector3 hitPoint = _hitResults[0].point;
                    seaLevelPos.x = hitPoint.x;
                    seaLevelPos.z = hitPoint.z;
                    GridPosition gridPos = GetHoloGramPosition(hitPoint);
                    _buildHologram.SetPosition(gridPos);
                    Debug.DrawRay(hitPoint, Vector3.up, Color.blue);

                    bool colliding = false;
                    if (_collisionDetectionEnabled)
                    {
                        Collider[] overlap = Physics.OverlapBox(_buildHologram.GetPosition(), (_buildHologram.GetScale() / 2.1f), Quaternion.identity, _buildConfig._collisionLayerMask);
                        colliding = overlap.Length > 0;
                        _buildHologram.UpdateHologram(colliding, _buildConfig.hologramData);
                    }

                    //TRY TO BUILD
                    if (!colliding && Input.GetKey(KeyCode.Mouse0))
                    {
                        //BUILD!
                        BuildConfig.BuildableBlueprint blueprint = _buildConfig.GetBuildableBlueprint(_currentBuildType);
                        Buildable building = _buildingGenerator.GetPooledObject(blueprint.GetKey()) as Buildable;
                        building.Build(_buildHologram.GetPosition(), blueprint.buildTime, blueprint.fallHeight, blueprint.buildEaseType);

                        _builderRecharging = true;
                    }
                }
                #endregion
            }
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
    }
}
