//Unity
using UnityEngine;

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
            public Buildable prefab;
        }

        public HologramData hologramData;
    }
}
