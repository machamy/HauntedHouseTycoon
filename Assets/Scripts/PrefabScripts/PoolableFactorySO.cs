
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pools
{
    [CreateAssetMenu(fileName = "PoolableFactorySO", menuName = "ScriptableObjects/FactorySO")]
    public class PoolableFactorySO : IFactory<Poolable>
    {
        [SerializeField] private Poolable _prefab;
    
        public Poolable Create()
        {
            return Object.Instantiate(_prefab);
        }
    }
}

