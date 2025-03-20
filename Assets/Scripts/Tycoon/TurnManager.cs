using System;
using DefaultNamespace;
using Define;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 턴 관리자
/// </summary>
public class TurnManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private IntVariableSO turnVariableSO;
    [FormerlySerializedAs("maxTurnDuration")]
    [Header("Settings")]
    [SerializeField] private float maxPlayerTurnDuration = 10f;
    [SerializeField] private float maxNonplayerTurnDuration = 4f;
    private float remainingTurnDuration = 10f;
    [SerializeField] private bool isPlayerTurn = true;
    public bool IsPlayerTurn => isPlayerTurn;
    [FormerlySerializedAs("turnIndicatingSlider")]
    [Header("References")]
    [SerializeField] private TurnIndicatingSliderUI turnIndicatingSliderUI;
    [Header("Events")]
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    // [SerializeField] private TurnEventChannelSO delayedTurnEventChannelSo;
    private void Start()
    {
        turnVariableSO.Value = 0;
        remainingTurnDuration = maxPlayerTurnDuration;
        // turnEventChannelSo.OnNonPlayerTurnEnter += () => { Debug.Log("NonPlayerTurnEnter"); };
        // turnEventChannelSo.OnNonPlayerTurnExit += () => { Debug.Log("NonPlayerTurnExit"); };
        // delayedTurnEventChannelSo.OnNonPlayerTurnEnter += () => { Debug.Log("DelayedNonPlayerTurnEnter"); };
        // delayedTurnEventChannelSo.OnNonPlayerTurnExit += () => { Debug.Log("DelayedNonPlayerTurnExit"); };
    }

    
    float turnDurationRatio;
    public void Update()
    {
        if(TycoonManager.Instance.IsPaused)
            return;
        remainingTurnDuration -= Time.deltaTime;
        if (isPlayerTurn)
        {
            turnDurationRatio = remainingTurnDuration / maxPlayerTurnDuration;
            if (remainingTurnDuration <= 0)
            {
                PlayerTurnEnd();
            }
        }
        else
        {
            turnDurationRatio = remainingTurnDuration / maxNonplayerTurnDuration;
            if (remainingTurnDuration <= 0)
            {
                PlayerTurnStart();
            }
        }
        turnIndicatingSliderUI.SetValue(turnDurationRatio);
    }

    /// <summary>
    /// 플레이어의 턴을 즉시 시작함
    /// </summary>
    public void PlayerTurnStart()
    {
        Debug.Log($"$Player Turn {turnVariableSO.Value} Start");
        isPlayerTurn = true;
        turnIndicatingSliderUI.isPlayerTurn = true;
        turnIndicatingSliderUI.UpdateColor();
        remainingTurnDuration = maxPlayerTurnDuration;
        turnVariableSO.Value = (turnVariableSO.Value + 1);

        TurnEventArgs args = TurnEventArgs.Get();
        args.turnType = TurnType.Npc;
        args.turnEventType = TurnEventType.Exit;
        turnEventChannelSo.RaiseNonPlayerTurnExitEvent(args);
        args.turnEventType = TurnEventType.Enter;
        turnEventChannelSo.RaiseNonPlayerTurnEnterEvent(args);
        args.turnType = TurnType.Player;
        args.turnEventType = TurnEventType.Enter;
        turnEventChannelSo.RaisePlayerTurnEnterEvent(args);
        args.turnEventType = TurnEventType.Exit;
    }

    /// <summary>
    /// 플레이어의 턴이 종료될 수 있다고 체크.
    /// </summary>
    public void ReadyToPlayerTurnEnd()
    {
        StartCoroutine(Wait());
        // TODO 애니메이션 종료 후 실행되도록 수정
        System.Collections.IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.2f);
            PlayerTurnEnd();
        }
    }
    
    /// <summary>
    /// 플레이어의 턴을 즉각 종료함.
    /// </summary>
    /// <remarks>
    /// 애니메이션 안전을 위해 ReadyToPlayerTurnEnd를 대신 호출할 것.
    /// </remarks>
    public void PlayerTurnEnd()
    {
        Debug.Log($"$Player Turn {turnVariableSO.Value} End");
        isPlayerTurn = false;
        turnIndicatingSliderUI.isPlayerTurn = false;
        turnIndicatingSliderUI.UpdateColor();
        remainingTurnDuration = maxNonplayerTurnDuration;
        
        TurnEventArgs args = TurnEventArgs.Get();
        args.turnType = TurnType.Player;
        args.turnEventType = TurnEventType.Exit;
        turnEventChannelSo.RaisePlayerTurnExitEvent(args);
        args.turnEventType = TurnEventType.Exit;
        args.turnType = TurnType.Npc;
        args.turnEventType = TurnEventType.Enter;
        turnEventChannelSo.RaiseNonPlayerTurnEnterEvent(args);
        args.turnEventType = TurnEventType.Exit;
    }
}
