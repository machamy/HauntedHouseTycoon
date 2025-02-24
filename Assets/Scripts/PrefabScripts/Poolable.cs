
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class Poolable : MonoBehaviour
    {
        internal IObjectPool<Poolable> _pool;
        

        public event Action OnGet;
        public event Action OnRelease;
        
        public void Release()
        {
            _pool.Release(this);
        }
        
        internal void OnGetFromPool()
        {
            // print("OnGet");
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
