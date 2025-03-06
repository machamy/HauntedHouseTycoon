
using Define;

/// <summary>
/// 손님의 정보를 담고 있음
/// 현재는 DB데이터를 기반으로 작성되어있으나
/// 게임속 필요한 정보는 담는 방식으로 추후 수정가능
/// </summary>
[System.Serializable]
public class GuestData
{
    public long id;
    public string name;
    public Sex sex;
    public int age;
    public long[] traumaIds;
    public float[] traumaRatios;
    public float[] fearResistances;
    public int[] fearRequirements;
    public int panicValue;
    public float fatigueCoefficientPerTurn;
    public int[] panicCoefficients;
    public int exitScreamAmount;
    public long[] entranceCardIds;
    public string prefabPath;
    
    public float VisionFearResistance => fearResistances[(int)FearType.Vision];
    public float HearingFearResistance => fearResistances[(int)FearType.Hearing];
    public float TouchFearResistance => fearResistances[(int)FearType.Touch];
    public float SmellFearResistance => fearResistances[(int)FearType.Smell];
}
