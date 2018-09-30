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
            HelixBomb,
            ExplosiveBarrel
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

        private Rigidbody _rigid;
        protected Rigidbody _rigidBody
        {
            get { return _rigid ?? (_rigid = GetComponent<Rigidbody>()); }
        }

        protected BuildConfig.BuildableBlueprint _blueprint;

        public virtual void Build(Vector3 finalPos,float buildTime,int fallheight,Ease easeType, BuildConfig.BuildableBlueprint blueprint)
        {
            _blueprint = blueprint;
            Vector3 startPosition = finalPos + (Vector3.up * fallheight);
            transform.position = startPosition;
        }

        public virtual void RemovePhysics()
        {

        }
    }
}
