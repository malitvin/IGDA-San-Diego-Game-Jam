//Unity
using UnityEngine;

namespace Gameplay.Loot
{
    public class BoomState : LootItemState
    {
        private Rigidbody _rigid;
        private Rigidbody _rigidBody
        {
            get { return _rigid ?? (_rigid = _lootItem.GetComponent<Rigidbody>()); }
        }

        private BoxCollider _boxColl;
        private BoxCollider _boxCollider
        {
            get { return _boxColl ?? (_boxColl = _lootItem.GetComponent<BoxCollider>()); }
        }

        protected LootConfig.LootBoomDef _boomDef;

        private string[] _maskList = { "BoardLayer" };
        private int _collisionMask;

        private Vector3 _noVelocity = Vector3.zero;

        private bool _forceAdded;

        public BoomState(BaseLootItem lootItem, LootConfig.LootBoomDef boomDef) : base(lootItem)
        {
            _boomDef = boomDef;
            _collisionMask = LayerMask.GetMask(_maskList);
        }

        public override void Begin()
        {
            base.Begin();

            _lootItem._navMeshAgent.enabled = false;
            _forceAdded = false;

            EnablePhysics(true);

            //Finally add force
            BoomEffect();
        }

        public override void Tick()
        {
            Vector3 velocity = _rigidBody.velocity;
            if (_forceAdded)
            {
                bool collision = CustomCollisionDetection();
                if (collision || velocity == _noVelocity)
                {
                    _lootItem.ChangeItemState(Loot.State.Collect);
                }
            }
        }

        public override void LateTick()
        {
            _previousFramePosition = _lootItem.transform.position;
        }

        public override void FixedTick()
        {
            Vector3 velocity = _rigidBody.velocity;
            if (velocity != _noVelocity)
            {
                _forceAdded = true;
            }
        }

        public override void Exit()
        {
            EnablePhysics(false);
        }

        private Vector3 _previousFramePosition;
        private bool CustomCollisionDetection()
        {
            Vector3 itemPosition = GetPosition();
            Ray ray = new Ray(_previousFramePosition, (itemPosition - _previousFramePosition).normalized);
            float distance = (itemPosition - _previousFramePosition).magnitude;
            RaycastHit[] hits = Physics.RaycastAll(ray, distance, _collisionMask);

            if (hits.Length > 0)
            {
                return true;
            }
            return false;
        }

        private void BoomEffect()
        {
            Vector3 itemPosition = GetPosition();
            float angle = Random.Range(0, Mathf.PI * 360);
            Vector3 origin = new Vector3(itemPosition.x, _boomDef.boomHeight, itemPosition.z);
            Vector3 directionPos = origin - new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            Vector3 directionalVector = (directionPos - itemPosition).normalized;
            _rigidBody.AddForce(directionalVector * _boomDef.boomForce);
        }

        private void EnablePhysics(bool enable)
        {
            _rigidBody.isKinematic = !enable;
            _boxCollider.enabled = enable;
        }
    }
}
