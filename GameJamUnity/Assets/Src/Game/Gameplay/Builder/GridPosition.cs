//Unity
using UnityEngine;

namespace Gameplay.Building
{
    [System.Serializable]
    public struct GridPosition
    {
        public float x;
        public float z;
        public float y;

        public static GridPosition Create(float _x,float _y, float _z)
        {
            GridPosition pos = new GridPosition();
            pos.x = _x;
            pos.z = _z;
            pos.y = _y;
            return pos;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, 0, z);
        }
    }
}