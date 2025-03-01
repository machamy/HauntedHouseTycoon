
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerRoomEvent", menuName = "EventChannel/CustomerRoomEvent")]
public class CustomerRoomEventSO : ScriptableObject
{
   public delegate void CustomerRoomEneterEvent(GuestObject guestObject, Room room);
   public delegate void CustomerRoomExitEvent(GuestObject guestObject, Room room);
   public event CustomerRoomEneterEvent OnCustomerRoomEnter;
   public event CustomerRoomExitEvent OnCustomerRoomExit;
   
   public void RaiseCustomerRoomEnter(GuestObject guestObject, Room room)
   {
      OnCustomerRoomEnter?.Invoke(guestObject, room);
   }
   public void RaiseCustomerRoomExit(GuestObject guestObject, Room room)
   {
      OnCustomerRoomExit?.Invoke(guestObject, room);
   }
}
