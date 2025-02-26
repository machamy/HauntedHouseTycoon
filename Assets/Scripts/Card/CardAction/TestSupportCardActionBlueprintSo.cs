
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "CardAction/TestSupportCardActionSO", fileName = "Test Support Card Action SO")]
public class TestSupportCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class TestSupportCard : CardAction
    {
        public int fearGlobalAmount = 2;
        
        public override bool OnCardPlaced(Room room, CardData cardData)
        {
            cardData.nonlocalVariables.AddInt(CardDataVariables.Key.FearGlobalAmount, fearGlobalAmount);
            return false;
        }
    
        public override bool OnCardRemoved(Room room, CardData cardData)
        {
            cardData.nonlocalVariables.AddInt(CardDataVariables.Key.FearGlobalAmount, -fearGlobalAmount);
            return false;
        }
    }
    
    
    public override CardAction CreateCardAction()
    {
        return new TestSupportCard();
    }
    
    public override Type GetCardActionType()
    {
        return typeof(TestSupportCard);
    }
    
}
