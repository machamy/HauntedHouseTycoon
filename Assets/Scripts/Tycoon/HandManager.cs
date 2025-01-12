
using System;
using UnityEngine;
using UnityEngine.Serialization;

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

    

    private void OnEnable()
    {
        turnEventChannel.OnTurnEnter += OnTurnEnter;
        turnEventChannel.OnTurnExit+= OnturnExit;
    }
    
    private void OnDisable()
    {
        turnEventChannel.OnTurnEnter -= OnTurnEnter;
        turnEventChannel.OnTurnExit -= OnturnExit;
    }
    
    private void OnTurnEnter()
    {
        for (int i = 0; i < drawCount; i++)
        {
            DrawCard();
        }
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
    }
    
    public void DiscardHand()
    {

    }
}
