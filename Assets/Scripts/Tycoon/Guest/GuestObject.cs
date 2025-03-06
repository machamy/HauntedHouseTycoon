
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
    [Header("Guest Properties")]
    [SerializeField] private GuestData[] guestData;
    [SerializeField] private int screamRequirement = 5;
    [SerializeField] private int screamRequirementIncrease = 3;
    [SerializeField] private int fear = 0;
    [SerializeField] private int panic = 20;
    [Header("References")]
    [SerializeField] private GuestVisualController guestVisualController;
    [SerializeField] private TextMeshPro fearText;
    [SerializeField] private GuestFearBar fearBar;
    
    
    private bool hasToMove = false;
    /// <summary>
    /// 해당 손님이 아직 움직이지 않았음.
    /// </summary>
    public bool HasToMove {get => hasToMove; set => hasToMove = value;}
    
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
    
    /// <summary>
    /// 최종 공포치
    /// </summary>
    public int FinalFear => fear; // 전역적인 무언가? 있을수 도 있음

    public int FearBySupportCard => 0;
    public float FearByRelic => 0;

    public int Panic => panic;
    public int ScreamRequirement => screamRequirement;
    public int ScreamRequirementIncrease => screamRequirementIncrease;
    public int NextScreamRequirement => screamRequirement + screamRequirementIncrease;
    
    public bool CanScream => FinalFear >= screamRequirement;
    public bool isPanic => FinalFear >= panic;
    public int MovedDistance => movedDistance;

    public Room CurrentRoom => entity.currentRoom;
    
    
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
    public void MoveBehaviour(bool forceMove = false)
    {
        if (!hasToMove && !forceMove)
        {
            return;
        }
        if (!CurrentRoom)
        {
            Debug.LogWarning($"Not at a Room");
            return;
        }
        
        Direction targetDirection;
        Room nextRoom = CurrentRoom.FindLeftmostRoom(TycoonManager.Instance.Field, orientingDirection,out targetDirection);
        if (nextRoom)
        {
            // Debug.Log($"from {CurrentRoom.name} to {nextRoom.name}");
            movedDistance++;
            entity.transform
                .DOMove(nextRoom.transform.position, 0.5f);
                // .OnComplete(()=> entity.Move(nextRoom));
            // entity.currentRoom = nextRoom;
            entity.Move(nextRoom);
            orientingDirection = targetDirection;
            roomEventChannelSo.RaiseCustomerRoomEnter(this, nextRoom);
        }
        else
        {
            Debug.LogWarning($"No next room found");
        }
        
        hasToMove = false;
    }
    
    /// <summary>
    /// 다른 손님으로 합체한다
    /// </summary>
    /// <param name="other"></param>
    public void MergeTo(GuestObject other)
    {
        /*
         - 동선이 겹칠 경우 손님들은 뭉치게 된며 아래 수치들이 변경 된다.
            - 비명을 지를 수 있는 횟수 = 손님들의 최댓값
            - 비명을 지르기 위해 필요한 공포치, 패닉에 빠지는 공포치의 최솟값 = (손님들의 평균) * 1.1^(손님의 수)
            - 현재 공포치 = 손님들의 평균 * 0.9
            - 각 공포 속성에 대한 내성 = 공포를 덜 받는 거만 남음
            - 트라우마는 중복되는거만 남음

        - 예시: A(비명 횟수3, 손님수 1) , B(비명 횟수 2, 손님수 2)가 합쳐지면 C(비명횟수3, 손님수 3)라고 할때
            - C의 첫번째 공포치 =  (A의 첫번째 공포치+B의 첫번째 공포치/2) * 1.1^(3)
            - C의 두번째 공포치 = (A의 두번째 공포치+B의 두번째 공포치/2) * 1.1^(3)
            - C의 세번째 공포치 = A의 세번째 공포치 * 1.1^(3)
         */
        Debug.Log($"{name} merged to {other.name}");
        poolable.Release();
    }
    /// <summary>
    /// 단순하게 공포를 더함
    /// </summary>
    public void AddFearSimple(int amount)
    {
        fear += amount;
        // if (CanScream)
        // {
        //     Scream();
        // }
        OnValueChanged();
    }
    
    /// <summary>
    ///  공포를 더함
    ///  공포치가 패닉치를 넘어가면 패닉에 빠짐
    /// </summary>
    /// {
    ///    (카드의 공포, int)
    ///     + (보조카드에 의한 추가 공포치, int)
    ///     + (유물에 의한 추가 공포치, int))
    /// }
    /// * (카드의 공포 속성에 맞는, float)
    /// * (카드의 키워드에 대응되는 트라우마, float)
    public void AddFear(int baseFear, int supportFear, int relicFear, float cardFearResistance, float relicFearResistance)
    {
        fear += baseFear + supportFear + relicFear;
        fear = Mathf.Max(0, fear);
        fear = Mathf.CeilToInt(fear * cardFearResistance * relicFearResistance);
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
    
    private void OnEnterRoom(Room room)
    {
        GuestObject otherGuest = null;
        Entity otherEntity = room.FindEntity((e)=>e.TryGetComponent<GuestObject>(out otherGuest));
        if (otherEntity && otherGuest)
        {
            MergeTo(otherGuest);
        }
    }
    #region Event Handlers

    
    public void OnCustomerScreamModified(ScreamEventArg screamEventArg)
    {
        AddFearSimple(screamEventArg.modifier + 1);
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
