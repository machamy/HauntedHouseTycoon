
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class SpirteShaderConfigurator : MonoBehaviour
{
    [SerializeField] private bool setMaterial = false;
    [SerializeField] private Material material = null;
    [SerializeField] bool registerAllChildren = true;
    [SerializeField] private SpriteRenderer[] spriteRenderers = Array.Empty<SpriteRenderer>();
    [SerializeField] private ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;

    public void ApplyConfig()
    {
        if (spriteRenderers.Length == 0)
        {
            if (registerAllChildren)
                spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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

    private void Awake()
    {
        ApplyConfig();
    }


    private void OnValidate()
    {
        ApplyConfig();
    }
}
