
using System;
using System.Collections;
using System.Collections.Generic;
using EventChannel;
using Pools;
using UI.GameUI;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 
/// </summary>
public class GuestManager : MonoBehaviour
{
    // [Header("Prefabs")]
    // [SerializeField] private 
    [Header("Channels")]
    [SerializeField] private CreateGuestChannelSO createGuestChannel;
    [SerializeField] private TurnEventChannelSO turnChannel;
    [SerializeField] private TurnEventChannelSO delayedTurnChannel;
    [FormerlySerializedAs("guestQueueBar")]
    [Header("References")]
    [SerializeField] private GuestQueueBarUI guestQueueBarUI;
    [FormerlySerializedAs("guests")]
    [Header("Guest Properties")]
    [SerializeField] private List<GuestObject> guestQueue = new List<GuestObject>(); // List가 조작에 용이, 큐쓴다고 해서 큰 이점이 없음
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
        Debug.Log($"NotifyGuestQueue {guestQueue.Count}");
        foreach (var guestObject in guestQueue)    
        {
            guestObject.HasToMove = true;
        }
        while (haveToMoveIndex < guestQueue.Count)
        {
            GuestObject guestObject = guestQueue[haveToMoveIndex];
            // 소환된 개체도 바로 이동하므로 주석처리
            // if (guest.IsCreatedNow) 
            // {
            //     continue;
            // }
            guestObject.MoveBehaviour();
            haveToMoveIndex++;
            yield return wait;
        }
    }
    
    // private void OnNonPlayerTurnExit()
    // {
    //     
    // }
    public GuestObject CreateGuest(Vector3 position = default)
    {
        GuestObject guestObject = PoolManager.Instance.Get(PoolManager.Poolables.Guest).GetComponent<GuestObject>();
        guestObject.transform.position = position;
        guestObject.OnCreate();
        guestQueue.Add(guestObject);
        guestQueueBarUI.AddGuestNode(guestObject);
        guestObject.OnRemoved += () =>
        {
            guestQueue.Remove(guestObject);
        };
        return guestObject;
    }
}
