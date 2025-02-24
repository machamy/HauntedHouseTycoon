
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class Poolable : MonoBehaviour
    {
        internal IObjectPool<Poolable> _pool;
        
        
        public void Release()
        {
            _pool.Release(this);
        }
        
        internal void OnGetFromPool()
        {
            gameObject.SetActive(true);
        }
    
        internal void OnReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }

}
