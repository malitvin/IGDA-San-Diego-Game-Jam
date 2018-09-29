//Unity
using UnityEngine;
using DG.Tweening;

//Game
using Common.Pooling;

namespace Gameplay.Building
{
    public class Buildable : PoolableObject
    {
        public enum TYPE
        {
            Block
        }

        private BoxCollider _collider;
        protected BoxCollider _boxCollider
        {
            get { return _collider ?? (_collider = GetComponent<BoxCollider>()); }
        }

        public void Build(Vector3 finalPos,float buildTime)
        {
            Vector3 startPosition = finalPos + (Vector3.up * 2);
            transform.position = startPosition;
            Tween t = transform.DOMove(finalPos, buildTime).SetEase(Ease.OutBounce).OnUpdate(() =>
            {
                Vector3 center = _boxCollider.center;
                center.y = -transform.position.y+0.5f;
                _boxCollider.center = center;
            });

        }
    }
}
