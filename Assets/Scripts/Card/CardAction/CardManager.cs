using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class CardManager : MonoBehaviour
{
    public GameObject[] cardPrefab;  // 카드 프리팹
    public Transform cardParent;   // 카드들이 들어갈 부모 (GridLayoutGroup이 있는 Panel)
    public Card[,] cardGrid;
    void GenerateCards()
    {
        int rows = 4;
        int cols = 3;
        cardGrid = new Card[rows, cols]; 
        for (int i = 0; i < 12; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab[i], cardParent); // 카드 생성
            Card card = cardObj.GetComponent<Card>();
            int x = i % rows;
            int y = i / rows;
            card.SetPosition(new Vector2Int(x,y));
            cardGrid[x,y] = card;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
