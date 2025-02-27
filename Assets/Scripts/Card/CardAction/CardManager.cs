using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class CardManager : MonoBehaviour
{
    public GameObject[] cardPrefab;  // ī�� ������
    public Transform cardParent;   // ī����� �� �θ� (GridLayoutGroup�� �ִ� Panel)
    public Card[,] cardGrid;
    void GenerateCards()
    {
        int rows = 4;
        int cols = 3;
        cardGrid = new Card[rows, cols]; 
        for (int i = 0; i < 12; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab[i], cardParent); // ī�� ����
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
