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
    [FormerlySerializedAs("guest")]
    [Header("References")]
    [SerializeField] private GuestObject guestObject;
    [SerializeField] private Transform node;

    

    private void Awake()
    {
        fearSliderUI.Initialize(startAngle, endAngle, clockwise);
        CurrentScreamLine.Initialize(startAngle, endAngle, clockwise);
        NextScreamLine.Initialize(startAngle, endAngle, clockwise);
        PanicLine.Initialize(startAngle, endAngle, clockwise);
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

    public void SetNode(Transform node)
    {
        this.node = node;
    }
    
    private void GuestObjectRemoved()
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
    
    public void GuestObjectChangedEvent()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(!guestObject)
            return;
        float panic = guestObject.Panic;
        fearSliderUI.Value = guestObject.FinalFear / panic;
        PanicLine.Value = 1;
        CurrentScreamLine.Value = (float)guestObject.ScreamRequirement / panic;
        NextScreamLine.Value = (float)guestObject.NextScreamRequirement / panic;
        currentFearCounter.text = guestObject.FinalFear.ToString();
        statusText.text = guestObject.isPanic ? "Panic" : guestObject.CanScream ? "Scream" : "Normal";
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
