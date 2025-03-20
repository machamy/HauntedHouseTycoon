using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "CardDataBaseSO")]
public class CardDataBaseSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.CardClass> cardDataBaseList = new List<ClassBase.Card.CardClass>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray cardArray = JArray.Parse(json);

        cardDataBaseList.Clear();


        foreach (JToken token in cardArray)
        {
            JObject cardObj = (JObject)token;

            int[] availableRoutes = TypeConverter.ExtractIntArray(cardObj, "availableRoutes");
            int[] input1 = TypeConverter.ExtractIntArray(cardObj, "input1");
            int[] input2 = TypeConverter.ExtractIntArray(cardObj, "input2");
            int[] input3 = TypeConverter.ExtractIntArray(cardObj, "input3");
            int[] input4 = TypeConverter.ExtractIntArray(cardObj, "input4");

            long[] keywordIndex = TypeConverter.ExtractLongArray(cardObj, "keywordIndex");
            long[] cardEffectIndex = TypeConverter.ExtractLongArray(cardObj, "cardEffectIndex");
            long[] placeAnimationIndex = TypeConverter.ExtractLongArray(cardObj, "placeAnimationIndex");
            long[] actionAnimationIndex = TypeConverter.ExtractLongArray(cardObj, "actionAnimationIndex");

            var newCard = new ClassBase.Card.CardClass
            {
                Index = TypeConverter.TryParseLong(cardObj["index"]?.ToString(), 0),
                NameIndex = TypeConverter.TryParseLong(cardObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TypeConverter.TryParseLong(cardObj["explainIndex"]?.ToString(), 0),

                CardType = (ClassBase.Card.CardClass.Type)TypeConverter.TryParseInt(cardObj["type"]?.ToString(), -1),
                CardRank = (ClassBase.Card.CardClass.Rank)TypeConverter.TryParseInt(cardObj["rank"]?.ToString(), -1),

                Cost = TypeConverter.TryParseInt(cardObj["cost"]?.ToString(), 0),
                KeyWordIndex = keywordIndex,
                SpritePath = cardObj["spritePath"]?.ToString() ?? "",

                AvailableRoutes = availableRoutes,
                DestroyPayback = TypeConverter.TryParseInt(cardObj["destroyPayback"]?.ToString(), 0),
                CardEffectIndex = cardEffectIndex,
                PlaceAnimationIndex = placeAnimationIndex,
                ActionAnimationIndex = actionAnimationIndex,

                input1 = input1,
                input2 = input2,
                input3 = input3,
                input4 = input4
            };

            cardDataBaseList.Add(newCard);
        }
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
#else
        SaveForAPI();
#endif
    }
    private void SaveForAPI()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "SavedAnimationData.json");
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(cardDataBaseList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}