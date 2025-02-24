
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class Poolable : MonoBehaviour
    {
        internal IObjectPool<Poolable> _pool;
        
        /// <summary>
        /// 첫 호출시에는 실행되지 않음.
        /// </summary>
        public event Action OnGet;
        public event Action OnRelease;
        
        public void Release()
        {
            _pool.Release(this);
        }
        
        internal void OnGetFromPool()
        {
            OnGet?.Invoke();
            gameObject.SetActive(true);
        }
    
        internal void OnReturnToPool()
        {
            OnRelease?.Invoke();
            gameObject.SetActive(false);
            
        }
    }

}
