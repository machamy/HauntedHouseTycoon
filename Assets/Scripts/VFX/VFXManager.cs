using System;
using Pools;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class VFXManager : SingletonBehaviour<VFXManager>
{
    [SerializeField] List<GameObject> _vfxPrefabs = new List<GameObject>();
    private List<IObjectPool<Poolable>> _pools = new List<IObjectPool<Poolable>>();

    public void Awake()
    {
        for (int i = 0; i < _vfxPrefabs.Count; i++)
        {
            _pools.Add(new ObjectPool<Poolable>(
                () => CreatePoolable(_vfxPrefabs[i]),
                ActionOnGet,
                ActionOnRelease,
                null,
                true,
                5,
                100
            ));
        }
    }

    public VisualEffect GetEffect()
    {
        
    }
    
    private Poolable CreatePoolable(GameObject prefab)
    {
        var res = prefab.GetOrAddComponent<Poolable>();
        res.gameObject.GetOrAddComponent<VisualEffect>();
        return res;
    }
    
    private void ActionOnGet(Poolable poolable)
    {
        poolable.transform.SetParent(transform,false);
        poolable.OnGetFromPool();
    }
    
    private void ActionOnRelease(Poolable poolable)
    {
        poolable.transform.SetParent(transform);
        poolable.OnReturnToPool();
    }
}
