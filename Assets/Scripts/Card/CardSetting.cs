
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CardSetting
{
    [Header("Defualt Parameters")]
    public float defaultAlpha = 1f;
    public float defaultScale = 1f;
    public float returnSpeed = 20f;
    public bool doAngleCurve = true;
    public AnimationCurve handAngleCurve;
   
    [Header("Follow Parameters")]
    public bool followAnimation = true;
    public float followSpeed = 10f;
    
    [Header("Hover Parameters")]
    public bool isHoverable = true;
    public float hoverScale = 1.5f;
    public float hoverAnimationDuration = 0.2f;
    public bool hoverVisible = true;
    
    [Header("Selection Parameters")]
    public bool isSelectable = true;
    
    [Header("Drag Parameters")]
    public bool isDraggable = true;
    public float dragAnimationDuration = 0.2f;
    public float dragAlpha = 0.5f;
    public float dragScale = 1.0f;
    
    [Header("ETC")]
    public bool forceStopAnimation = false;
}
