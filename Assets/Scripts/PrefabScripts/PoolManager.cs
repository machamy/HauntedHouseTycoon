
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }
        
        [SerializeField] private int _initialSize = 10;
        [SerializeField] private int _maxSize = 100;
        
        public enum Poolables
        {
            Guest,
        }
        
        [SerializeField]
        private SerialzableDict<Poolables, PoolableFactorySO> _factories = new SerialzableDict<Poolables, PoolableFactorySO>();
        
        private SerialzableDict<Poolables, IObjectPool<Poolable>> _pools = new SerialzableDict<Poolables, IObjectPool<Poolable>>();
    
        private void Awake()
        {
            Instance = this;
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
            poolable.OnGetFromPool();
        }
        
        private void ActionOnRelease(Poolable poolable)
        {
            poolable.OnReturnToPool();
        }
        
        public void Release(Poolable poolable)
        {
            poolable._pool.Release(poolable);
        }
        
        public Poolable Get(Poolables type)
        {
            return _pools[type].Get();
        }
    }
}

