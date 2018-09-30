//Unity
using UnityEngine;

//C#
using System.Collections.Generic;

//Tween
using DG.Tweening;

//Game
using Common.Util;

namespace Gameplay.Building
{
    [CreateAssetMenu(menuName = "IDGA/Build Config")]
    public class BuildConfig : ScriptableObject
    {
        [System.Serializable]
        public class HologramData
        {
            public Color buildColor;
            public Color cantBuildColor;
        }

        [System.Serializable]
        public class BuildableBlueprint
        {
            public KeyCode buildHotKey;
            public string displayName;
            public bool enableCollisionDetection;
            public int currencyCost;
            public Buildable.TYPE key;
            [Tooltip("Build space in terms of sea level At is basically 0")]
            public Buildable.BuildSpace buildSpace;

            [Range(0.01f, 2f)]
            public float buildRechargeRate = 0.25f;

            //anim
            [Range(0,15)]
            public int fallHeight = 2;
            [Range(0.1f, 5f)]
            public float buildTime = 0.75f;
            public Ease buildEaseType;

            //lookup
            private string keyCache;
            public string GetKey()
            {
                if(string.IsNullOrEmpty(keyCache))
                {
                    keyCache = key.ToString();
                }
                return keyCache;
            }

            //Prefab
            public Buildable prefab;
            public Mesh hollogramMesh;
            public Material hologramMat;
            [Range(0.1f,5)]
            public float hologramScale;
            public float hologramOffset;
        }

        [System.Serializable]
        public class BuildUIData
        {
            public BuildableUIItem _itemPrefabUI;
        }

        [System.Serializable]
        public class BuildCameraData
        {
            public int edgeLimit;
            public float drag;
            public float speed;
            public WorldLimit worldLimit;
        }

        [System.Serializable]
        public class WorldLimit
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        public Buildable.TYPE startingBuildType;

        public LayerMask _collisionLayerMask;
        public string _boardTag;
        public BuildHologram buildHologram;
        public HologramData hologramData;

        public BuildableBlueprint[] buildables;
        public BuildUIData _uiData;
        public BuildCameraData _buildCamData;

        #region Quick buildable lookup by type

        private static Dictionary<Buildable.TYPE, BuildableBlueprint> _buildableLookup
        = new Dictionary<Buildable.TYPE, BuildableBlueprint>(new FastEnumIntEqualityComparer<Buildable.TYPE>());

        private void InitCharacteristics()
        {
            int i = 0;
            int length = buildables.Length;
            for (i = 0; i < length; i++)
            {
                BuildableBlueprint blueprint = buildables[i];
                Buildable.TYPE type = blueprint.key;
                if (_buildableLookup.ContainsKey(type))
                {
                    Debug.LogWarning("Multiple types: " + type + " added to " + name + " config");
                }
                else
                {
                    _buildableLookup.Add(type, blueprint);
                }
            }
        }

        public BuildableBlueprint GetBuildableBlueprint(Buildable.TYPE type)
        {
            if (_buildableLookup.Count == 0)
            {
                InitCharacteristics();
            }

            if (_buildableLookup.ContainsKey(type))
            {
                return _buildableLookup[type];
            }
            else
            {
                Debug.LogWarning("No buildables of type " + type + " found in buildables blueprint");
                return null;
            }
        }


        public Buildable.BuildSpace GetBuildSpace(Buildable.TYPE type)
        {
            if (_buildableLookup.Count == 0)
            {
                InitCharacteristics();
            }

            if (_buildableLookup.ContainsKey(type))
            {
                return _buildableLookup[type].buildSpace;
            }
            else
            {
                Debug.LogWarning("No buildables of type " + type + " found in buildables blueprint");
                return Buildable.BuildSpace.At;
            }
        }

        public string GetBuildable(Buildable.TYPE type)
        {
            if (_buildableLookup.Count == 0)
            {
                InitCharacteristics();
            }

            if (_buildableLookup.ContainsKey(type))
            {
                return _buildableLookup[type].GetKey();
            }
            else
            {
                Debug.LogWarning("No buildables of type " + type + " found in buildables blueprint");
                return "";
            }
        }

        public int GetCost(Buildable.TYPE type)
        {
            if (_buildableLookup.Count == 0)
            {
                InitCharacteristics();
            }

            if (_buildableLookup.ContainsKey(type))
            {
                return _buildableLookup[type].currencyCost;
            }
            else
            {
                Debug.LogWarning("No buildables of type " + type + " found in buildables blueprint");
                return 0;
            }
        }

        public float GetOffset(Buildable.TYPE type)
        {
            if(_buildableLookup.Count == 0)
            {
                InitCharacteristics();
            }

            if(_buildableLookup.ContainsKey(type))
            {
                return _buildableLookup[type].hologramOffset;
            }
            else
            {
                Debug.LogWarning("No buildables of type " + type + " found in buildables blueprint");
                return 0;
            }
        }

        #endregion
    }
}
