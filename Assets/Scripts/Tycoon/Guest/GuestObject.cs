
using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Pools;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Entity), typeof(Poolable))]
public class GuestObject : MonoBehaviour
{
    // public class GuestObjectVariable
    // {
    //    
    // }
    
    private Entity entity;
    [Header("Events")]
    [SerializeField] private ScreamEventChannelSO screamEventChannel;
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    [SerializeField] private CustomerRoomEventSO roomEventChannelSo;
    [FormerlySerializedAs("guestDataArray")]
    [FormerlySerializedAs("guestData")]
    [Header("Guest Properties")]
    [SerializeField]
    [ItemNotNull]
    private List<GuestData> guestDataList = new List<GuestData>();
    [SerializeField] private Dictionary<long,float> traumaDict = new Dictionary<long, float>();
    [SerializeField] private List<int> screamRequirements = new List<int>();
    // [SerializeField] private int screamRequirement = 5;
    // [SerializeField] private int screamRequirementIncrease = 3;
    [SerializeField] private int fear = 0;
    [SerializeField] private int panic = 20;
    // [SerializeField] private int fatigue = 2;
    [SerializeField] private int fatigueCoefficient = 2;
    [Header("References")]
    [SerializeField] private GuestVisualController guestVisualController;
    [SerializeField] private TextMeshPro fearText;
    [SerializeField] private GuestFearBar fearBar;
    
    
    private bool hasToMove = false;
    private bool isMoving = false;
    private Room targetRoom;
    /// <summary>
    /// 해당 손님이 아직 움직이지 않았음.
    /// </summary>
    public bool HasToMove {get => hasToMove; set => hasToMove = value;}
    public int GuestAmount => guestDataList.Count;
    
    private int movedDistance = 0;
    private int aliveTurnCnt = 0;
    
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
    public int ScreamRequirement => screamRequirements[0];
    // public int ScreamRequirementIncrease => screamRequirementIncrease;
    public int NextScreamRequirement => screamRequirements.Count > 1 ? screamRequirements[1] : 0;
    
    public bool CanScream => FinalFear >= ScreamRequirement;
    public bool isPanic => FinalFear >= panic;
    public int MovedDistance => movedDistance;

    public Room CurrentRoom => entity.currentRoom;
    
    
    private Poolable poolable;
    
    private void Awake()
    {
        entity = GetComponent<Entity>();
        poolable = GetComponent<Poolable>();
        poolable.OnRelease += OnRelease;
        if(guestVisualController == null)
            guestVisualController = GetComponentInChildren<GuestVisualController>();
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
    
    
    public void ClearAndSetGuestData(GuestData guestData)
    {
        this.guestDataList.Clear();
        this.guestDataList.Add(guestData);
        traumaDict.Clear();
        foreach (var trauma in guestData.traumaRatios)
        {
            traumaDict.Add(trauma.Key, trauma.Value);
        }
        OnValueChanged();
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

        if (isMoving)
        {
            //TODO : 이전 이동 강제로 처리하기
            entity.transform.DOKill();
            entity.MoveWithTransform(targetRoom);
            OnEnterRoom(targetRoom); // 강제 처리한다면, 이건 제대로 작동 안할 확률 높음
        }
        Direction targetDirection;
        Room nextRoom = CurrentRoom.FindLeftmostRoom(TycoonManager.Instance.Field, orientingDirection,out targetDirection);
        if (nextRoom)
        {
            targetRoom = nextRoom;
            // Debug.Log($"from {CurrentRoom.name} to {nextRoom.name}");
            movedDistance++;
            guestVisualController?.PlayAnimation(AnimationType.MOVE);
            entity.transform
                .DOMove(nextRoom.transform.position, 0.5f)
                .OnComplete(() =>
                {
                    guestVisualController?.SetIsMoving(false);
                    entity.Move(nextRoom);
                    OnEnterRoom(nextRoom);
                });
            // entity.currentRoom = nextRoom;
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
    /// 다른 손님으로 합체한다.
    /// 이 객체는 Release된다.
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
        
        // 초기값 설정
        int mergedGuestAmount = GuestAmount + other.GuestAmount;
        other.guestDataList.AddRange(guestDataList);
        float tmpCoefficient = Mathf.Pow(1.1f, mergedGuestAmount);
        
        // 비명 최대치
        int maxScream = Mathf.Max(screamRequirements.Count, other.screamRequirements.Count);
        
        // 비명 요구량 적용
        List<int> newScreamRequirements = new List<int>();
        for (int i = 0; i < maxScream; i++)
        {
            int thisScream = i < screamRequirements.Count ? screamRequirements[i] : 0;
            int otherScream = i < other.screamRequirements.Count ? other.screamRequirements[i] : 0;
            if(thisScream == 0 && otherScream == 0)
                continue;
            newScreamRequirements.Add((int) (tmpCoefficient * (thisScream + otherScream) / mergedGuestAmount));
        }
        
        // other.screamRequirement = (int) (tmpCoefficient * (screamRequirement + other.screamRequirement) / mergedGuestAmount);
        // other.screamRequirementIncrease = (int) (tmpCoefficient * (screamRequirementIncrease + other.screamRequirementIncrease) / mergedGuestAmount);
        other.fear = (int) ((fear + other.fear) / (float)mergedGuestAmount * 0.9f);
        other.panic = (int) (tmpCoefficient * (Panic + other.Panic) / mergedGuestAmount);
        // 트라우마에 대하여, 중복되는 것만 남김
        foreach (var otherTrauma in traumaDict)
        {
            if (other.traumaDict.ContainsKey(otherTrauma.Key))
            {
                // 중복되는 경우
                other.traumaDict[otherTrauma.Key] = tmpCoefficient * ((traumaDict[otherTrauma.Key] + otherTrauma.Value) / mergedGuestAmount);
            }
            else
            {
                // 중복되지 않는 경우
                // other.traumaDict.Add(otherTrauma.Key, otherTrauma.Value);
            }
        }
        
        
        poolable.Release();
    }
    /// <summary>
    /// 단순하게 공포를 더함
    /// </summary>
    public void AddFearSimple(int amount)
    {
        guestVisualController.PlayAnimation(AnimationType.FEAR);
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
        guestVisualController.PlayAnimation(AnimationType.FEAR);
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
        guestVisualController.PlayAnimation(AnimationType.SCREAM);
        Debug.Log($"{entity.name} is screaming!");
        screamEventChannel.RaiseScreamEvent(new ScreamEventArg(this,0));
        screamRequirements.RemoveAt(0);
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
        fearText.text = $"{fear} / {screamRequirements[0]}";
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
        aliveTurnCnt += 1;
        fear = Mathf.CeilToInt(fear * 0.9f);
        // 감소량 = logcoefficient(aliveTurnCnt);
        float panicDecrease = Mathf.Log(aliveTurnCnt, fatigueCoefficient);
        panic -= (int)panicDecrease;
        // MoveBehaviour(); 이동은 GuestManager에서 처리
    }
    
    public void OnNonPlayerTurnExit()
    {
        isCreatedNow = false;
    }
    #endregion

}
