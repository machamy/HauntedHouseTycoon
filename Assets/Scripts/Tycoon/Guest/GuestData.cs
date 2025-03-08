
using System;
using Define;

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
    public long[] traumaIds = new long[4];
    public float[] traumaRatios = new float[4];
    public float[] fearResistances = new float[4];
    public int[] fearRequirements = new int[4];
    public int panicValue;
    public float fatigueCoefficientPerTurn;
    public int[] panicCoefficients = new int[4];
    public int exitScreamAmount;
    public long[] entranceCardIds = new long[4];
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
        Array.Copy(traumaIds, target.traumaIds, traumaIds.Length);
        Array.Copy(traumaRatios, target.traumaRatios, traumaRatios.Length);
        Array.Copy(fearResistances, target.fearResistances, fearResistances.Length);
        Array.Copy(fearRequirements, target.fearRequirements, fearRequirements.Length);
        target.panicValue = panicValue;
        target.fatigueCoefficientPerTurn = fatigueCoefficientPerTurn;
        Array.Copy(panicCoefficients, target.panicCoefficients, panicCoefficients.Length);
        target.exitScreamAmount = exitScreamAmount;
        Array.Copy(entranceCardIds, target.entranceCardIds, entranceCardIds.Length);
        target.prefabPath = prefabPath;
    }

    public void CopyFrom(GuestData target)
    {
        target.CopyTo(this);
    }
}
