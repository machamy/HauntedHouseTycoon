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
                    Debug.LogError("CardDatabaseHolder�� ã�� �� �����ϴ�.");
                }
            }
        }

        public static ScriptableObject GetSOByIndex(string soListName, long index)
        {
            if (cardDatabaseHolder == null)
            {
                Debug.LogError("cardDatabaseHolder�� �ʱ�ȭ���� �ʾҽ��ϴ�. Initialize()�� ���� ȣ���ϼ���.");
                return null;
            }

            if (cardDatabaseHolder.allDataDictionaries.ContainsKey(soListName))
            {
                if (cardDatabaseHolder.allDataDictionaries[soListName].TryGetValue(index, out ScriptableObject so))
                {
                    return so;
                }
            }

            Debug.LogError($"{soListName}���� Index {index}�� ã�� �� �����ϴ�.");
            return null;
        }
    }
}