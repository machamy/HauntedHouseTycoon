using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "CardpackDataSO")]
public class CardpackDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.CardPackData> cardpackDataList = new List<ClassBase.Card.CardPackData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray cardpackDataArray = JArray.Parse(json);

        cardpackDataList.Clear();

        foreach(JToken token in cardpackDataArray)
        {
            JObject cardpackDataObj = (JObject)token;

            long[] appearingCardIndex = TypeConverter.ExtractLongArray(cardpackDataObj, "appearingCardIndex");

            int[] weightedRatioForEachCards = TypeConverter.ExtractIntArray(cardpackDataObj, "weightedRatioForEachCards");

            var newcardpackData = new ClassBase.Card.CardPackData
            {
                Index = TypeConverter.TryParseLong(cardpackDataObj["index"]?.ToString(), 0),
                NameIndex = TypeConverter.TryParseLong(cardpackDataObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TypeConverter.TryParseLong(cardpackDataObj["explainIndex"]?.ToString(), 0),
                PackPrice = TypeConverter.TryParseInt(cardpackDataObj["packPrice"]?.ToString(), 0),
                NumberOfCardsAppearingUponOpening = TypeConverter.TryParseInt(cardpackDataObj["numberOfCardsaAppearingUponOning"]?.ToString(), 0),
                AppearingCardIndex = appearingCardIndex,
                WeightedRatioForEachCards = weightedRatioForEachCards,
                RepurchaseAllowed = TypeConverter.TryParseBool(cardpackDataObj["repurchaseAllowed"]?.ToString(), false),
                RepurchaseAllowedForOnce = TypeConverter.TryParseBool(cardpackDataObj["repurchaseAllowedForOnce"]?.ToString(), false),
                MaximumOpenedPackAmount = TypeConverter.TryParseInt(cardpackDataObj["maximumOpenedPackAmount"]?.ToString(), 0),
                PackOpeningAnimation = TypeConverter.TryParseLong(cardpackDataObj["packOpeningAnimation"]?.ToString(), 0),
                IllustFileName = cardpackDataObj["illustFileName"]?.ToString() ?? ""
            };

            cardpackDataList.Add(newcardpackData);
        }
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

#else
        SaveForAPI();
#endif
    }
    private void SaveForAPI()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "SavedAnimationData.json");
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(cardpackDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}