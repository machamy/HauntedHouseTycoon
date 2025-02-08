
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TycoonManager : SingletonBehaviour<TycoonManager>
{
    [SerializeField] private Field field;
    [SerializeField] private HandManager handManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Deck deck;
    
    public HandManager HandManager => handManager;
    public TurnManager TurnManager => turnManager;
    public Deck Deck => deck;
    public Field Field => field;
    
    [SerializeField] private bool isPaused = false;
    public bool IsPaused => isPaused;
    private void Start()
    {
    }
    
    public void Pause()
    {
        isPaused = true;
    }
    
    public void Resume()
    {
        isPaused = false;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isPaused)
                Resume();
            else
                Pause();
        }
    }
    
    public void StartCycle()
    {
        field.InitField(4, 3);
        Vector3 centerPosition = field.GetCenterPosition();
        Camera.main.transform.position = new Vector3(centerPosition.x, centerPosition.y, Camera.main.transform.position.z);
        handManager.deck = deck;
        deck.SetupForCycle();
    }
    
    public DataBase dataBase;
    public void InitDeckByDatabae()
    {
        foreach (var cardData in dataBase.cardDataList)
        {
            if(cardData.cardName == "Blank")
                continue;
            deck.AddCard(cardData.Clone() as CardData);
        }
    }

    public void TestStart()
    {
        InitDeckByDatabae();
        StartCycle();
    }
}
