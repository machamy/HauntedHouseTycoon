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

        public ScriptableObject GetSOByIndex(string soListName, long index)
        {
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

    };
}