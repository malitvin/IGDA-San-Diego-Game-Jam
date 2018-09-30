//Unity
using UnityEngine;

//Game
using Common.Util;

//C#
using System.Collections.Generic;

namespace Gameplay.Particles
{
    [CreateAssetMenu(fileName = "Particle Config I", menuName = "Particles/Particle Config")]
    public class ParticleConfig : ScriptableObject
    {
        [System.Serializable]
        public class ParticleBlueprint
        {
            public string displayName;
            public Particle.Type type;
            public BaseParticle particlePrefab;
            [Tooltip("Data of this particle")]
            public Characteristics characteristics;
        }

        [System.Serializable]
        public class Characteristics
        {
            [Tooltip("How long does this particle last in the scene before pooled")]
            public bool autoDestruct;
            [Range(0.01f, 5)]
            public float lifeTime = 1;
        }

        public ParticleBlueprint[] particles;

        #region Characteristics Lookup
        private static Dictionary<Particle.Type, Characteristics> _characteristicLookup
        = new Dictionary<Particle.Type, Characteristics>(new FastEnumIntEqualityComparer<Particle.Type>());

        public Characteristics GetCharacteristics(Particle.Type type)
        {
            if (_characteristicLookup.Count == 0)
            {
                InitCharacteristics();
            }

            if (_characteristicLookup.ContainsKey(type))
            {
                return _characteristicLookup[type];
            }
            else
            {
                Debug.LogError("No Characteristics found of particle type:" + type);
                return null;
            }
        }

        private void InitCharacteristics()
        {
            InitCharacteristic(particles);
        }

        private void InitCharacteristic(ParticleBlueprint[] particles)
        {
            int i = 0;
            int particleLength = particles.Length;
            for (i = 0; i < particleLength; i++)
            {
                ParticleBlueprint blueprint = particles[i];
                Particle.Type type = blueprint.type;
                if (_characteristicLookup.ContainsKey(type))
                {
                    Debug.LogWarning("Multiple types: " + type + " added to " + name + " config");
                }
                else
                {
                    _characteristicLookup.Add(type, blueprint.characteristics);
                }
            }
        }
        #endregion

    }
}
