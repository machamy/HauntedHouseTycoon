
using System;
using System.Collections.Generic;
using Define;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 손님의 정보를 담고 있음
/// 현재는 DB데이터를 기반으로 작성되어있으나
/// 게임속 필요한 정보는 담는 방식으로 추후 수정가능
/// </summary>
[System.Serializable]
public class GuestData : ICopyable<GuestData>
{
    public long id;
    public string name;
    public Sex sex;
    public int age;
    public SerialzableDict<long, float> traumaRatios = new SerialzableDict<long, float>();
    public List<float> fearResistances = new List<float>() {0, 0, 0, 0};
    [FormerlySerializedAs("fearRequirements")] public List<int> screamRequirements = new List<int>();
    public int panicValue;
    public float fatigueCoefficientPerTurn;
    public List<int> panicCoefficients = new List<int>();
    public int exitScreamAmount;
    public List<long> entranceCardIds = new List<long>();
    public string prefabPath;
    
    
    public float VisionFearResistance => fearResistances[(int)FearType.Vision];
    public float HearingFearResistance => fearResistances[(int)FearType.Hearing];
    public float TouchFearResistance => fearResistances[(int)FearType.Touch];
    public float SmellFearResistance => fearResistances[(int)FearType.Smell];
    
    public void CopyTo(GuestData target)
    {
        target.id = id;
        target.name = name;
        target.sex = sex;
        target.age = age;
        target.traumaRatios.Clear();
        foreach (KeyValuePair<long, float> ratio in traumaRatios)
        {
            target.traumaRatios.Add(ratio.Key, ratio.Value);
        }
        target.fearResistances.Clear();
        target.fearResistances.AddRange(fearResistances);
        target.screamRequirements.Clear();
        target.screamRequirements.AddRange(screamRequirements);
        target.panicValue = panicValue;
        target.fatigueCoefficientPerTurn = fatigueCoefficientPerTurn;
        target.panicCoefficients.Clear();
        target.panicCoefficients.AddRange(panicCoefficients);
        target.exitScreamAmount = exitScreamAmount;
        target.entranceCardIds.Clear();
        target.entranceCardIds.AddRange(entranceCardIds);
        target.prefabPath = prefabPath;
    }

    public void CopyFrom(GuestData target)
    {
        target.CopyTo(this);
    }
}
