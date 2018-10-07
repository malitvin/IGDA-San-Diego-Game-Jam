//Unity
using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.Particles
{
    public class Particle
    {
        public enum Type
        {
            //basic impact
            BaseExplosion,
            BuildZone,
            HelixBomb,
            BuildingGlow,
            Escape,
            EnemyDeath,
            Break,
            Parent
        }

        public static Dictionary<Type, string> sParticleTypeString = new Dictionary<Type, string>();

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
