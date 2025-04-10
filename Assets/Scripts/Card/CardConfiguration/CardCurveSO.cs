
using UnityEngine;

[CreateAssetMenu(fileName = "CardCurveSO", menuName = "ScriptableObjects/CardCurveSO")]
public class CardCurveSO : ScriptableObject
{
    public AnimationCurve positionCurve = AnimationCurve.Linear(0, 0, 1, 0);
    public float positionCoefficient = 1;
    public AnimationCurve rotationCurve = AnimationCurve.Linear(0, 0, 1, 0);
    public float rotationCoefficient = 1;
}