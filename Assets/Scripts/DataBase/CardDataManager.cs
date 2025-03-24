using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System;
using ClassBase;
using ClassBase.Card;
using ClassBase.ListName;
using System.Runtime.CompilerServices;

namespace CardDataManager
{
    public class CardDataHelper
    {
        private static CardDatabaseHolder cardDatabaseHolder;

        public static void Initialize()
        {
            if (cardDatabaseHolder == null)
            {
                cardDatabaseHolder = UnityEngine.Object.FindFirstObjectByType<CardDatabaseHolder>();

                if (cardDatabaseHolder == null)
                {
                    Debug.LogError("CardDatabaseHolder를 찾을 수 없습니다.");
                }
            }
        }

        public static ScriptableObject GetSOByIndex(DatabaseType dbType, long index)
        {
            string soListName = dbType.ToString() + "List";

            if (cardDatabaseHolder.allDataDictionaries.ContainsKey(soListName))
            {
                if (cardDatabaseHolder.allDataDictionaries[soListName].TryGetValue(index, out ScriptableObject so))
                {
                    return so;
                }
            }

            Debug.LogError($"{soListName}에서 Index {index}를 찾을 수 없습니다.");
            return null;
        }
    }

    public static class SpriteLoader
    {
        private static string edgeSpritePath = "Sprite/CardEdgeSprite/";
        private static string illustSpritePath = "Sprite/CardIllustSprite";

        public static Sprite GetCardEdgeSprite(long index)
        {
            string fullPath = edgeSpritePath + index.ToString();

            Sprite loadedSprite = Resources.Load<Sprite>(fullPath);

            if (loadedSprite == null)
            {
                Debug.LogError($"Sprite '{fullPath}'을(를) 찾을 수 없습니다.");
            }

            return loadedSprite;
        }

        public static Sprite GetIllustSprite(long index)
        {
            string fullPath = illustSpritePath + index.ToString();

            Sprite loadedSprite = Resources.Load<Sprite>(fullPath);

            if (loadedSprite == null)
            {
                Debug.LogError($"Sprite '{fullPath}'을(를) 찾을 수 없습니다.");
            }

            return loadedSprite;
        }
    }
}