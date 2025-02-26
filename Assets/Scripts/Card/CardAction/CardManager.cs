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
        int rows = 3;
        int cols=4;
        cardGrid = new Card[rows, cols]; 
        for (int i = 0; i < 12; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab[i], cardParent); // ī�� ����
            Card card = cardObj.GetComponent<Card>();
            int x = i % cols;
            int y = (i / cols);
            card.SetPosition(new Vector2Int(x, y));
            cardGrid[x, y] = card;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateCards();
        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 3; y++)
                Debug.Log(cardGrid[x, y]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
