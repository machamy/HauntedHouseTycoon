
using System;
using System.Collections.Generic;
using UnityEngine;

// [Flags]
public enum Direction
{
    None = -1,
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
    Max,
}

[Flags]
public enum DirectionFlag
{
    None = 0,
    Up = 1 << 0,
    Right = 1 << 1,
    Down = 1 << 2,
    Left = 1 << 3,
    All = (1 << 4) - 1,
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
            case Direction.Up:
                return new Vector2Int(-1, 0);
            case Direction.Right:
                return new Vector2Int(1, 0);
            case Direction.Down:
                return new Vector2Int(0, 1);
            case Direction.Left:
                return new Vector2Int(0, -1);
            default:
                return Vector2Int.zero;
        }
    }
    public static List<Direction> ToList(this DirectionFlag flag)
    {
        List<Direction> list = new List<Direction>();
        for(int i = 0; i < (int)Direction.Max; i++)
        {
            if((flag & (DirectionFlag)(1 << i)) != 0)
            {
                list.Add((Direction)i);
            }
        }
        return list;
    }
    
    public static DirectionFlag ToFlag(this List<Direction> list)
    {
        DirectionFlag flag = DirectionFlag.None;
        foreach (var dir in list)
        {
            flag |= (DirectionFlag)(1 << (int)dir);
        }
        return flag;
    }
    
    public static DirectionFlag ToFlag(this Direction dir)
    {
        return (DirectionFlag)(1 << (int)dir);
    }
    
    public static DirectionFlag Clockwise(this DirectionFlag flag)
    {
        int res = (int)flag << 1;
        if (res > (int)DirectionFlag.All)
            res += 1;
        return (DirectionFlag)(res & (int)DirectionFlag.All);
    }
    
    public static DirectionFlag CounterClockwise(this DirectionFlag flag)
    {
        int res = (int)flag >> 1;
        if (((int)flag & 1) == 1)
            res |= (int)DirectionFlag.Left;
        return (DirectionFlag)(res & (int)DirectionFlag.All);
    }
}