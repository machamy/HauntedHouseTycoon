

using UnityEngine;

/// <summary>
/// 일정 턴이 지나면, 카드 비활성화(Action을 모두 제거)
/// 체크 시점은 NPC의 턴이 끝날 때
/// </summary>
[CreateAssetMenu (menuName = "CardAction/SimpleDecayCardAction")]
public class SimpleDecayCardActionSO : BaseCardActionSO
{
    public override bool OnCardPlaced(Room room, CardData cardData)
    {
        int DecayTurnAmount = cardData.GetArgumentInt(CardDataAgument.Key.DecayTurnAmount);
        cardData.SetArgumentInt(CardDataAgument.Key.DecayTurnRemain, DecayTurnAmount);
        Debug.Log($"[SimpleDecayCardActionSO::OnCardPlaced] {room.name} : {DecayTurnAmount} turns remain");
        Debug.Log($"[SimpleDecayCardActionSO::OnCardPlaced] {room.name} : ({cardData.GetArgumentInt(CardDataAgument.Key.DecayTurnRemain)})");
        return breakChain;
    }

    public override bool OnNpcTurnExit(Room room, CardData cardData)
    {
        int decayTurnRemain = cardData.GetArgumentInt(CardDataAgument.Key.DecayTurnRemain);
        cardData.SetArgumentInt(CardDataAgument.Key.DecayTurnRemain, decayTurnRemain-1);
        if(decayTurnRemain <= 0)
        {
            Debug.Log($"[SimpleDecayCardActionSO::OnTurnExit] {room.name} : Card decayed({decayTurnRemain})");
            cardData.CleanAction();
            // TODO: 디버그용 색변경, 스프라이트 등을 변경해야함
            room.FocusColor(new Color(0.4f, 0.4f, 0.4f, 0.5f)); 
            return true;
        }
        return breakChain;
    }
}
