using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

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
    [Header("References")]
    [SerializeField] private TurnIndicatingSlider turnIndicatingSlider;
    [Header("Events")]
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    [SerializeField] private TurnEventChannelSO delayedTurnEventChannelSo;
    private void Start()
    {
        turnVariableSO.Value = 0;
        remainingTurnDuration = maxPlayerTurnDuration;
        turnEventChannelSo.OnNonPlayerTurnEnter += () => { Debug.Log("NonPlayerTurnEnter"); };
        turnEventChannelSo.OnNonPlayerTurnExit += () => { Debug.Log("NonPlayerTurnExit"); };
        delayedTurnEventChannelSo.OnNonPlayerTurnEnter += () => { Debug.Log("DelayedNonPlayerTurnEnter"); };
        delayedTurnEventChannelSo.OnNonPlayerTurnExit += () => { Debug.Log("DelayedNonPlayerTurnExit"); };
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
        turnIndicatingSlider.SetValue(turnDurationRatio);
    }

    public void PlayerTurnStart()
    {
        Debug.Log($"$Player Turn {turnVariableSO.Value} Start");
        isPlayerTurn = true;
        turnIndicatingSlider.isPlayerTurn = true;
        turnIndicatingSlider.UpdateColor();
        remainingTurnDuration = maxPlayerTurnDuration;
        turnVariableSO.Value = (turnVariableSO.Value + 1);
        turnEventChannelSo.RaiseNonPlayerTurnExitEvent();
        delayedTurnEventChannelSo.RaiseNonPlayerTurnExitEvent();
        turnEventChannelSo.RaisePlayerTurnEnterEvent();
        delayedTurnEventChannelSo.RaisePlayerTurnEnterEvent();
    }

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
    
    public void PlayerTurnEnd()
    {
        Debug.Log($"$Player Turn {turnVariableSO.Value} End");
        isPlayerTurn = false;
        turnIndicatingSlider.isPlayerTurn = false;
        turnIndicatingSlider.UpdateColor();
        remainingTurnDuration = maxNonplayerTurnDuration;
        turnEventChannelSo.RaisePlayerTurnExitEvent();
        delayedTurnEventChannelSo.RaisePlayerTurnExitEvent();
        turnEventChannelSo.RaiseNonPlayerTurnEnterEvent();
        delayedTurnEventChannelSo.RaiseNonPlayerTurnEnterEvent();
    }
}
