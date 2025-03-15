
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PathDrawer : MonoBehaviour
{
    Field _field;
    [Header("Current Object")]
    [SerializeField] Room _room;
    [SerializeField,Tooltip("GuestObj가 있으면 Room을 덮어씌움")] GuestObject _guestObject;
    [SerializeField] Direction _startDirection;
    [SerializeField] int depth = 5;
    
    public GuestObject GuestObject {
        get => _guestObject;
        set
        {
            _guestObject = value;
            if(enabled)
                UpdateDraw();
        }
    }
    
    [Header("LineRenderer")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Color _lineStartColor = Color.red;
    [SerializeField] private Color _lineEndColor = Color.green;
    [SerializeField] private float width = 0.2f;
    [SerializeField] private float _lineAbsY = 2f;
    private void Awake()
    {
        _field = FindFirstObjectByType<Field>();
        if(TryGetComponent(out Room room))
        {
            _room = room;
            _startDirection = Direction.Right;
        }
        else if(TryGetComponent(out GuestObject guestObject))
        {
            _guestObject = guestObject;
            _room = _guestObject.CurrentRoom;
        }
        
        if(TryGetComponent(out LineRenderer lineRenderer))
        {
            _lineRenderer = lineRenderer;
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
        }
        else
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
    }

    public void LateUpdate()
    {
        UpdateDraw(); //TODO: 필요할때만 호출 ex: 더티플래그(방 변경, 이동 등)
    }

    private void OnEnable()
    {
        UpdateDraw();
    }
    private void OnDisable()
    {
        _lineRenderer.enabled = false;
    }

    public void UpdateDraw()
    {

        if (_guestObject)
        {
            _room = _guestObject.CurrentRoom;
            _startDirection = _guestObject.OrientingDirection;
        }
        if (!_room)
        {
            return;
        }
        UpdatePath();
        if (path.Count <= 1)
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = false;
            return;
        }
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = path.Count;
        _lineRenderer.startColor = _lineStartColor;
        _lineRenderer.endColor = _lineEndColor;

        _rooms.Clear();
        _directions.Clear();

        for (int i = 0; i < path.Count; i++)
        {
            _rooms.Add(path[i].Item1);
            _directions.Add(path[i].Item2);
        }
        if (_guestObject)
        {
            Vector3 pos = _guestObject.transform.position;
            pos.y = _lineAbsY;
            _lineRenderer.SetPosition(0, pos);
        }
        else
        {
            Vector3 pos = path[0].Item1.transform.position;
            pos.y = _lineAbsY;
            _lineRenderer.SetPosition(0, pos);
        }
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 pos = path[i].Item1.transform.position;
            pos.y = _lineAbsY;
            _lineRenderer.SetPosition(i, pos);
        }
    } 
    [Header("연산 결과")]
    [SerializeField,VisibleOnly] private List<Room> _rooms = new ();
    [SerializeField,VisibleOnly] private List<Direction> _directions = new ();
    List<Tuple<Room,Direction>> path = new ();
    public void UpdatePath()
    {
        path.Clear();
        Room currentRoom = _room;
        Direction currentDirection = _startDirection;
        int currentDepth = 0;
        while (depth == -1 || currentDepth < depth)
        {
            currentDepth++;
            var toAdd = new Tuple<Room, Direction>(currentRoom, currentDirection);
            if (path.Contains(toAdd))
            {
                break;
            }
            path.Add(toAdd);
            Room nxtRoom = currentRoom.FindLeftmostRoom(_field, currentDirection, out Direction nextDirection);
            if (nxtRoom == null)
            {
                break;
            }
            currentRoom = nxtRoom;
            currentDirection = nextDirection;
        }
    }
}
