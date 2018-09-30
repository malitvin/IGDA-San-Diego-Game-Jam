//Unity
using UnityEngine;

//Game
using Common.Pooling;

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
            InitParticlePool();
        }

        /// <summary>
        /// Adds different particles of type (for example impact) to pool
        /// Particles are stored in different arrays in particle config for readability
        /// </summary>
        private void InitParticlePool()
        {
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
            string spawnType = type.ToString();
            ParticleConfig.Characteristics characteristics = _particleConfig.GetCharacteristics(type);
            //Spawn particle
            BaseParticle particle =_particlePool.GetPooledObject(spawnType) as BaseParticle;
            particle.Init(characteristics, position);
        }
        #endregion
    }
}
