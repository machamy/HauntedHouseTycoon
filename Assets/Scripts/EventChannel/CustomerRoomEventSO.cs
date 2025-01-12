
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerRoomEvent", menuName = "EventChannel/CustomerRoomEvent")]
public class CustomerRoomEventSO : ScriptableObject
{
   public delegate void CustomerRoomEneterEvent(Customer customer, Room room);
   public delegate void CustomerRoomExitEvent(Customer customer, Room room);
   public event CustomerRoomEneterEvent OnCustomerRoomEnter;
   public event CustomerRoomExitEvent OnCustomerRoomExit;
   
   public void RaiseCustomerRoomEnter(Customer customer, Room room)
   {
      OnCustomerRoomEnter?.Invoke(customer, room);
   }
   public void RaiseCustomerRoomExit(Customer customer, Room room)
   {
      OnCustomerRoomExit?.Invoke(customer, room);
   }
}
