
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
        currentRoom = room;
        coordinate = room.Coordinate;
        transform.position = room.transform.position;
    }
    
    public void MoveWithTransform(Room room)
    {
        Move(room);
        transform.position = room.transform.position;
    }
}
