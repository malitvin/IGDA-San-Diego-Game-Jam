//Unity
using UnityEngine;

//Game
using Common.Pooling;
using System;

namespace Gameplay.Particles
{
    public class ParticleGOD : GhostGen.EventDispatcher
    {
        private GenericPooler _particlePool;

        private ParticleConfig _particleConfig;

        private const int PARTICLE_POOL_WARM_AMOUNT = 5;

        #region Init
        public ParticleGOD(GameConfig gameConfig)
        {
            _particleConfig = gameConfig.particleConfig;
        }

        /// <summary>
        /// Adds different particles of type (for example impact) to pool
        /// Particles are stored in different arrays in particle config for readability
        /// </summary>
        public void InitParticlePool()
        {

            var particleTypes = Enum.GetValues(typeof(Particle.Type));
            foreach(Particle.Type type in particleTypes)
            {
                Particle.sParticleTypeString[type] = type.ToString();
            }

            if(_particleConfig)
            {
                GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
                _particlePool = new GenericPooler(pool.transform);
                AddToPool(_particleConfig.particles);
            }
            else
            {
                Debug.LogError("Particle Config is null in the Particle God System");
            }
            
        }

        private GenericPooler particlePool
        {
            get
            {
                if(_particlePool == null)
                {
                    GameObject pool = GameObject.FindGameObjectWithTag("ScenePool");
                    if(pool != null)
                    {
                        _particlePool = new GenericPooler(pool.transform);
                    }
                }
                return _particlePool;
            }
        }
        private void AddToPool(ParticleConfig.ParticleBlueprint[] particles)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                ParticleConfig.ParticleBlueprint particle = particles[i];
                string type = particle.type.ToString();
                _particlePool.InitPool(type, PARTICLE_POOL_WARM_AMOUNT, particle.particlePrefab);
            }
        }
        #endregion

        #region Events
        public void GenerateParticle(Particle.Type type, Vector3 position)
        {
            //Get data
            string spawnType = Particle.sParticleTypeString[type];
            ParticleConfig.Characteristics characteristics = _particleConfig.GetCharacteristics(type);
            //Spawn particle
            if(particlePool != null)
            {
                BaseParticle particle = particlePool.GetPooledObject(spawnType) as BaseParticle;
                particle.Init(characteristics, position);
            }
        }
        #endregion
    }
}
