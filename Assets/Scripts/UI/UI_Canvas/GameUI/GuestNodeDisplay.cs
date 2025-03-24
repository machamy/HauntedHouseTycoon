using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GuestNodeDisplay : MonoBehaviour
{
    [FormerlySerializedAs("fearSlider")]
    [Header("UI References")]
    [SerializeField] private CircularSliderUI fearSliderUI;
    [SerializeField] private CircularElement PanicLine;
    [SerializeField] private CircularElement CurrentScreamLine;
    [SerializeField] private CircularElement NextScreamLine;
    [SerializeField] private TextMeshProUGUI currentFearCounter;
    [SerializeField] private TextMeshProUGUI statusText;
    [Header("UI Settings")]
    [SerializeField] float followSpeed = 1;
    [SerializeField] bool clockwise = false;
    [SerializeField] float startAngle = 0;
    [SerializeField] float endAngle = 360;
    [FormerlySerializedAs("guestObject")]
    [FormerlySerializedAs("guest")]
    [Header("References")]
    [SerializeField] private GuestParty guestParty;
    [SerializeField] private Transform node;

    

    private void Awake()
    {
        fearSliderUI.Initialize(startAngle, endAngle, clockwise);
        CurrentScreamLine.Initialize(startAngle, endAngle, clockwise);
        NextScreamLine.Initialize(startAngle, endAngle, clockwise);
        PanicLine.Initialize(startAngle, endAngle, clockwise);
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

    public void SetNode(Transform node)
    {
        this.node = node;
    }
    
    private void GuestPartyRemoved()
    {
        Destroy(node.gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (node)
        {
            Vector3 moveTo = Vector3.Lerp(transform.position, node.position, followSpeed * Time.deltaTime);
            transform.position = moveTo;
        }
    }
    
    public void GuestPartyChangedEvent()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(!guestParty)
            return;
        float panic = guestParty.Panic;
        fearSliderUI.Value = guestParty.FinalFear / panic;
        PanicLine.Value = 1;
        CurrentScreamLine.Value = (float)guestParty.ScreamRequirement / panic;
        NextScreamLine.Value = (float)guestParty.NextScreamRequirement / panic;
        currentFearCounter.text = guestParty.FinalFear.ToString();
        statusText.text = guestParty.isPanic ? "Panic" : guestParty.ScreamedBefore ? "Scream" : "Normal";
    }

    public void OnValidate()
    {
        if(endAngle < startAngle)
        {
            endAngle = startAngle;
        }
        else if(endAngle-startAngle > 360)
        {
            endAngle = startAngle + 360;
        }
        fearSliderUI.Initialize(startAngle, endAngle, clockwise);
        CurrentScreamLine.Initialize(startAngle, endAngle, clockwise);
        NextScreamLine.Initialize(startAngle, endAngle, clockwise);
        PanicLine.Initialize(startAngle, endAngle, clockwise);
        UpdateUI();
    }
    
}
