using UnityEditor;
using UnityEngine;
using System;
using System.IO;

public class CardDataManager : MonoBehaviour
{
    private void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "AssetBundles/CardDataBase_JSON/CardData.json");

        var soCreator = new JsonToSO();
        var so = soCreator.CreateSO("CardDatabase", path) as CardDatabase;

        if (so != null)
        {
            var card = so.FindCardByIndex(1000300);

            if (card != null)
            {
                Debug.Log($"카드 찾음! Index: {card.CardType}, 이름 인덱스: {card.NameIndex}, Cost: {card.Cost}");
            }
            else
            {
                Debug.LogWarning("해당 인덱스를 가진 카드를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("ScriptableObject 생성 실패");
        }
    }
}