
using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using JetBrains.Annotations;
using Pools;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Entity), typeof(Poolable))]
public class GuestParty : MonoBehaviour, IFocusable
{
    private Entity entity;
    [Header("Events")]
    [SerializeField] private ScreamEventChannelSO screamEventChannel;
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    [SerializeField] private GuestRoomEventSO roomEventChannelSo;
    [FormerlySerializedAs("guestDataArray")]
    [FormerlySerializedAs("guestData")]
    [Header("Guest Properties")]
    [SerializeField]
    [ItemNotNull]
    private List<GuestData> guestDataList = new List<GuestData>();
    [SerializeField] private Dictionary<long,float> traumaDict = new Dictionary<long, float>();
    [SerializeField] private List<int> screamRequirements = new List<int>();

    // [SerializeField] private int screamRequirementIncrease = 3;
    [SerializeField] private int fear = 0;
    [SerializeField] private int panic = 20;
    [SerializeField] private List<float> fearResistances = new List<float>() {1f,1f,1f,1f};
    // [SerializeField] private int fatigue = 2;
    [SerializeField] private int fatigueCoefficient = 2;
    [FormerlySerializedAs("guestVisualController")]
    [Header("References")]
    [SerializeField] private GuestVisualHolder _guestVisualHolder;
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
    public int ScreamRequirement => screamRequirements.Count > 0 ? screamRequirements[0] : 0;
    // public int ScreamRequirementIncrease => screamRequirementIncrease;
    public int NextScreamRequirement => screamRequirements.Count > 1 ? screamRequirements[1] : 0;
    
    public bool CanScream => FinalFear >= ScreamRequirement;
    
    public bool _screamedBefore = false;
    /// <summary>
    /// NPC 턴이 시작되기 전의 비명 여부
    /// </summary>
    public bool ScreamedBefore => _screamedBefore;
    public bool isPanic => FinalFear >= panic;
    public int MovedDistance => movedDistance;

    public Room CurrentRoom => entity.currentRoom;
    
    
    private Poolable poolable;
    private PathDrawer pathDrawer;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        poolable = GetComponent<Poolable>();
        poolable.OnRelease += OnRelease;
        if(_guestVisualHolder == null)
            _guestVisualHolder = GetComponentInChildren<GuestVisualHolder>();
        pathDrawer = gameObject.GetOrAddComponent<PathDrawer>();
        pathDrawer.enabled = false;
        _guestVisualHolder.Initialize(this);
    }


    private void OnEnable()
    {
        OnValueChanged();
        screamEventChannel.OnScreamModified += OnCustomerScreamModified;
        turnEventChannelSo.OnPlayerTurnEnterEvent.AddListener(OnPlayerTurnEnter);
        turnEventChannelSo.OnPlayerTurnExitEvent.AddListener(OnPlayerTurnExit);
        turnEventChannelSo.OnNonPlayerTurnEnterEvent.AddListener(OnNonPlayerTurnEnter);
        turnEventChannelSo.OnNonPlayerTurnExitEvent.AddListener(OnNonPlayerTurnExit); 
    }
    
    private void OnDisable()
    {
        screamEventChannel.OnScreamModified -= OnCustomerScreamModified;
        turnEventChannelSo.OnPlayerTurnEnterEvent.RemoveListener(OnPlayerTurnEnter);
        turnEventChannelSo.OnPlayerTurnExitEvent.RemoveListener(OnPlayerTurnExit);
        turnEventChannelSo.OnNonPlayerTurnEnterEvent.RemoveListener(OnNonPlayerTurnEnter);
        turnEventChannelSo.OnNonPlayerTurnExitEvent.RemoveListener(OnNonPlayerTurnExit);
        
    }
    
    private void OnRelease()
    {
        entity.OnRemoved();
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
        screamRequirements.Clear();
        screamRequirements.AddRange(guestData.screamRequirements);
        traumaDict.Clear();
        foreach (var trauma in guestData.traumaRatios)
        {
            traumaDict.Add(trauma.Key, trauma.Value);
        }
        _guestVisualHolder.AddGuestVisual(guestData);
        OnValueChanged();
    }

    /// <summary>
    /// 단순히 손님 정보 등록 + 비주얼 추가
    /// </summary>
    /// <param name="guestData"></param>
    private void AddGuestDataRaw(GuestData guestData)
    {
        guestDataList.Add(guestData);
        _guestVisualHolder.AddGuestVisual(guestData);
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
            isMoving = false;
            entity.transform.DOKill();
            entity.MoveWithTransform(targetRoom);
            OnEnterRoom(targetRoom); // 강제 처리한다면, 이건 제대로 작동 안할 확률 높음
        }
        Direction targetDirection;
        Room nextRoom = CurrentRoom.FindLeftmostRoom(TycoonManager.Instance.Field, orientingDirection,out targetDirection);
        if (nextRoom)
        {
            targetRoom = nextRoom;
            
            GuestMoveEventArgs e = GuestMoveEventArgs.Get();
            e.GuestParty = this;
            e.fromRoom = CurrentRoom;
            e.toRoom = nextRoom;
            e.isEnter = false;
            roomEventChannelSo.RaiseCustomerRoomExit(e);
            
            // Debug.Log($"from {CurrentRoom.name} to {nextRoom.name}");
            movedDistance++;
            _guestVisualHolder?.PlayAnimation(AnimationType.MOVE);
            isMoving = true;
            entity.transform
                .DOMove(nextRoom.transform.position, 0.5f)
                .OnComplete(() =>
                {
                    _guestVisualHolder?.SetIsMoving(false);
                    entity.Move(nextRoom);
                    OnEnterRoom(nextRoom);
                    isMoving = false;
                });
            // entity.currentRoom = nextRoom;
            orientingDirection = targetDirection;
            
            GuestMoveEventArgs nextRoomEventArgs = GuestMoveEventArgs.Get();
            nextRoomEventArgs.GuestParty = this;
            nextRoomEventArgs.fromRoom = CurrentRoom;
            nextRoomEventArgs.toRoom = nextRoom;
            nextRoomEventArgs.isEnter = true;
            roomEventChannelSo.RaiseCustomerRoomEnter(nextRoomEventArgs);
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
    public void MergeTo(GuestParty other)
    {
        /*
         - 동선이 겹칠 경우 손님들은 뭉치게 된며 아래 수치들이 변경 된다.
    - 비명을 지를 수 있는 횟수 = 손님들의 최댓값
    - 비명을 지르기 위해 필요한 공포치, 패닉에 빠지는 공포치의 최솟값 = (손님들의 평균) * 1.1^(손님의 수)
    - 현재 공포치 = 손님들의 평균 * 0.9^(손님의 수)
    - 각 공포 속성에 대한 내성 = 공포를 덜 받는 거만 남음(float값이 낮은거)
    - 트라우마는 모두 유지되고 중복되는 것들은 적용되는 값이 합쳐져서 적용됨
    - 피로계수 =  (손님들의 평균)
    - 손님이 필드에 존재한 턴수 = (최대값)
         */
        Debug.Log($"{name} merged to {other.name}");
        
        // 초기값 설정
        int mergedGuestAmount = GuestAmount + other.GuestAmount;
        
        // 손님 추가 로직
        foreach (var data in guestDataList)
        {
            other.AddGuestDataRaw(data);
        }
        
        float mergeCoeff1_1 = Mathf.Pow(1.1f, mergedGuestAmount);
        float mergeCoeff0_9 = Mathf.Pow(0.9f, mergedGuestAmount);
        // 비명 최대치
        int maxScream = Mathf.Max(screamRequirements.Count, other.screamRequirements.Count);
        
        StringBuilder debugMsg = new StringBuilder();
        // 비명 요구량 적용
        List<int> newScreamRequirements = new List<int>();
        for (int i = 0; i < maxScream; i++)
        {
            int thisScream = i < screamRequirements.Count ? screamRequirements[i] : 0;
            int otherScream = i < other.screamRequirements.Count ? other.screamRequirements[i] : 0;
            if(thisScream == 0 && otherScream == 0)
                continue;
            newScreamRequirements.Add(Mathf.RoundToInt(mergeCoeff1_1 * (thisScream + otherScream) / mergedGuestAmount));
            debugMsg.Append($"{thisScream} + {otherScream} -> {newScreamRequirements[i]}\n");   
        }
        other.screamRequirements = newScreamRequirements;
        Debug.Log(debugMsg);
        
        // 공포치와 패닉치 : 모든 손님의 평균 * 계수
        other.fear = Mathf.RoundToInt((fear * GuestAmount + other.fear * other.GuestAmount) / (float) mergedGuestAmount * mergeCoeff0_9);
        other.panic = Mathf.RoundToInt((panic * GuestAmount + other.panic * other.GuestAmount) / (float)  mergedGuestAmount * mergeCoeff0_9);
        
        // 공포 속성 내성 : 낮은 값으로 설정
        for (int i = 0; i < fearResistances.Count; i++)
        {
            other.fearResistances[i] = Mathf.Min(fearResistances[i], other.fearResistances[i]);
        }
        
        // 트라우마는 모두 유지되고 중복되는 것들은 적용되는 값이 합쳐져서 적용됨
        foreach (var otherTrauma in traumaDict)
        {
            if (other.traumaDict.ContainsKey(otherTrauma.Key))
            {
                // 중복되는 경우
                other.traumaDict[otherTrauma.Key] = traumaDict[otherTrauma.Key] + otherTrauma.Value;
            }
            else
            {
                // 중복되지 않는 경우
                other.traumaDict.Add(otherTrauma.Key, otherTrauma.Value);
            }
        }
        
        
        // 피로계수 =  (손님들의 평균)
        fatigueCoefficient = Mathf.RoundToInt((fatigueCoefficient * GuestAmount + other.fatigueCoefficient * other.GuestAmount) / (float) mergedGuestAmount);
        
        other.movedDistance = Mathf.Max(movedDistance, other.movedDistance);
        other.OnValueChanged();
        poolable.Release();
    }
    /// <summary>
    /// 단순하게 공포를 더함
    /// </summary>
    public void AddFearSimple(int amount)
    {
        _guestVisualHolder.PlayAnimation(AnimationType.FEAR);
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
        _guestVisualHolder.PlayAnimation(AnimationType.FEAR);
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
        _guestVisualHolder.PlayAnimation(AnimationType.SCREAM);
        Debug.Log($"{entity.name} is screaming!");
        ScreamEventArgs screamEventArg = ScreamEventArgs.Get();
        screamEventArg.GuestParty = this;
        screamEventArg.modifier = 0;
        screamEventChannel.RaiseScreamEvent(screamEventArg);
        screamRequirements.RemoveAt(0);
        _screamedBefore = true;
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
        
        fearText.text = $"{fear} / {ScreamRequirement}";
        OnValueChangedEvent?.Invoke();
    }
    
    private void OnEnterRoom(Room room)
    {
        GuestParty otherGuest = null;
        Entity otherEntity = room.FindEntity( // 움직임이 끝난 손님을 찾기
            (e)=>
                e != entity 
                && e.TryGetComponent<GuestParty>(out otherGuest)
                && !otherGuest.isMoving
                && !otherGuest.hasToMove);
        if (otherEntity && otherGuest)
        {
            MergeTo(otherGuest);
        }
    }
    #region Event Handlers

    
    public void OnCustomerScreamModified(ScreamEventArgs screamEventArg)
    {
        AddFearSimple(screamEventArg.modifier + 1);
    }


    public void OnPlayerTurnEnter(TurnEventArgs e)
    {
        aliveTurnCnt += 1;
        fear = Mathf.CeilToInt(fear * 0.9f);
        // 감소량 = logcoefficient(aliveTurnCnt);
        float panicDecrease = Mathf.Log(aliveTurnCnt, fatigueCoefficient);
        panic = Mathf.Max(panic, panic - Mathf.RoundToInt(panicDecrease));
        // MoveBehaviour(); 이동은 GuestManager에서 처리
    }
    
    public void OnPlayerTurnExit(TurnEventArgs e)
    {
        
    }
    
    public void OnNonPlayerTurnEnter(TurnEventArgs e)
    {
        _screamedBefore = false;
    }
    
    public void OnNonPlayerTurnExit(TurnEventArgs e)
    {
        isCreatedNow = false;
    }
    

    #endregion

    public void OnFocus()
    {
        pathDrawer.enabled = true;
    }

    public void OnFocusLost()
    {
        pathDrawer.enabled = false;
    }
}
