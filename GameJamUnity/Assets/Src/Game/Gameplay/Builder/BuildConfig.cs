//Unity
using UnityEngine;

//C#
using System.Collections.Generic;

//Game
using Common.Util;

namespace Gameplay.Building
{
    [CreateAssetMenu]
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
            public Buildable.TYPE key;
            [Range(0.1f,4f)]
            public float buildTime = 0.75f;
            private string keyCache;
            public string GetKey()
            {
                if(keyCache == "")
                {
                    keyCache = key.ToString();
                }
                return keyCache;
            }
            public Buildable prefab;
        }

        [Range(0.01f,2f)]
        public float buildRechargeRate = 0.25f;

        public HologramData hologramData;

        public BuildableBlueprint[] buildables;

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

        #endregion
    }
}
