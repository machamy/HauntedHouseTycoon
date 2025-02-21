
using System;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "PrefabContainerSO", menuName = "UI/UI_PrefabSO")]
public class PrefabContainerSO<T> : ScriptableObject, ISerializationCallbackReceiver where T : Enum
{
    [Serializable]
    public class PrefabData
    {
        public T type;
        public GameObject prefab;
    }
    [SerializeField] private List<PrefabData> prefabs = new List<PrefabData>();
    private List<GameObject> prefabList = new List<GameObject>();
    
    public GameObject Get(T type) => Get((int)(object)type);
    
    public GameObject Get(int index)
    {
        if (index < 0 || index >= prefabList.Count)
        {
            return null;
        }
        return prefabList[index];
    }
    
    
    public int Count => prefabs.Count;
    public void OnBeforeSerialize()
    {
        prefabs.Clear();
        for(int i = 0; i < Enum.GetNames(typeof(T)).Length; i++)
        {
            prefabs.Add(new PrefabData()
            {
                type = (T)(i as object),
                prefab = prefabList[i]
            });
        }
        prefabs.Sort((a, b) => a.type.CompareTo(b.type));
    }

    public void OnAfterDeserialize()
    {
        prefabList.Clear();
        foreach (var prefabData in prefabs)
        {
            prefabList.Add(prefabData.prefab);
        }
    }
}
