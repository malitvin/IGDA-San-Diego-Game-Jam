//Unity
using UnityEngine;
//C#
using System.Collections.Generic;

[CreateAssetMenu(menuName = "IDGA/Level Config")]
public class LevelConfig : ScriptableObject
{
    [System.Serializable]
    public class LevelDef
    {
        public float spawnDistance;
        public float waveStartWaitTime;
        public WaveDef[] waves;
    }

    [System.Serializable]
    public class WaveDef
    {
        [Space(10)]

        public int enemyCount;
        public float minSpawnIntervalTime;
        public float maxSpawnIntervalTime;

        public float waveCompleteWaitTime;

        public float waveIndex
        {
            get; set;
        }
    }

    public List<LevelDef> levels;
}
