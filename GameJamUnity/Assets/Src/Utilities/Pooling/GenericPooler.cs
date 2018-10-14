//Unity
using UnityEngine;
using UnityEngine.Events;

//C#
using System.Collections.Generic;

namespace Common.Pooling
{
    /// <summary>
    /// User: MaximusLit
    /// Date: 7/14/2018
    ///  Generic Object Pool
    /// </summary>
    public class GenericPooler
    {
        #region Internal Pool Class
        /// <summary>
        /// Object Pool internal class w/ Stack as our container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class ObjectPool<T> where T : new()
        {
            public delegate T0 UnityFunc<T0>();

            private readonly UnityAction<T> _onGet;
            private readonly UnityAction<T> _onRemove;
            private readonly UnityAction<T> _onReuse;
            private readonly UnityFunc<T> _onNew;

            private readonly Stack<T> _poolStack;

            public ObjectPool(UnityFunc<T> actionNew, UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease, UnityAction<T> actionOnReuse)
            {
                _poolStack = new Stack<T>();

                _onNew = actionNew;
                _onGet = actionOnGet;
                _onRemove = actionOnRelease;
                _onReuse = actionOnReuse;
            }

            /// <summary>
            /// gets element
            /// </summary>
            /// <returns></returns>
            public T Get()
            {
                T element;
                if (_poolStack.Count == 0)
                {
                    if (_onNew != null) element = _onNew();
                    else element = new T();
                }
                else
                {
                    element = _poolStack.Pop();
                    if (_onReuse != null)
                    {
                        _onReuse(element);
                    }
                }
                if (_onGet != null)
                {
                    _onGet(element);
                }
                return element;
            }

            /// <summary>
            /// release element
            /// </summary>
            /// <param name="element"></param>
            public void Release(T element)
            {
                if (_onRemove != null)
                {
                    _onRemove(element);
                }

                _poolStack.Push(element);
            }

            /// <summary>
            /// release all elements from the pool
            /// </summary>
            /// <param name="element"></param>
            public void ClearPool()
            {
                foreach (T element in _poolStack)
                {
                    Release(element);
                }
            }
        }
        #endregion

        public GenericPooler(Transform idealTransform)
        {
            _idealTransform = idealTransform;
        }

        private Dictionary<string, ObjectPool<PoolableObject>> _poolMap = new Dictionary<string, ObjectPool<PoolableObject>>();
        private Dictionary<string, PoolableObject> _prototypeMap = new Dictionary<string, PoolableObject>();

        public class SpawnParams
        {
            public Transform _parent;
            public string _key;

            public void setParams(string key,Transform parent)
            {
                _parent = parent;
                _key = key;
            }
        }

        private SpawnParams _currentSpawnParams = new SpawnParams();
        private Transform _idealTransform;

        public void InitPool(string key, int warmAmount, PoolableObject pooledObject)
        {
            if (!_poolMap.ContainsKey(key) && !_prototypeMap.ContainsKey(key))
            {
                ObjectPool<PoolableObject> pool = new ObjectPool<PoolableObject>(OnNew, OnGet, OnRemove, OnReuse);

                _poolMap[key] = pool;
                _prototypeMap[key] = pooledObject;

                WarmPool(warmAmount, key, pool);
            }
        }

        private void WarmPool(int amount, string key, ObjectPool<PoolableObject> pool)
        {
            PoolableObject[] temp = new PoolableObject[amount];
            for (int i = 0; i < amount; i++)
            {
                PoolableObject p = GetPooledObject(key);
                temp[i] = p;
            }
            for (int i = 0; i < amount; i++)
            {
                PoolableObject p = temp[i];
                RemovePooledObject(key, p);
            }
            temp = null;
        }

        public PoolableObject GetPooledObject(string key)
        {
            _currentSpawnParams.setParams(key, _idealTransform);
            ObjectPool<PoolableObject> pool = GetObjectPool(key);
            return pool.Get();
        }

        public void RemovePooledObject(string key, PoolableObject removedObject)
        {
            if (removedObject == null)
            {
                return;
            }
            else
            {
                ObjectPool<PoolableObject> pool = GetObjectPool(key);
                pool.Release(removedObject);
            }
        }

        #region Poolable Events
        /// <summary>
        /// On New Event
        /// </summary>
        /// <returns></returns>
        private PoolableObject OnNew()
        {
            string currentKey = _currentSpawnParams._key;
            Transform currentTransform = _currentSpawnParams._parent;

            PoolableObject p = null;
            if (_prototypeMap[currentKey])
            {
                p = Object.Instantiate(_prototypeMap[currentKey], currentTransform, false) as PoolableObject;
                p.OnInit(currentKey, this);
                p.OnPoolCreate();
            }
            else
            {
                Debug.LogError("No Prefab of key " + currentKey + " found to spawn in prototype map");
            }
            return p;
        }

        /// <summary>
        /// On Get From Pool Event
        /// </summary>
        /// <param name="poolableObject"></param>
        private void OnGet(PoolableObject poolableObject)
        {
            if (poolableObject == null)
            {
                return;
            }
            poolableObject.OnPoolGet();
        }

        /// <summary>
        /// When an element is reused from the pool
        /// </summary>
        /// <param name="poolableObject"></param>
        private void OnReuse(PoolableObject poolableObject)
        {
            if (poolableObject == null)
            {
                return;
            }
            poolableObject.OnPoolReuse();
        }

        /// <summary>
        /// On Remove from Pool Event
        /// </summary>
        /// <param name="poolableObject"></param>
        private void OnRemove(PoolableObject poolableObject)
        {
            if (poolableObject == null)
            {
                return;
            }
            poolableObject.OnPoolRelease();
        }
        #endregion

        private ObjectPool<PoolableObject> GetObjectPool(string key)
        {
            if (_poolMap.ContainsKey(key))
            {
                return _poolMap[key];
            }
            else
            {
                Debug.LogError("No Key of: " + key + " found in pool map");
                return null;
            }
        }
    }
}
