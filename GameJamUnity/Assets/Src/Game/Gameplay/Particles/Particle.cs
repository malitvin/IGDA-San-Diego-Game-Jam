//Unity
using UnityEngine;

namespace Gameplay.Particles
{
    public class Particle
    {
        public enum Type
        {
            //basic impact
            BaseExplosion
        }

        public struct SpawnData
        {
            public Type _type;
            public Vector3 _position;

            public SpawnData(Type type, Vector3 position)
            {
                _type = type;
                _position = position;
            }
        }
    }
}
