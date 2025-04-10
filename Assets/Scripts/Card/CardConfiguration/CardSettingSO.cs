
using UnityEngine;

[CreateAssetMenu(fileName = "CardSetting", menuName = "Card/CardSetting", order = 0)]
public class CardSettingSO : ScriptableObject
{
    public CardSetting Setting;

    #if UNITY_EDITOR
    
    public bool isDirty = false;
    
    private void OnValidate()
    {
        isDirty = true;
    }
    #endif
}
