
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
    // Vector2Int에 이미 선언되어있지만, 변동 가능성을 고려하여 별도로 선언
    public static readonly Vector2Int Up = new Vector2Int(0, 1);
    public static readonly Vector2Int Right = new Vector2Int(1, 0);
    public static readonly Vector2Int Down = new Vector2Int(0, -1);
    public static readonly Vector2Int Left = new Vector2Int(-1, 0);

    #region 회전
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
    #endregion
   

    public static Vector2Int ToVector2Int(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Up;
            case Direction.Right:
                return Right;
            case Direction.Down:
                return Down;
            case Direction.Left:
                return Left;
            default:
                return Vector2Int.zero;
        }
    }

    #region Flag 조작
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
    

    #endregion

    
    

    #region 기타 헬퍼 함수

    /// <summary>
    /// 들어간 방향을 기준으로, 다음 방향을 구한다.
    /// </summary>
    /// <param name="originDir"></param>
    /// <param name="candidates"></param>
    /// <returns></returns>
    public static Direction GetLeftmostDirection(this Direction originDir, DirectionFlag candidates)
    {
        if (candidates == DirectionFlag.None)
        {
            return Direction.None;
        }

        Direction res = originDir;
        
        for(int i = 0; i < 4; i++)
        {
            res = res.Clockwise();
            if ((candidates & res.ToFlag()) != 0)
            {
                return res;
            }
        }
        return Direction.None;
    }    

    #endregion
}