
using System;
using UnityEngine;

// [Flags]
public enum Direction
{
    None = -1,
    L = 0,
    R = 1,
    U = 2,
    D = 3,
    Max,
    // None = 0,
    // L = 1 << 0,
    // R = 1 << 1,
    // U = 1 << 2,
    // D = 1 << 3,
    // MAX = (1 << 4) - 1,
}


public static class DirectionHelper
{
    public static Direction Clockwise(this Direction dir)
    {
        return (Direction)(((int)dir + 1) % (int)Direction.Max);
    }
    
    public static Direction CounterClockwise(this Direction dir)
    {
        return (Direction)(((int)dir + (int)Direction.Max - 1) % (int)Direction.Max);
    }
    
    public static Direction Opposite(this Direction dir)
    {
        return (Direction)(((int)dir + 2) % (int)Direction.Max);
    }
    
    public static Vector2Int ToVector2Int(this Direction dir)
    {
        switch (dir)
        {
            case Direction.L:
                return new Vector2Int(-1, 0);
            case Direction.R:
                return new Vector2Int(1, 0);
            case Direction.U:
                return new Vector2Int(0, 1);
            case Direction.D:
                return new Vector2Int(0, -1);
            default:
                return Vector2Int.zero;
        }
    }
}