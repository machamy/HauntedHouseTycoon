
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class LineElement : MonoBehaviour
{
    [SerializeField] private float left = 0f;
    [SerializeField] private float right = 1f;
    [FormerlySerializedAs("value")] [SerializeField] private float ratio = 0f;
    
    public void Initialize(float left, float right)
    {
        this.left = left;
        this.right = right;
    }
    
    public float Ratio
    {
        get => ratio;
        set
        {
            this.ratio = Mathf.Clamp01(value);
            UpdatePos();
        }
    }
    
    private void UpdatePos()
    {
        transform.localPosition = Vector3.Lerp(new Vector3(left, 0, 0), new Vector3(right, 0, 0), ratio);
    }
    
    private void OnValidate()
    {
        UpdatePos();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(left, 0, 0), new Vector3(right, 0, 0));
    }
}
