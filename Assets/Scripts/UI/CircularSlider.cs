using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


/// <summary>
/// 원형 슬라이더
/// 내부값은 Value(0~1)로 저장되며, 이를 각도로 변환하여 표시
/// </summary>
public class CircularSlider : MonoBehaviour, ICircularUI
{
    [FormerlySerializedAs("fillImage")] [SerializeField] private Image maskImage;
    [FormerlySerializedAs("backgroundFillImage")] [SerializeField] private Image fillImage;

    [SerializeField] private bool clockwise = false;
    // [SerializeField] private Direction startDirection = Direction.Up;
    [SerializeField] private float startAngle = 0;
    [SerializeField] private float endAngle = 360;
    
    public float StartAngle => startAngle;
    public float EndAngle => endAngle;
    
    [SerializeField,Range(0,1)] private float value = 0.5f;

    public float Value { get => value; set => SetValue(value); }
    public float AbsAngle { get => GetAbsAngle(value);}

    private void Reset()
    {
        maskImage = GetComponentInChildren<Image>();
    }
    
    public void Initialize(float startAngle, float endAngle, bool clockwise)
    {
        
        this.startAngle = startAngle;
        this.endAngle = endAngle;
        this.clockwise = clockwise;
        if (maskImage)
        {
            maskImage.fillOrigin = 1;
            maskImage.fillClockwise = clockwise;
        }
        if (fillImage)
        {
            fillImage.fillClockwise = clockwise;
        }
        UpdateBaseRotation();
    }

    private void SetValue(float value)
    {
        this.value = value;
        float fillValue = GetAbsAngle(value);
        if (maskImage)
        {
            maskImage.fillAmount = fillValue / 360;
        }
        // if (fillImage)
        // {
        //     fillImage.fillAmount = fillValue / 360;
        // }
    }
    
    /// <summary>
    /// 0~1 사이의 값을 받아서 원형 슬라이더 위의 각도로 변환
    /// </summary>
    /// <param name="value">0~1의 값</param>
    /// <returns></returns>
    public float GetAbsAngle(float value)
    {
        float deltaAngle = endAngle - startAngle;
        return Mathf.Lerp(0, deltaAngle, value);
    }
    
    private void UpdateBaseRotation()
    {
        var originalImageRotation = fillImage.transform.localEulerAngles;
        
        var currentRotation = maskImage.transform.localEulerAngles;
        currentRotation.z = startAngle;
        maskImage.transform.localEulerAngles = currentRotation;
        fillImage.transform.localEulerAngles = originalImageRotation;
    }


    private void OnValidate()
    {
        Initialize(startAngle, endAngle, clockwise);
    }
}
