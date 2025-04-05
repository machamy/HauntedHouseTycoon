using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class VariableSO<T> : ScriptableObject
    {
        [SerializeField] protected T value;
        /// <summary>
        /// 값이 변하면 Invoke 된다.
        /// </summary>
        public event ValueChanged OnValueChanged;

        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        protected void InvokeValueChanged()
        {
            OnValueChanged?.Invoke(value);
        }


        private void OnValidate()
        {
            if (value == null)
            {
                value = default;
            }
            OnValueChanged?.Invoke(value);
        }

        public delegate void ValueChanged(T value);
    }


    

    

    

}