﻿
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleScreamCardAction")]
public class SimpleScreamCardActionBlueprintSo : CardActionBlueprintSO
{
    
    [System.Serializable]
    public class SimpleScreamCardAction : CardAction
    {
        public override bool OnGuestEnter(GuestMoveEventArgs gusetMoveEventArgs)
        {
            if (gusetMoveEventArgs.GuestParty.CanScream)
            {
                gusetMoveEventArgs.GuestParty.Scream();
            }
            return breakChain;
        }
    }
    
    
    public override CardAction CreateCardAction()
    {
        return new SimpleScreamCardAction();
    }
    
    public override System.Type GetCardActionType()
    {
        return typeof(SimpleScreamCardAction);
    }
    
}
