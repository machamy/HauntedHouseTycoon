
using System;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    Field _field;
    [SerializeField] Room _room;
    [SerializeField] GuestObject _guestObject;

    [SerializeField] Direction _startDirection;
    [SerializeField] int depth = 5;
    
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Color _lineStartColor = Color.red;
    [SerializeField] private Color _lineEndColor = Color.green;
    [SerializeField] private float width = 0.1f;
    [SerializeField] private float _lineDeltaY = 0.1f;
    private void Awake()
    {
        _field = FindObjectOfType<Field>();
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
    public void Update()
    {
       
        if (_room == null)
        {
            return;
        }
        if (_guestObject != null)
        {
            _room = _guestObject.CurrentRoom;
            _startDirection = _guestObject.OrientingDirection;
        }
        
        UpdatePath();
        
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
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = path[i].Item1.transform.position;
            pos.y += _lineDeltaY;
            _lineRenderer.SetPosition(i, pos);
        }

        
    }
    [SerializeField] private List<Room> _rooms = new ();
    [SerializeField] private List<Direction> _directions = new ();
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
