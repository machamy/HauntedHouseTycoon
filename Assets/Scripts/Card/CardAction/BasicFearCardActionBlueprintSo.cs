
using System;
using System.Collections.Generic;
using Define;
using UnityEngine;


/// <summary>
/// 손님 입장 시 레벨에 따라 해당 공포를 부여하는 기본적인 카드 액션
/// </summary>
[CreateAssetMenu (menuName = "CardAction/SimpleFearCardAction")]
public class BasicFearCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class BasicFearCardAction : CardAction
    {
        public FearType fearType = FearType.None;
        public List<int> fearAmouts = new (){ 2,3,4 };
        
        public override bool OnGuestEnter(GuestMoveEventArgs gusetMoveEventArgs)
        {
            int idx = Mathf.Min(cardData.GetVariableInt(CardDataVariables.Key.CardLevel), fearAmouts.Count - 1);
            int fear = fearAmouts[idx];
            gusetMoveEventArgs.GuestParty.ApplyFear(fearType, fear, 0, 0);
            return breakChain;
        }
    }
    
    public override CardAction CreateCardAction()
    {
        return new BasicFearCardAction();
    }
    
    public override Type GetCardActionType()
    {
        return typeof(BasicFearCardAction);
    }
    
    
}

