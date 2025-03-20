using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System;
using ClassBase;
using ClassBase.Card;

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

        public static ScriptableObject GetSOByIndex(string soListName, long index)
        {
            if (cardDatabaseHolder == null)
            {
                Debug.LogError("cardDatabaseHolder가 초기화되지 않았습니다. Initialize()를 먼저 호출하세요.");
                return null;
            }

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
}