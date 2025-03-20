
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerRoomEvent", menuName = "EventChannel/CustomerRoomEvent")]
public class GuestRoomEventSO : ScriptableObject
{
   // public delegate void CustomerRoomEneterEvent(GuestObject guestObject, Room room);
   // public delegate void CustomerRoomExitEvent(GuestObject guestObject, Room room);
   public readonly PriorityEvent<GuestMoveEventArgs> OnGuestRoomEnter = new PriorityEvent<GuestMoveEventArgs>();
   public readonly PriorityEvent<GuestMoveEventArgs> OnGuestRoomExit = new PriorityEvent<GuestMoveEventArgs>();
   
   public void RaiseCustomerRoomEnter(GuestMoveEventArgs e)
   {
      OnGuestRoomEnter?.Invoke(e);
   }
   public void RaiseCustomerRoomExit(GuestMoveEventArgs e)
   {
      OnGuestRoomExit?.Invoke(e);
   }
}
