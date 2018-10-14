//Unity
using UnityEngine;
using UnityEngine.AI;

//Game
using Common.Util;
using Common.Pooling;

//C#
using System.Collections.Generic;

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
        public NavMeshAgent _navMeshAgent
        {
            get { return _navAgent ?? (_navAgent = GetComponent<NavMeshAgent>()); }
        }
        #endregion

        protected LootConfig _lootConfig;
        protected LootConfig.LootDef _lootDef;

        public LootConfig.LootItemDef _itemDef { get; private set; }

        private Transform _followPosition;

        private Dictionary<Loot.State, ILootState> _lootStateMachine;
        private ILootState _currentLootState;

        #region ILootItem
        public virtual void Init(ILootable origin, Transform follow, LootConfig.LootDef lootDef, LootConfig lootConfig, LootConfig.LootItemDef itemDef)
        {
            _autoDestruct = true;
            _lifeTime = lootConfig.autoDestroyTime;

            _followPosition = follow;

            _lootConfig = lootConfig;
            _lootDef = lootDef;
            _itemDef = itemDef;

            RefreshColor();
            transform.position = origin.position;
            transform.position += new Vector3(0,1f, 0); 

            ChangeItemState(Loot.State.Boom);
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

        public void PickUp()
        {
            RemoveFromPool();
        }
        #endregion

        #region State Machine
        private void InitStateMachine()
        {
            _lootStateMachine = new Dictionary<Loot.State, ILootState>();
            _lootStateMachine[Loot.State.Boom] = new BoomState(this,_lootConfig.lootBoomDef);
            _lootStateMachine[Loot.State.Collect] = new CollectState(this, _lootConfig,_followPosition);
            _lootStateMachine[Loot.State.Collected] = new CollectedState(this);
        }

        public void ChangeItemState(Loot.State state)
        {
            if(_lootStateMachine == null)
            {
                InitStateMachine();
            }

            if (_currentLootState != null)
            {
                _currentLootState.Exit();
            }

            if (_lootStateMachine.ContainsKey(state))
            {
                _currentLootState = _lootStateMachine[state];
                _currentLootState.Begin();
            }
            else
            {
                Debug.LogError("NO LOOT state: " + state + " exists in Loot item state map");
            }
        }
        #endregion

        #region Updates
        public override void Update()
        {
            base.Update();
            if(_currentLootState != null)
            {
                _currentLootState.Tick();
            }
        }

        private void LateUpdate()
        {
            if (_currentLootState != null)
            {
                _currentLootState.LateTick();
            }
        }

        private void FixedUpdate()
        {
            if (_currentLootState != null)
            {
                _currentLootState.FixedTick();
            }
        }
        #endregion
    }
}
