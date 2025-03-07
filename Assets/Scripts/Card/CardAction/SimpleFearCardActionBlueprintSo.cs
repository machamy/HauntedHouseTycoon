﻿
using System;
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleFearCardAction")]
public class SimpleFearCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class SimpleFearCardAction : CardAction
    {
        public int fearAmount = 2;
        
        public override bool OnGuestEnter(Room room, CardData cardData, GuestObject guestObject)
        {
            int fear = fearAmount;
            guestObject.AddFearSimple(fear);
            return breakChain;
        }
    }
    
    public override CardAction CreateCardAction()
    {
        return new SimpleFearCardAction();
    }
    
    public override Type GetCardActionType()
    {
        return typeof(SimpleFearCardAction);
    }
    
    
}

