
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SpriteSlider : MonoBehaviour
{
    private static readonly int CutoffValue = Shader.PropertyToID("_Value");
    [SerializeField] private SpriteRenderer spriteRenderer;

    [FormerlySerializedAs("value")] [SerializeField,Range(0,1)] private float ratio;

    public float Ratio
    {
        get
        {
            return ratio;
        }
        set
        {
            this.ratio = value;
            UpdateSprite();
        }
    }

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
    private void UpdateSprite()
    {
        if(spriteRenderer == null)
        {
            return;
        }
        
        spriteRenderer.material.SetFloat(CutoffValue, ratio);
    }

    public void OnValidate()
    {
        if(Application.isPlaying)
            UpdateSprite();
        else
        {
            if(spriteRenderer&&spriteRenderer.sharedMaterial)
                spriteRenderer.sharedMaterial.SetFloat(CutoffValue, ratio);
        }
    }
}
