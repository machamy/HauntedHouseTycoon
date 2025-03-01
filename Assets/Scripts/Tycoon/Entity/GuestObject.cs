
using System;
using System.Collections.Generic;
using DG.Tweening;
using Pools;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Entity), typeof(Poolable))]
public class GuestObject : MonoBehaviour
{
    private Entity entity;
    [Header("Events")]
    [SerializeField] private ScreamEventChannelSO screamEventChannel;
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    [SerializeField] private CustomerRoomEventSO roomEventChannelSo;
    [Header("Customer Properties")]
    [SerializeField] private int screamRequirement = 5;
    [SerializeField] private int screamRequirementIncrease = 3;
    [SerializeField] private int fear = 0;
    [SerializeField] private int panic = 20;
    [Header("References")]
    [SerializeField] private TextMeshPro fearText;
    
    private int movedDistance = 0;
    
    [FormerlySerializedAs("direction")] [SerializeField] Direction orientingDirection = Direction.None;
    public Direction OrientingDirection
    {
        get => orientingDirection;
        set => orientingDirection = value;
    }

    /// <summary>
    /// 내부 변수가 바뀔 시 호출됨
    /// </summary>
    public event Action OnValueChangedEvent; 
    public event Action OnRemoved;
    
    public int Fear => fear;
    public int Panic => panic;
    public int ScreamRequirement => screamRequirement;
    public int ScreamRequirementIncrease => screamRequirementIncrease;
    public int NextScreamRequirement => screamRequirement + screamRequirementIncrease;
    
    public bool CanScream => fear >= screamRequirement;
    public bool isPanic => fear >= panic;
    public int MovedDistance => movedDistance;

    private Room CurrentRoom => entity.currentRoom;
    
    
    private Poolable poolable;
    
    private void Awake()
    {
        entity = GetComponent<Entity>();
        poolable = GetComponent<Poolable>();
        poolable.OnRelease += OnRelease;
    }


    private void OnEnable()
    {
        OnValueChanged();
        screamEventChannel.OnScreamModified += OnCustomerScreamModified;
        turnEventChannelSo.OnPlayerTurnEnter += OnPlayerTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnExit += OnNonPlayerTurnExit;
    }
    
    private void OnDisable()
    {
        screamEventChannel.OnScreamModified -= OnCustomerScreamModified;
        turnEventChannelSo.OnPlayerTurnEnter -= OnPlayerTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnExit -= OnNonPlayerTurnExit;
        
    }
    
    private void OnRelease()
    {
        OnRemoved?.Invoke();
    }

    private bool isCreatedNow = false;
    public bool IsCreatedNow => isCreatedNow;
    public void OnCreate()
    {
        isCreatedNow = true;
    }
    
    /// <summary>
    /// 손님 1턴간의 이동 로직
    /// </summary>
    public void MoveBehaviour()
    {
        if (!CurrentRoom)
        {
            Debug.LogWarning($"Not at a Room");
            return;
        }
        
        Direction targetDirection;
        Room nextRoom = CurrentRoom.FindLeftmostRoom(TycoonManager.Instance.Field, orientingDirection.Opposite(),out targetDirection);
        if (nextRoom)
        {
            Debug.Log($"from {CurrentRoom.name} to {nextRoom.name}");
            movedDistance++;
            entity.transform
                .DOMove(nextRoom.transform.position, 0.5f)
                .OnComplete(()=> entity.Move(nextRoom));
            entity.currentRoom = nextRoom;
            orientingDirection = targetDirection;
            roomEventChannelSo.RaiseCustomerRoomEnter(this, nextRoom);
        }
        else
        {
            Debug.LogWarning($"No next room found");
        }
    }


    

    public void AddFear(int amount)
    {
        fear += amount;
        // if (CanScream)
        // {
        //     Scream();
        // }
        OnValueChanged();
    }
    
    public void Scream()
    {
        Debug.Log($"{entity.name} is screaming!");
        screamEventChannel.RaiseScreamEvent(new ScreamEventArg(this,0));
        screamRequirement += screamRequirementIncrease;
        fear /= 2;
        OnValueChanged();
    }

    [ContextMenu("Exit")]
    public void Exit()
    {
        Debug.Log($"{entity.name} is exiting!");
        poolable.Release();
    }


    public void OnValueChanged()
    {
        fearText.text = $"{fear} / {screamRequirement}";
        OnValueChangedEvent?.Invoke();
    }
    #region Event Handlers

    
    public void OnCustomerScreamModified(ScreamEventArg screamEventArg)
    {
        AddFear(screamEventArg.modifier + 1);
    }


    public void OnPlayerTurnEnter()
    {
        fear = Mathf.CeilToInt(fear * 0.9f);
        // MoveBehaviour(); 이동은 GuestManager에서 처리
    }
    
    public void OnNonPlayerTurnExit()
    {
        isCreatedNow = false;
    }
    #endregion

}
