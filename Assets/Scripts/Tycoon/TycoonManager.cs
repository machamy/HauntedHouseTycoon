
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
        if(Input.GetKeyDown(KeyCode.Space)) // 임시 테스트용
        {
            if(isPaused)
                Resume();
            else
                Pause();
        }
    }
    public bool initCameraToFieldCenter = false;
    public void StartCycle()
    {
        field.InitField(4, 3);
        if (initCameraToFieldCenter)
        {
            Vector3 centerPosition = field.GetCenterPosition();
            Camera.main.transform.position =
                new Vector3(centerPosition.x, centerPosition.y, Camera.main.transform.position.z);
        }
        handManager.deck = deck;
        deck.SetupForCycle();
    }
    
    public DataBase dataBase;
    
    [ContextMenu("InitDeckByDatabae")]
    public void InitDeckByDatabae()
    {
        foreach (var cardData in dataBase.cardDataList)
        {
            if(cardData.cardName == "Blank")
                continue;
            deck.AddCard(cardData.Clone() as CardData);
        }
    }
    
    [ContextMenu("SetHandManagerDeck")]
    public void SetHandManagerDeck()
    {
        handManager.deck = deck;
    }

    public void TestStart()
    {
        InitDeckByDatabae();
        StartCycle();
    }
}
