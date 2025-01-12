

using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VariableSOTextUI<T> : MonoBehaviour
{
    public string formatText = "variable : {0}";
    
    public VariableSO<T> variableSO;
    private TextMeshProUGUI textUI;

    private void OnEnable()
    {
        variableSO.OnValueChanged += OnValueChanged;
    }
    
    private void OnDisable()
    {
        variableSO.OnValueChanged -= OnValueChanged;
    }
    
    
    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        OnValueChanged(variableSO.Value);
    }
    
    private void OnValueChanged(T value)
    {
        textUI.text = string.Format(formatText, value);
    }
}
