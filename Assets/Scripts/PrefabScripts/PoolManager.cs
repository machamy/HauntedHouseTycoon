
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class PoolManager : SingletonBehaviour<PoolManager>
    {
        
        [SerializeField] private int _initialSize = 5;
        [SerializeField] private int _maxSize = 100;
        
        public enum Poolables
        {
            None,
            Guest,
            CardObject,
            CardDisplay,
        }
        
        [SerializeField]
        private SerialzableDict<Poolables, PoolableFactorySO> _factories = new SerialzableDict<Poolables, PoolableFactorySO>();
        
        private SerialzableDict<Poolables, IObjectPool<Poolable>> _pools = new SerialzableDict<Poolables, IObjectPool<Poolable>>();

        private void Awake()
        {
            foreach (var factory in _factories)
            {
                _pools[factory.Key] =
                    new ObjectPool<Poolable>(
                        factory.Value.Create,
                        ActionOnGet,
                        ActionOnRelease,
                        null,
                        true,
                        _initialSize,
                        _maxSize
                    );
            }
        }

        private void ActionOnGet(Poolable poolable)
        {
            poolable.transform.SetParent(null);
            poolable.OnGetFromPool();
        }
        
        private void ActionOnRelease(Poolable poolable)
        {
            poolable.transform.SetParent(_poolParent);
            poolable.OnReturnToPool();
        }
        
        public void Release(Poolable poolable)
        {
            poolable._pool.Release(poolable);
        }
        
        private Transform _poolParent;
        public Poolable Get(Poolables type, Transform parent = null)
        {
            var poolable = _pools[type].Get();
            _poolParent = parent;
            return poolable;
        }
    }
}

