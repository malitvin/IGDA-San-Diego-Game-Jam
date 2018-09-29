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
            Block,
            HelixBomb
        }

        public enum BuildSpace
        {
            Above,
            At,
            Below
        }

        private BoxCollider _collider;
        protected BoxCollider _boxCollider
        {
            get { return _collider ?? (_collider = GetComponent<BoxCollider>()); }
        }

        public virtual void Build(Vector3 finalPos,float buildTime,int fallheight,Ease easeType)
        {
            Vector3 startPosition = finalPos + (Vector3.up * fallheight);
            transform.position = startPosition;
        }
    }
}
