
using System;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    
    [Tooltip("-1일 경우 무제한")] public float Duration = 3.0f;
    private float remainTime = 3.0f;
    
    public event Action OnEndEffect;
    private void OnEnable()
    {
        remainTime = Duration;
    }

    private void Update()
    {
        if (Duration < 0) return;
        remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            EndEffect();
        }
    }
    
    public void EndEffect(){
        gameObject.SetActive(false);
        OnEndEffect?.Invoke();
    }
}
