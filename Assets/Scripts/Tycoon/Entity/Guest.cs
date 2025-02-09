
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Entity))]
public class Guest : MonoBehaviour
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
    
    [SerializeField] Direction direction = Direction.None;


    public event Action OnValueChangedEvent; 
    public event Action OnRemoved; // 현재로서는 Disable시에만 호출됨.
    
    public int Fear => fear;
    public int Panic => panic;
    public int ScreamRequirement => screamRequirement;
    public int ScreamRequirementIncrease => screamRequirementIncrease;
    public int NextScreamRequirement => screamRequirement + screamRequirementIncrease;
    
    public bool CanScream => fear >= screamRequirement;
    public bool isPanic => fear >= panic;
    public int MovedDistance => movedDistance;

    private Room CurrentRoom => entity.currentRoom;
    
    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Start()
    {
        OnValueChanged();
    }

    private void OnEnable()
    {
        screamEventChannel.OnScreamModified += OnCustomerScreamModified;
        turnEventChannelSo.OnPlayerTurnEnter += OnPlayerTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnExit += OnNonPlayerTurnExit;
    }
    
    private void OnDisable()
    {
        screamEventChannel.OnScreamModified -= OnCustomerScreamModified;
        turnEventChannelSo.OnPlayerTurnEnter -= OnPlayerTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnExit -= OnNonPlayerTurnExit;
        OnRemoved?.Invoke();
    }

    private bool isCreatedNow = false;
    public bool IsCreatedNow => isCreatedNow;
    public void OnCreate()
    {
        isCreatedNow = true;
    }
    public void MoveBehaviour()
    {
        if (!CurrentRoom)
        {
            Debug.LogWarning($"Not at a Room");
            return;
        }
        
        Room nextRoom = FindNextRoom(TycoonManager.Instance.Field, CurrentRoom, direction);
        if (nextRoom)
        {
            movedDistance++;
            entity.transform
                .DOMove(nextRoom.transform.position, 0.5f)
                .OnComplete(()=> entity.Move(nextRoom));
            entity.currentRoom = nextRoom;
            roomEventChannelSo.RaiseCustomerRoomEnter(this, nextRoom);
        }
        else
        {
            Debug.LogWarning($"No next room found");
        }
    }

    /// <summary>
    /// 들어간 방향을 기준으로, 다음 방향을 구한다.
    /// </summary>
    /// <param name="originDir"></param>
    /// <param name="candidates"></param>
    /// <returns></returns>
    private static Direction GetFirstDirection(Direction originDir, DirectionFlag candidates)
    {
        if (candidates == DirectionFlag.None)
        {
            return Direction.None;
        }

        Direction res = originDir;
        
        for(int i = 0; i < 4; i++)
        {
            res = res.Clockwise();
            if ((candidates & res.ToFlag()) != 0)
            {
                return res;
            }
        }
        return Direction.None;
    }
    
    /// <summary>
    /// 해당 방에서, 들어온 방향을 기준으로 좌측우선 탐색을 통해 다음 방을 찾는다.
    /// 다음방도 들어온 방향을 가지고 있어야 한다.
    /// </summary>
    /// <param name="field"></param>
    /// <param name="currentRoom"></param>
    /// <param name="originDirection"></param>
    /// <returns></returns>
    private static Room FindNextRoom(Field field, Room currentRoom, Direction originDirection)
    {
        Direction targetDir = GetFirstDirection(originDirection, currentRoom.CardData.directions);
        Room nextRoom;
        int count = 0;
        while (targetDir != Direction.None && count++ < 4)
        {
            nextRoom = field.GetRoomByDirection(currentRoom, targetDir);
            if (nextRoom.CardData.directions.HasFlag(targetDir.Opposite().ToFlag()))
            {
                return nextRoom;
            }
            targetDir = GetFirstDirection(targetDir, currentRoom.CardData.directions);
        }
        return null;
    }
    public void AddFear(int amount)
    {
        fear += amount;
        if (CanScream)
        {
            Scream();
        }
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

    public void Exit()
    {
        Debug.Log($"{entity.name} is exiting!");
        Destroy(gameObject); // TODO 테스트용
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
