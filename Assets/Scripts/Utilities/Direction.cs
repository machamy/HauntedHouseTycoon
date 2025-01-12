
using UnityEngine;

public enum Direction
{
    None = -1,
    L = 0,
    R,
    U,
    D,
    MAX
}


public static class DirectionHelper
{
    public static Direction Clockwise(this Direction dir)
    {
        return (Direction)(((int)dir + 1) % (int)Direction.MAX);
    }
    
    public static Direction CounterClockwise(this Direction dir)
    {
        return (Direction)(((int)dir + (int)Direction.MAX - 1) % (int)Direction.MAX);
    }
    
    public static Direction Opposite(this Direction dir)
    {
        return (Direction)(((int)dir + 2) % (int)Direction.MAX);
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