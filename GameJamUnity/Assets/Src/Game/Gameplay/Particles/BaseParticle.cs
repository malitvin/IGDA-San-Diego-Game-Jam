//Unity
using UnityEngine;

//Game
using Common.Pooling;

namespace Gameplay.Particles
{

    public class BaseParticle : PoolableObject
    {
        private ParticleConfig.Characteristics _characteristics;

        private Particle.SpawnData _spawnData;

        public void Init(ParticleConfig.Characteristics characteristics, Vector3 pos)
        {
            _characteristics = characteristics;

            //set auto destruct data
            _autoDestruct = true;
            _lifeTime = _characteristics.lifeTime;
            //set position
            transform.position = pos;
        }
    }
}
