using System;
using TMPro;
using UnityEngine;

public class GuestNodeDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CircularSlider fearSlider;
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
    [Header("References")]
    [SerializeField] private Guest guest;
    [SerializeField] private Transform node;

    

    private void Awake()
    {
        fearSlider.Initialize(startAngle, endAngle, clockwise);
        CurrentScreamLine.Initialize(startAngle, endAngle, clockwise);
        NextScreamLine.Initialize(startAngle, endAngle, clockwise);
        PanicLine.Initialize(startAngle, endAngle, clockwise);
    }
    
    public void SetGuest(Guest guest)
    {
        if(this.guest)
        {
            this.guest.OnValueChangedEvent -= OnGuestChangedEvent;
            this.guest.OnRemoved -= OnGuestRemoved;
        }
        this.guest = guest;
        this.guest.OnValueChangedEvent += OnGuestChangedEvent;
        this.guest.OnRemoved += OnGuestRemoved;
        UpdateUI();
    }

    private void OnDisable()
    {
        if(guest)
        {
            guest.OnValueChangedEvent -= OnGuestChangedEvent;
            guest.OnRemoved -= OnGuestRemoved;
        }
    }

    public void SetNode(Transform node)
    {
        this.node = node;
    }
    
    private void OnGuestRemoved()
    {
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
    
    public void OnGuestChangedEvent()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(!guest)
            return;
        float panic = guest.Panic;
        fearSlider.Value = guest.Fear / panic;
        PanicLine.Value = 1;
        CurrentScreamLine.Value = (float)guest.ScreamRequirement / panic;
        NextScreamLine.Value = (float)guest.NextScreamRequirement / panic;
        currentFearCounter.text = guest.Fear.ToString();
        statusText.text = guest.isPanic ? "Panic" : guest.CanScream ? "Scream" : "Normal";
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
        fearSlider.Initialize(startAngle, endAngle, clockwise);
        CurrentScreamLine.Initialize(startAngle, endAngle, clockwise);
        NextScreamLine.Initialize(startAngle, endAngle, clockwise);
        PanicLine.Initialize(startAngle, endAngle, clockwise);
        UpdateUI();
    }
    
}
