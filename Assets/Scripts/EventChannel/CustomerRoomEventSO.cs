
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerRoomEvent", menuName = "EventChannel/CustomerRoomEvent")]
public class CustomerRoomEventSO : ScriptableObject
{
   public delegate void CustomerRoomEneterEvent(Guest guest, Room room);
   public delegate void CustomerRoomExitEvent(Guest guest, Room room);
   public event CustomerRoomEneterEvent OnCustomerRoomEnter;
   public event CustomerRoomExitEvent OnCustomerRoomExit;
   
   public void RaiseCustomerRoomEnter(Guest guest, Room room)
   {
      OnCustomerRoomEnter?.Invoke(guest, room);
   }
   public void RaiseCustomerRoomExit(Guest guest, Room room)
   {
      OnCustomerRoomExit?.Invoke(guest, room);
   }
}
