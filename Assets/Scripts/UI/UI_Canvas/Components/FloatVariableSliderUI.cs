using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FloatVariableSliderUI : MonoBehaviour
{
    private Slider slider;
    /// <summary>
    /// 0~1값이어야함
    /// </summary>
    public FloatVariableSO variable;
    private Color backgroundColor = Color.white;
    private Color fillAreaColor = Color.green;

    public Color BackgroundColor
    {
        get => backgroundColor;
        set
        {
            backgroundColor = value;
            OnValidate();
        }
    }
    
    public Color FillAreaColor
    {
        get => fillAreaColor;
        set
        {
            fillAreaColor = value;
            OnValidate();
        }
    }
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        variable.OnValueChanged += OnValueChanged;
        OnValueChanged(variable.Value);
    }
    
    private void OnDisable()
    {
        variable.OnValueChanged -= OnValueChanged;
    }
    
    private void OnValueChanged(float value)
    {
        slider.value = value;
    }

    private void OnValidate()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
        slider.fillRect.GetComponent<Image>().color = fillAreaColor;
        slider.fillRect.GetComponent<Image>().color = backgroundColor;
    }
}
