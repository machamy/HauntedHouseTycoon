
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GuestFearBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float left = -1;
    [SerializeField] private float right = 1;
    /// <summary>
    /// 최소 "최대 값"
    /// </summary>
    [SerializeField] private float minimumMaxValue = 50;
    [Header("UI References")]
    [SerializeField] private SpriteSlider fearSlider;
    [SerializeField] private LineElement CurrentScreamLine;
    [SerializeField] private LineElement NextScreamLine;
    [SerializeField] private LineElement PanicLine;
    
    [Header("Ref")]
    [SerializeField] private GuestObject guestObject;
    private void Awake()
    {
        CurrentScreamLine.Initialize(left, right);
        NextScreamLine.Initialize(left, right);
        PanicLine.Initialize(left, right);
    }

    private void OnEnable()
    {
        // Debug.Log($"Is guest object null? {guestObject == null}");
        if (guestObject)
        {
            guestObject.OnValueChangedEvent -= GuestObjectChangedEvent;
            guestObject.OnRemoved -= GuestObjectRemoved;
            guestObject.OnValueChangedEvent += GuestObjectChangedEvent;
            guestObject.OnRemoved += GuestObjectRemoved;
            
            UpdateUI();
        }
    }

    public void SetGuest(GuestObject guestObject)
    {
        if(this.guestObject)
        {
            this.guestObject.OnValueChangedEvent -= GuestObjectChangedEvent;
            this.guestObject.OnRemoved -= GuestObjectRemoved;
        }
        this.guestObject = guestObject;
        this.guestObject.OnValueChangedEvent += GuestObjectChangedEvent;
        this.guestObject.OnRemoved += GuestObjectRemoved;
        UpdateUI();
    }

    private void OnDisable()
    {
        if(guestObject)
        {
            guestObject.OnValueChangedEvent -= GuestObjectChangedEvent;
            guestObject.OnRemoved -= GuestObjectRemoved;
        }
    }

    private void OnValidate()
    {
        CurrentScreamLine.Initialize(left, right);
        NextScreamLine.Initialize(left, right);
        PanicLine.Initialize(left, right);
    }

    public void UpdateUI()
    {
        // Debug.Log($"Update UI");
        float panic = guestObject.Panic;
        float maxValue = Mathf.Min(minimumMaxValue, panic);
        print($"{guestObject.Fear} / {panic}");
        fearSlider.Ratio = guestObject.Fear / panic;
        PanicLine.Ratio = panic / maxValue;
        CurrentScreamLine.Ratio = (float)guestObject.ScreamRequirement / panic;
        NextScreamLine.Ratio = (float)guestObject.NextScreamRequirement / panic;
    }

    private void GuestObjectRemoved()
    {
        
    }

    private void GuestObjectChangedEvent()
    {
        UpdateUI();
    }
}
