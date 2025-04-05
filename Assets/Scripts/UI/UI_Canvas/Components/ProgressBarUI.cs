
using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;


// [RequireComponent(typeof(Slider))]
public class ProgressBarUI : MonoBehaviour
{
    // [SerializeField] private Slider slider;
    [SerializeField] private RangedFloatVariableSO variable;
    [SerializeField] bool UseFilledTypeImage = true;
    
    [SerializeField] Image fillImage;

    private void Awake()
    {
        // slider = GetComponent<Slider>();
        if (fillImage == null)
        {
            fillImage = GetComponentInChildren<Image>();
        }
    }

    private void OnEnable()
    {
        if (variable == null)
        {
            variable = GetComponent<RangedFloatVariableSO>();
        }
        if (variable == null)
        {
            Debug.LogError("ProgressBarUI: variable is null");
            return;
        }
        variable.OnValueChanged += OnValueChanged;
        variable.OnRangeChanged += OnRangeChanged;
        OnValueChanged(variable.Value);
        OnRangeChanged(variable.MinValue, variable.MaxValue);
    }
    
    private void OnDisable()
    {
        if (variable == null)
        {
            return;
        }
        variable.OnValueChanged -= OnValueChanged;
        variable.OnRangeChanged -= OnRangeChanged;
    }

    private void SetValue(float value)
    {
        if (UseFilledTypeImage)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = value;
            }
        }
    }

    private void OnValidate()
    {
        if(variable)
            SetValue(variable.NormalizedValue);
    }

    private void OnValueChanged(float value)
    {
        SetValue(variable.NormalizedValue);
    }
    
    private void OnRangeChanged(float minValue, float maxValue)
    {
        SetValue(variable.NormalizedValue);
    }
}
