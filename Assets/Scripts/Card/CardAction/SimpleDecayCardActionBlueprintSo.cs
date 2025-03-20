

using System;
using UnityEngine;

/// <summary>
/// 일정 턴이 지나면, 카드 비활성화(Action을 모두 제거)
/// 체크 시점은 NPC의 턴이 끝날 때
/// </summary>
[CreateAssetMenu (menuName = "CardAction/SimpleDecayCardAction")]
public class SimpleDecayCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class SimpleDecayCardAction : CardAction
    {
        public int DecayTurnAmount = 3;
        public int DecayTurnRemain = 3;
        
        public override bool OnCardPlaced(CardEventArgs cardEvent)
        {
            DecayTurnRemain = DecayTurnAmount;
            // Debug.Log($"[SimpleDecayCardActionSO::OnCardPlaced] {room.name} : {DecayTurnAmount} turns remain");
            // Debug.Log($"[SimpleDecayCardActionSO::OnCardPlaced] {room.name} : ({variables.GetInt(CardDataVariables.Key.DecayTurnRemain)})");
            return breakChain;
        }

        public override bool OnNpcTurnExit(TurnEventArgs turnEvent)
        {
            DecayTurnRemain -= 1;
            if(DecayTurnRemain <= 0)
            {
                Debug.Log($"[SimpleDecayCardActionSO::OnTurnExit] {room.name} : Card decayed({DecayTurnRemain})");
                cardData.CleanAction();
                // TODO: 디버그용 색변경, 스프라이트 등을 변경해야함
                room.FocusColor(new Color(0.4f, 0.4f, 0.4f, 0.5f)); 
                return true;
            }
            return breakChain;
        }
    }
    
    
    public override CardAction CreateCardAction()
    {
        return new SimpleDecayCardAction();
    }
    
    public override Type GetCardActionType()
    {
        return typeof(SimpleDecayCardAction);
    }
   
}
