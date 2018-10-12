using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gameplay.Loot;

[System.Serializable]
public class EnemyDef
{

    [System.Serializable]
    public class Movement
    {
        public float speed;
        public float drag;
    }

    [System.Serializable]
    public class Attack
    {
        public float damage;
        public float refreshDuration;
    }

    public Movement movement;
    public Attack attack;
    public LayerMask targetMask;
    public int rewardsForKill;
    public float startHealth;
}


[CreateAssetMenu(menuName = "IDGA/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    
    public EnemyDef basicEnemy;
}
