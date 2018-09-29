//Unity
using UnityEngine;

namespace Common.Pooling
{
  public class PoolableObject : MonoBehaviour, IPoolable
    {
        private GenericPooler _pooler;
        private string _poolKey;

        protected bool _autoDestruct = false;
        protected float _destructionTimer;
        protected float _lifeTime;

        #region Unity
        public void Awake()
        {
            gameObject.SetActive(false);
        }

        public virtual void Update()
        {
            //Cleanup
            if(_autoDestruct)
            {
                _destructionTimer += Time.deltaTime;
                if (_destructionTimer > _lifeTime)
                {
                    RemoveFromPool();
                }
            }
        }
        #endregion

        #region Interface Methods
        public void OnInit(string key, GenericPooler pooler)
        {
            _pooler = pooler;
            _poolKey = key;
        }

        public virtual void OnPoolCreate()
        {

        }

        public virtual void OnPoolGet()
        {
            _destructionTimer = 0;
            gameObject.SetActive(true);
        }

        public virtual void OnPoolRelease()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnPoolReuse()
        {

        }
        #endregion

        protected virtual void RemoveFromPool()
        {
            if (_pooler != null)
            {
                _pooler.RemovePooledObject(_poolKey, this);
            }
            else
            {
                Debug.LogError("Pooler reference " + name + " is null");
            }
        }
    }
}