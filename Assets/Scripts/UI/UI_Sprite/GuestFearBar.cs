
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    [FormerlySerializedAs("guestObject")]
    [Header("Ref")]
    [SerializeField] private GuestParty guestParty;
    private void Awake()
    {
        CurrentScreamLine.Initialize(left, right);
        NextScreamLine.Initialize(left, right);
        PanicLine.Initialize(left, right);
    }

    private void OnEnable()
    {
        // Debug.Log($"Is guest object null? {guestObject == null}");
        if (guestParty)
        {
            guestParty.OnValueChangedEvent -= GuestPartyChangedEvent;
            guestParty.OnRemoved -= GuestPartyRemoved;
            guestParty.OnValueChangedEvent += GuestPartyChangedEvent;
            guestParty.OnRemoved += GuestPartyRemoved;
            
            UpdateUI();
        }
    }

    public void SetGuest(GuestParty guestParty)
    {
        if(this.guestParty)
        {
            this.guestParty.OnValueChangedEvent -= GuestPartyChangedEvent;
            this.guestParty.OnRemoved -= GuestPartyRemoved;
        }
        this.guestParty = guestParty;
        this.guestParty.OnValueChangedEvent += GuestPartyChangedEvent;
        this.guestParty.OnRemoved += GuestPartyRemoved;
        UpdateUI();
    }

    private void OnDisable()
    {
        if(guestParty)
        {
            guestParty.OnValueChangedEvent -= GuestPartyChangedEvent;
            guestParty.OnRemoved -= GuestPartyRemoved;
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
        float panic = guestParty.Panic;
        float maxValue = Mathf.Min(minimumMaxValue, panic);
        print($"{guestParty.FinalFear} / {panic}");
        fearSlider.Ratio = guestParty.FinalFear / panic;
        PanicLine.Ratio = panic / maxValue;
        CurrentScreamLine.Ratio = (float)guestParty.ScreamRequirement / panic;
        NextScreamLine.Ratio = (float)guestParty.NextScreamRequirement / panic;
    }

    private void GuestPartyRemoved()
    {
        
    }

    private void GuestPartyChangedEvent()
    {
        UpdateUI();
    }
}
