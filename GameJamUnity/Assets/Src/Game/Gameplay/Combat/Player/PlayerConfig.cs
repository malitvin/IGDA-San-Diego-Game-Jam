using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IDGA/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [System.Serializable]
    public class Movement
    {
        public float speed;
        public float drag;
    }

    [System.Serializable]
    public class Weapon
    {
        public float bulletSpeed;
        public float fireCooldown;
        public float damage;
    }

    public Movement movement;
    public Weapon weapon;
    public float startHealth;
    public float parentStartHealth;



}
