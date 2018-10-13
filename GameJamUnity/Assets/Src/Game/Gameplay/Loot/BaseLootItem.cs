//Unity
using UnityEngine;
using UnityEngine.AI;

using Common.Pooling;

namespace Gameplay.Loot
{
    public class BaseLootItem : PoolableObject, ILootItem
    {
        private ParticleSystem[] _particles;
        private ParticleSystem[] _lootParticles
        {
            get { return _particles ?? (_particles = GetComponentsInChildren<ParticleSystem>()); }
        }

        private NavMeshAgent _navAgent;
        private NavMeshAgent _navMeshAgent
        {
            get { return _navAgent ?? (_navAgent = GetComponent<NavMeshAgent>()); }
        }

        protected LootConfig.LootDef _lootDef;

        #region ILootItem
        public virtual void Init(ILootable origin,LootConfig.LootDef lootDef)
        {
            _lootDef = lootDef;
            RefreshColor();
            BoomEffect();
            transform.position = origin.position;
        }

        public virtual void RefreshColor()
        {
            ParticleSystem system = null;
            for (int i=0; i < _lootParticles.Length; i++)
            {
                system = _lootParticles[i];
                var main = system.main;
                main.startColor = _lootDef.color;
            }
        }
        #endregion

        private void BoomEffect()
        {

        }

        public override void Update()
        {
            base.Update();
        }
    }
}
