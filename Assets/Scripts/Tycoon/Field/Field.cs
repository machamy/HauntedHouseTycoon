
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Field : MonoBehaviour, IEnumerable<Room>
{
    [SerializeField] private Room defaultRoomPrefab;
    [SerializeField] private CardDataSO defaultCardData;
    private Room[][] roomContainer;
    [SerializeField] private Grid grid;
    public Grid Grid => grid;
    
    [SerializeField,VisibleOnly] int width;
    [SerializeField,VisibleOnly] int height;
    
    public void InitField(int width, int height)
    {
        this.width = width;
        this.height = height;
        roomContainer = new Room[height][];
        var RoomHolder = FindFirstObjectByType<RoomHolder>();
        for (int y = 0; y < height; y++)
        {
            roomContainer[y] = new Room[width];
            for (int x = 0; x < width; x++)
            {
                var worldPosition = grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                var room = Instantiate(defaultRoomPrefab, worldPosition, Quaternion.identity);
                roomContainer[y][x] = room;
                room.transform.parent = transform;
                room.name = $"Room ({x}, {y})";
                room.Init(new Vector2Int(x, y), defaultCardData.OriginalCardData);
                // room.transform.localScale = grid.cellSize;
                if (RoomHolder != null)
                {
                    room.transform.SetParent(RoomHolder.transform);
                }
            }
        }
    }
    
    /// <summary>
    /// 중심 좌표 받아옴
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterPosition()
    {
        var LeftBottom = grid.GetCellCenterWorld(new Vector3Int(0, 0, 0));
        var RightTop = grid.GetCellCenterWorld(new Vector3Int(width - 1, height - 1, 0));
        return (LeftBottom + RightTop) / 2;
    }
    
    public Room WorldToRoom(Vector3 worldPosition)
    {
        var localPosition = transform.InverseTransformPoint(worldPosition);
        var cellPosition = grid.WorldToCell(localPosition);
        if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height)
            return null;
        return roomContainer[cellPosition.y][cellPosition.x];
    }
    public Vector2Int WorldToCoordinate(Vector3 worldPosition)
    {
        var localPosition = transform.InverseTransformPoint(worldPosition);
        var cellPosition = grid.WorldToCell(localPosition);
        return new Vector2Int(cellPosition.x, cellPosition.y);
    }
    
    public Room GetRoom(Vector2Int coordinate)
    {
        return GetRoom(coordinate.x, coordinate.y);
    }
    public Room GetRoom(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
        return roomContainer[y][x];
    }
    
    public Room GetRoomByDirection(Room room, Direction direction)
    {
        var coordinate = room.Coordinate;
        var nextCoordinate = coordinate + direction.ToVector2Int();
        return GetRoom(nextCoordinate);
    }
    
    public Vector3 GetRoomPosition(Vector2Int coordinate)
    {
        return GetRoomPosition(coordinate.x, coordinate.y);
    }
    
    public Vector3 GetRoomPosition(int x, int y)
    {
        return roomContainer[y][x].transform.position;
    }
    
    public Room[] this[int i]
    {
        get => roomContainer[i];
    }
    
    
    #if UNITY_EDITOR
    [ContextMenu("Init Field Editor")]
    public void InitFieldEditor()
    {
        width = 4;
        height = 4;
        roomContainer = new Room[height][];
        for (int y = 0; y < height; y++)
        {
            roomContainer[y] = new Room[width];
            for (int x = 0; x < width; x++)
            {
                var worldPosition = grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                var room = PrefabUtility.InstantiatePrefab(defaultRoomPrefab, transform) as Room;
                if (room != null)
                {
                    room.transform.position = worldPosition;
                    roomContainer[y][x] = room;
                    room.name = $"Room ({x}, {y})";
                    PrefabUtility.RecordPrefabInstancePropertyModifications(room);
                }
                else
                {
                    Debug.LogError("Failed to instantiate room prefab");
                }
            }
        }
        
    }
    
    #endif
    public IEnumerator<Room> GetEnumerator()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                yield return roomContainer[y][x];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
