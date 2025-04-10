
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CardSetting
{
    public bool useDebugSprite = false;
    [Header("Background Sprite")]
    public Sprite simpleBackgroundSprite;
    public Sprite halfBackgroundSprite;
    public Sprite fullBackgroundSprite;
    
    
    [Header("Card Parameters")]
    public float slotWidth = 238.5f;
    public float slotHeight = 375f;
    
    
    [Header("Defualt Parameters")]
    public float defaultAlpha = 1f;
    public float defaultScale = 1f;
    // public float returnSpeed = 20f;
    // public bool doAngleCurve = true;
    public bool usePositionCurve = true;
    public bool useRotationCurve = true;
    public CardCurveSO cardCurveSo;
   
    [Header("Follow Parameters")]
    public bool followAnimation = true;
    public float followSpeed = 10f;
    [Header("Rotation Parameters")]
    public bool followRotation = true;
    public float followRotationSpeed = 10f;
    
    [Header("Hover Parameters")]
    public bool isHoverable = true;
    public float hoverScale = 1.3f;
    public float hoverAnimationDuration = 0.2f;
    public bool hoverVisible = true;
    public bool hoverRotation = true;
    
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
