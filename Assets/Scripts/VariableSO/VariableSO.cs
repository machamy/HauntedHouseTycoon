using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class VariableSO<T> : ScriptableObject
    {
        [SerializeField] private T value;
        /// <summary>
        /// 값이 변하면 Invoke 된다.
        /// </summary>
        public event ValueChanged OnValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        
        public delegate void ValueChanged(T value);
    }


    

    

    

}