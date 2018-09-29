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

    public Movement movement;

}
