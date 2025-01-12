
using System;
using UnityEngine;

[Serializable]
public class CardDataAgument : ICloneable
{
    public enum Key
    {
        None,
        CustomerAmount,
        FearEnterAmount,
        FearMovementCoefficient,
        FearScreamAmount,
        DecayTurnAmount,
        DecayTurnRemain,
        FearGlobalAmount,
    }

    public Key key; // string이 아닌 enum으로도 가능함.
    public string stringValue;
    public int intValue;
    public object Clone()
    {
        return new CardDataAgument
        {
            key = key,
            stringValue = stringValue,
            intValue = intValue
        };
    }
}
