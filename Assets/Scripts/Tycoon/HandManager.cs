
using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 실제 핸드를 관리함
/// </summary>
public class HandManager : MonoBehaviour
{
    [SerializeReference] public Deck deck;
    [Header("Settings")]
    public int maxHandSize = 5;
    public int drawCount = 1;
    [FormerlySerializedAs("cardHolder")]
    [Header("References")]
    [SerializeField]
    private HorizontalCardHolder handCardHolder;
    [Header("Events")]
    [SerializeField] private TurnEventChannelSO turnEventChannel;
    

    
    public int HandCount => handCardHolder.CardCount;
    public void Initialize()
    {
        
    }

    private void OnEnable()
    {

    }
    
    private void OnDisable()
    {

    }
    
    private void OnPlayerTurnEnter()
    {
        // for (int i = 0; i < drawCount; i++)
        // {
        //     DrawCard();
        // }
    }
    
    private void OnturnExit()
    {
        
    }
    
    public void DrawCard()
    {
        CardData cardData = deck.DrawCard();
        if (cardData == null)
            return;
        handCardHolder.AddCardWithSlot(cardData);
        cardData.returnDeck = deck;
        
        TycoonManager.Context.OnCardDrawn(cardData);
    }
    
    public void DiscardHand()
    {
    
    }
}
