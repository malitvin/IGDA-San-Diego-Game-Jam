//Unity
using Common.Pooling;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Loot
{
    public class BaseLootItem : PoolableObject, ILootItem
    {
        #region Components
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

        private Rigidbody _rigid;
        private Rigidbody _rigidBody
        {
            get { return _rigid ?? (_rigid = GetComponent<Rigidbody>()); }
        }

        private BoxCollider _boxColl;
        private BoxCollider _boxCollider
        {
            get { return _boxColl ?? (_boxColl = GetComponent<BoxCollider>()); }
        }
        #endregion

        protected LootConfig _lootConfig;
        protected LootConfig.LootDef _lootDef;
        protected LootConfig.LootAnimDef _animDef;
        protected LootConfig.LootItemDef _itemDef;

        private bool _booming;
        private Vector3 zeroVector = Vector3.zero;
        private Transform _followPosition;

        private GhostGen.IEventDispatcher _dispatcher;

        #region ILootItem
        public virtual void Init(ILootable origin, Transform follow, LootConfig.LootDef lootDef, LootConfig lootConfig, LootConfig.LootItemDef itemDef)
        {
            if(_dispatcher == null)
            {
                _dispatcher = _dispatcher = Singleton.instance.notificationDispatcher;
            }

            _autoDestruct = true;
            _lifeTime = lootConfig.autoDestroyTime;

            _followPosition = follow;

            _lootConfig = lootConfig;
            _lootDef = lootDef;
            _animDef = lootConfig.lootAnimDef;
            _itemDef = itemDef;

            _navMeshAgent.speed = _animDef.playerAttractSpeed;

            _navMeshAgent.enabled = false;
            _rigidBody.isKinematic = false;
            _boxCollider.enabled = true;

            _booming = true;

            RefreshColor();
            transform.position = origin.position;
            BoomEffect();
        }

        public virtual void RefreshColor()
        {
            ParticleSystem system = null;
            for (int i = 0; i < _lootParticles.Length; i++)
            {
                system = _lootParticles[i];
                var main = system.main;
                main.startColor = _lootDef.color;
            }
        }
        #endregion

        private void BoomEffect()
        {
            float angle = Random.Range(0, Mathf.PI * 360);
            Vector3 origin = new Vector3(transform.position.x, _animDef.boomHeight, transform.position.z);
            Vector3 directionPos = origin - new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            Vector3 directionalVector = (directionPos - transform.position).normalized;
            _rigidBody.AddForce(directionalVector * _animDef.boomForce);
        }

        public override void Update()
        {
            base.Update();
            if (_booming)
            {
                if (_rigidBody.velocity == zeroVector)
                {
                    _booming = false;
                    _navMeshAgent.enabled = true;
                    _rigidBody.isKinematic = true;
                    _boxCollider.enabled = false;
                }
            }
            else
            {
                refreshAgent();
            }
        }

        private void refreshAgent()
        {
            if (_followPosition)
            {
                Vector3 playerPos = _followPosition.position;
                float dist = (transform.position - playerPos).sqrMagnitude;
                if (dist < _animDef.playerAttractDistance)
                {
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.SetDestination(_followPosition.position);

                    if (dist < _lootConfig.collectRange)
                    {
                        _dispatcher.DispatchEvent(GameplayEventType.LOOT_PICKED_UP, false, _itemDef);
                        RemoveFromPool();
                    }
                }
                else
                {
                    if (_navMeshAgent.isActiveAndEnabled)
                    {
                        _navMeshAgent.isStopped = true;
                    }
                }
            }
            else
            {
                _navMeshAgent.enabled = false;
            }
        }
    }
}
