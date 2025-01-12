
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Entity))]
public class Customer : MonoBehaviour
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
    [Header("References")]
    [SerializeField] private TextMeshPro fearText;
    
    private int movedDistance = 0;
    
    [SerializeField] Direction direction = Direction.None;
    
    
    public bool CanScream => fear >= screamRequirement;
    public int MovedDistance => movedDistance;

    private Room CurrentRoom => entity.currentRoom;

    private Field _field;
    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Start()
    {
        _field = TycoonManager.Instance.Field;
        UpdateText();
    }

    private void OnEnable()
    {
        screamEventChannel.OnScreamModified += OnCustomerScreamModified;
        turnEventChannelSo.OnTurnEnter += OnTurnEnter;
    }
    
    private void OnDisable()
    {
        screamEventChannel.OnScreamModified -= OnCustomerScreamModified;
        turnEventChannelSo.OnTurnEnter -= OnTurnEnter;
    }
    public void MoveBehaviour()
    {
        if (!CurrentRoom)
        {
            Debug.LogWarning($"Not at a Room");
            return;
        }
        
        // TODO: 다음 이동 값 구하는 방식 수정 필요, 현재는 이동 안될때가 생김
        
        Direction targetDir = GetFirstDirection(direction, CurrentRoom.CardData.directions);
        Room nextRoom = _field.GetRoomByDirection(CurrentRoom, targetDir);
        
        if (nextRoom)
        {
            roomEventChannelSo.RaiseCustomerRoomExit(this, CurrentRoom);
            entity.Move(nextRoom);
            transform.DOMove(nextRoom.transform.position, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                roomEventChannelSo.RaiseCustomerRoomEnter(this, nextRoom);
            });
            movedDistance++;
        }
        else
        {
            Debug.LogWarning($"No room in direction {targetDir}");
        }
    }

    private static Direction GetFirstDirection(Direction originDir, List<Direction> candidates)
    {
        if(Direction.None == originDir)
        {
            int RandomIndex = UnityEngine.Random.Range(0, candidates.Count);
            return candidates[RandomIndex];
        }
        Direction chkDir = originDir.Clockwise();
        for (int i = 0; i < 4; i++)
        {
            if (candidates.Contains(chkDir))
            {
                return chkDir;
            }
            chkDir = chkDir.Clockwise();
        }

        return Direction.None;
    }
    public void AddFear(int amount)
    {
        fear += amount;
        if (CanScream)
        {
            Scream();
        }
        UpdateText();
    }
    
    public void Scream()
    {
        Debug.Log($"{entity.name} is screaming!");
        screamEventChannel.RaiseScreamEvent(new ScreamEventArg(this,0));
        screamRequirement += screamRequirementIncrease;
        fear /= 2;
        UpdateText();
    }

    public void Exit()
    {
        Debug.Log($"{entity.name} is exiting!");
        Destroy(gameObject); // TODO 테스트용
    }


    public void UpdateText()
    {
        fearText.text = $"{fear} / {screamRequirement}";
    }
    #region Event Handlers

    
    public void OnCustomerScreamModified(ScreamEventArg screamEventArg)
    {
        AddFear(screamEventArg.modifier + 1);
    }


    public void OnTurnEnter()
    {
        fear = Mathf.CeilToInt(fear * 0.9f);
        MoveBehaviour();
    }
    #endregion

}
