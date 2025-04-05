using System;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "RangedFloatVariableSO", menuName = "VariableSO/RangedFloat", order = 0)]
    public class RangedFloatVariableSO : VariableSO<float>
    {
        [SerializeField] private bool doClamp = true;
        [SerializeField] private float minValue = 0;
        [SerializeField] private float maxValue = 100;
        
        public event Action<float, float> OnRangeChanged;
        public override float Value
        {
            get => value;
            set => base.Value = doClamp ? Mathf.Clamp(value, minValue, maxValue) : value;
        }
        
        public float NormalizedValue
        {
            get => Mathf.InverseLerp(minValue, maxValue, Value);
            set => Value = Mathf.Lerp(minValue, maxValue, value);
        }

        public float MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                OnRangeChanged?.Invoke(minValue, maxValue);
                if (doClamp)
                {
                    Value = Mathf.Clamp(Value, minValue, maxValue);
                }
            }
        }

        public float MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                OnRangeChanged?.Invoke(minValue, maxValue);
                if (doClamp)
                {
                    Value = Mathf.Clamp(Value, minValue, maxValue);
                }
            }
        }

        public void OnValidate()
        {
            if (minValue > maxValue)
            {
                minValue = maxValue;
            }
        }
    }
}