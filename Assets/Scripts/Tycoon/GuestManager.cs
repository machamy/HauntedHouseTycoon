
using System;
using System.Collections;
using System.Collections.Generic;
using EventChannel;
using UI.GameUI;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 
/// </summary>
public class GuestManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Guest guestPrefab; // FactorySO로 이양 가능
    [Header("Channels")]
    [SerializeField] private CreateGuestChannelSO createGuestChannel;
    [SerializeField] private TurnEventChannelSO turnChannel;
    [SerializeField] private TurnEventChannelSO delayedTurnChannel;
    [FormerlySerializedAs("guestQueueBar")]
    [Header("References")]
    [SerializeField] private GuestQueueBarUI guestQueueBarUI;
    [FormerlySerializedAs("guests")]
    [Header("Guest Properties")]
    [SerializeField] private List<Guest> guestQueue = new List<Guest>(); // List가 조작에 용이, 큐쓴다고 해서 큰 이점이 없음
    [Header("Settings")]
    [SerializeField] private float guestNotifyInterval = 0.2f;

    private void OnEnable()
    {
        createGuestChannel.RegisterListener(CreateGuest);
        // turnChannel.OnPlayerTurnEnter += OnPlayerTurnEnter; 
        // turnChannel.OnPlayerTurnExit += OnPlayerTurnExit;
        // turnChannel.OnNonPlayerTurnEnter += OnNonPlayerTurnEnter;
        // turnChannel.OnNonPlayerTurnExit += OnNonPlayerTurnExit;
        
        
        delayedTurnChannel.OnNonPlayerTurnEnter += OnDelayedNonPlayerTurnEnter;
    }
    
    private void OnDisable()
    {
        createGuestChannel.UnregisterListener(CreateGuest);
        // turnChannel.OnPlayerTurnEnter -= OnPlayerTurnEnter;
        // turnChannel.OnPlayerTurnExit -= OnPlayerTurnExit;
        // turnChannel.OnNonPlayerTurnEnter -= OnNonPlayerTurnEnter;
        // turnChannel.OnNonPlayerTurnExit -= OnNonPlayerTurnExit;
        
        
        delayedTurnChannel.OnNonPlayerTurnEnter -= OnDelayedNonPlayerTurnEnter;
    }

    // private void OnPlayerTurnEnter()
    // {
    //     
    // }
    //
    // private void OnPlayerTurnExit()
    // {
    //     
    // }
    
    private int haveToMoveIndex = 0;
    private void OnDelayedNonPlayerTurnEnter()
    {
        haveToMoveIndex = 0;
        StartCoroutine(NotifyGuestQueue());
    }
    
    private IEnumerator NotifyGuestQueue()
    {
        WaitForSeconds wait = new WaitForSeconds(guestNotifyInterval);
        while (haveToMoveIndex < guestQueue.Count)
        {
            Guest guest = guestQueue[haveToMoveIndex];
            // 소환된 개체도 바로 이동하므로 주석처리
            // if (guest.IsCreatedNow) 
            // {
            //     continue;
            // }
            guest.MoveBehaviour();
            haveToMoveIndex++;
            yield return wait;
        }
    }
    
    // private void OnNonPlayerTurnExit()
    // {
    //     
    // }
    public Guest CreateGuest(Vector3 position = default)
    {
        Guest guest = Instantiate(guestPrefab);
        guest.transform.position = position;
        guest.OnCreate();
        guestQueue.Add(guest);
        guestQueueBarUI.AddGuestNode(guest);
        return guest;
    }
}
