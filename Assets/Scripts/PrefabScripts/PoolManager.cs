
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
            Guest = 101,
            GuestVisual = 102,
            
            CardObject = 201,
            CardDisplay = 202,
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
            poolable.transform.SetParent(_poolParent,false);
            poolable.OnGetFromPool();
        }
        
        private void ActionOnRelease(Poolable poolable)
        {
            poolable.transform.SetParent(transform);
            poolable.OnReturnToPool();
        }
        
        public void Release(Poolable poolable)
        {
            poolable._pool.Release(poolable);
        }
        
        private Transform _poolParent;
        public Poolable Get(Poolables type, Transform parent = null)
        {
            _poolParent = parent;
            var poolable = _pools[type].Get();
            poolable._pool = _pools[type];
            return poolable;
        }
    }
}

