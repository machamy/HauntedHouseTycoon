
using UnityEngine;

/// <summary>
/// Field 좌표계로 움직이는 객체
/// </summary>
public class Entity : MonoBehaviour
{
    private Vector2Int coordinate;
    public Room currentRoom;
    public Vector2Int Coordinate => coordinate;
    
    
    public void Move(Room room)
    {
        if (currentRoom)
        {
            if (currentRoom.HasEntity(this))
            {
                currentRoom.RemoveEntity(this);
            }
            else
            {
                Debug.LogError($"엔티티가 {currentRoom.name}에서 제대로 삭제되지 않음");
            }
        }
        currentRoom = room;
        currentRoom.AddEntity(this);
        coordinate = room.Coordinate;
        // transform.position = room.transform.position;
    }
    
    public void MoveWithTransform(Room room)
    {
        Move(room);
        transform.position = room.transform.position;
    }
    
    public void OnRemoved()
    {
        if (currentRoom)
        {
            currentRoom.RemoveEntity(this);
            currentRoom = null;
        }
    }
}
