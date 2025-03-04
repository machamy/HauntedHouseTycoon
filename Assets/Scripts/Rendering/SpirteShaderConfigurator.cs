
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpirteShaderConfigurator : MonoBehaviour
{
    [SerializeField] private bool setMaterial = false;
    [SerializeField] private Material material = null;
    [SerializeField] bool registerAllChildren = true;
    [SerializeField] bool overrideChildren = false;
    [SerializeField] private SpriteRenderer[] spriteRenderers = Array.Empty<SpriteRenderer>();
    [SerializeField] private ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;

    public void ApplyConfig()
    {
        if (spriteRenderers.Length == 0)
        {
            if (registerAllChildren)
                spriteRenderers = GetRenderers();
            else
                spriteRenderers = new[] { GetComponent<SpriteRenderer>() };
        }
        foreach (var spriteRenderer in spriteRenderers)
        {
            if(!spriteRenderer)
                return;
            if(material && setMaterial)
                spriteRenderer.material = material;
            spriteRenderer.shadowCastingMode = shadowCastingMode;
            
        }
    }
    
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    private Queue<Transform> serachQueue = new Queue<Transform>();
    private SpriteRenderer[] GetRenderers()
    {
        renderers.Clear();
        serachQueue.Enqueue(transform);
        while (serachQueue.Count > 0)
        {
            GetRenderersRecursive(serachQueue.Dequeue(), !overrideChildren);
        }
        
        return renderers.ToArray();
    }
    private void GetRenderersRecursive(Transform parentTransform, bool stopOnAnoteherComponent = false)
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            var child = parentTransform.GetChild(i);
            if (stopOnAnoteherComponent && child.TryGetComponent<SpirteShaderConfigurator>(out var _))
            {
                continue;
            }
            if (child.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderers.Add(renderer);
            }
            serachQueue.Enqueue(child);
        }
    }

    private void Awake()
    {
        ApplyConfig();
    }


    private void OnValidate()
    {
        ApplyConfig();
    }
}
