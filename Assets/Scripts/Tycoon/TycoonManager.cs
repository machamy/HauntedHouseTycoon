
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TycoonManager : SingletonBehaviour<TycoonManager>
{
    [SerializeField] private Field field;
    [SerializeField] private HandManager handManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Deck deck;
    
    /// <summary>
    /// 타이쿤의 상태를 저장하고 확인할 수 있는 Context 클래스
    /// </summary>
    public class TycoonContext
    {
        public class TurnContext<T>
        {
            private List<List<T>> _data = new();
            private int count = 0;
            
            public int Count => count;
            public void Clear()
            {
                foreach (var turnData in _data)
                {
                    turnData.Clear();
                }
                count = 0;
            }
            public void AddTurnData(int turn, T data)
            {
                if (turn < 0)
                    return;
                while (_data.Count <= turn)
                {
                    _data.Add(new List<T>());
                }
                _data[turn].Add(data);
                count++;
            }
            public List<T> GetTurnData(int turn)
            {
                if (turn < 0 || turn >= _data.Count)
                    return null;
                return _data[turn];
            }
            public int GetTurnDataCount(int turn)
            {
                if (turn < 0 || turn >= _data.Count)
                    return 0;
                return _data[turn].Count;
            }
            public List<T> GetAllData()
            {
                List<T> allData = new List<T>();
                foreach (var turnData in _data)
                {
                    allData.AddRange(turnData);
                }
                return allData;
            }
        }
        TycoonManager _tycoonManager;
        TurnManager _turnManager;
        HandManager _handManager;
        Field _field;
        Deck _deck;
        
        TurnContext<CardData> usedCardContext = new TurnContext<CardData>();
        TurnContext<CardData> drawnCardContext = new TurnContext<CardData>();
        TurnContext<CardData> discardedCardContext = new TurnContext<CardData>();
        
        public TycoonContext(TycoonManager tycoonManager)
        {
            _tycoonManager = tycoonManager;
            _tycoonManager = tycoonManager;
            _handManager = tycoonManager.handManager;
            _field = tycoonManager.field;
            _deck = tycoonManager.deck;
        }
        
        public bool IsPlayerTurn => _turnManager.IsPlayerTurn;
        public int CurrentTurn => _turnManager.TurnCount;
        
        public int CardCountOnField
        {
            get
            {
                int count = 0;
                foreach (var room in _field)
                {
                    if (room.CardData.IsBlank())
                        count++;
                }
                return count;
            }
        }
        
        public int CardCountInDeck
        {
            get
            {
                int count = 0;
                foreach (var cardData in _deck.CardDataListRef)
                {
                    if (cardData.IsBlank())
                        count++;
                }
                return count;
            }
        }
        
        public int CardCountInHand
        {
            get
            {
                return _handManager.HandCount;
            }
        }
        
        public int CardCountInDiscard
        {
            get
            {
                int count = 0;
                foreach (var cardData in _deck.DiscardCardQueueRef)
                {
                    if (cardData.IsBlank())
                        count++;
                }
                return count;
            }
        }

        #region EventHandlers
        
        
        // TODO : EventManager 따로 두는게 나을듯?
        public void OnCardUsed(CardData cardData)
        {
            usedCardContext.AddTurnData(CurrentTurn, cardData);
        }
        public void OnCardDrawn(CardData cardData)
        {
            drawnCardContext.AddTurnData(CurrentTurn, cardData);
        }
        public void OnCardDiscarded(CardData cardData)
        {
            discardedCardContext.AddTurnData(CurrentTurn, cardData);
        }
        

        #endregion
    }

    private TycoonContext tycoonContext;
    public TycoonContext GetContext() => tycoonContext;
    public static TycoonContext Context => Instance.GetContext();
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
        tycoonContext = new TycoonContext(this);
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
        // InitDeckByDatabae();
        StartCycle();
    }
}

