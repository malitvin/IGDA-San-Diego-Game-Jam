//Unity
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Loot
{
    public class CollectState : LootItemState
    {
        private NavMeshAgent _agent;

        private int _refreshTimer;
        private int _refreshRate;
        private float _collectRange;
        private float _playerAttractDistance;

        private Transform _followPoint;

        public CollectState(BaseLootItem lootItem,LootConfig _lootConfig,Transform followPoint) : base(lootItem)
        {
            _agent = _lootItem._navMeshAgent;
            _agent.speed = _lootConfig.playerAttractSpeed;
            _refreshRate = _lootConfig.refreshRate;
            _collectRange = _lootConfig.collectRange;
            _playerAttractDistance = _lootConfig.playerAttractDistance;

            _followPoint = followPoint;
        }

        public override void Begin()
        {
            base.Begin();

            if (!_agent.isOnNavMesh)
            {
                NavMeshHit closestHit;
                Vector3 itemPos = GetPosition();
                if (NavMesh.SamplePosition(itemPos, out closestHit, 500f, NavMesh.AllAreas))
                {
                    SetPosition(closestHit.position);
                }
            }
            _agent.enabled = true;
            _refreshTimer = 0;
        }

        public override void Exit()
        {
            base.Exit();
            _agent.enabled = false;
        }

        public override void Tick()
        {
            _refreshTimer++;
            if (_refreshTimer > _refreshRate)
            {
                _refreshTimer = 0;

                if (_followPoint && _agent.isActiveAndEnabled)
                {
                    _agent.isStopped = false;
                    Vector3 playerPos = _followPoint.position;
                    Vector3 lootPos = GetPosition();
                    float dist = (lootPos - playerPos).sqrMagnitude;
                    if (dist < _playerAttractDistance)
                    {
                        _agent.SetDestination(playerPos);

                        if (dist < _collectRange)
                        {
                            _lootItem.ChangeItemState(Loot.State.Collected);
                        }
                    }
                    else
                    {
                       _agent.isStopped = true;
                    }
                }
                else if(_followPoint == null)
                {
                    _agent.enabled = false;
                }
            }
        }
    }
}
