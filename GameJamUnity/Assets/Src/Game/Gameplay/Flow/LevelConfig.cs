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
        public string WAVE;
        public int enemyCount;
        public float minSpawnIntervalTime;
        public float maxSpawnIntervalTime;

        [Space(10)]
        public float waveCompleteWaitTime;
    }

    public List<LevelDef> levels;
}
