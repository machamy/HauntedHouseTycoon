
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 해당 스프라이트에 Lit 셰이더 적용
/// </summary>
public class SpirteShaderConfigurator : MonoBehaviour
{
    
    [SerializeField,Tooltip("렌더러에 Material 강제 적용여부")] private bool setMaterial = false;
    [SerializeField,Tooltip("적용할 Material")] private Material material = null;
    [SerializeField,Tooltip("모든 자식 Renderer에 적용여부")] bool registerAllChildren = true;
    [SerializeField,Tooltip("자식에 SpirteShaderConfigurator가 있어도 적용 여부")] bool overrideChildren = false;
    [SerializeField] private SpriteRenderer[] spriteRenderers = Array.Empty<SpriteRenderer>();
    [SerializeField,Tooltip("그림자 적용")] private ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;

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
