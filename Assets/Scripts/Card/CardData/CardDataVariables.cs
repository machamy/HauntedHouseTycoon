
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardDataVariables 
{
    public List<CardDataVariable> arguments = new List<CardDataVariable>();
    
    public void Clear() => arguments.Clear();
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
        
        PrevGuestDirection,
    }
    
    [Serializable]
    public class CardDataVariable : ICloneable
    {
        public Key key; // string이 아닌 enum으로도 가능함.
        public string stringValue;
        public int intValue;
        public object Clone()
        {
            return new CardDataVariable
            {
                key = key,
                stringValue = stringValue,
                intValue = intValue
            };
        }
    }
    
    public CardDataVariable GetVariable(Key key)
    {
        foreach (var agument in arguments)
        {
            if (agument.key == key)
            {
                return agument;
            }
        }
        return null;
    }
    
    public string GetString(Key key)
    {
        var variable = GetVariable(key);
        if (variable == null)
        {
            return string.Empty;
        }
        return variable.stringValue;
    }
    
    public int GetInt(Key key)
    {
        var variable = GetVariable(key);
        if (variable == null)
        {
            return 0;
        }
        return variable.intValue;
    }
    
    public int GetIntDefault(Key key, int defaultValue)
    {
        var variable = GetVariable(key);
        if (variable == null)
        {
            return defaultValue;
        }
        return variable.intValue;
    }
    
    public void SetString(Key key, string value)
    {
        var variable = GetVariable(key);
        if (variable == null)
        {
            variable = new CardDataVariable();
            variable.key = key;
            arguments.Add(variable);
        }
        variable.stringValue = value;
    }
    
    public void SetInt(Key key, int value)
    {
        var variable = GetVariable(key);
        if (variable == null)
        {
            variable = new CardDataVariable();
            variable.key = key;
            arguments.Add(variable);
        }
        variable.intValue = value;
    }
    
    public void AddInt(Key key, int value)
    {
        var variable = GetVariable(key);
        if (variable == null)
        {
            variable = new CardDataVariable();
            variable.key = key;
            arguments.Add(variable);
        }
        variable.intValue += value;
    }
    
    
    public void CopyFrom(CardDataVariables target)
    {
        Clear();
        foreach (var argument in target.arguments)
        {
            arguments.Add((CardDataVariable)argument.Clone());
        }
    }
}
