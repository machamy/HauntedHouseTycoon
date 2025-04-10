
using UnityEngine;

/// <summary>
/// 카드의 바깥쪽 테투리
/// </summary>
public class CardOuterFrameSpritesSO : ScriptableObject
{ 
    public Sprite FullOuterFrameSprite;
    public Sprite[] HalfOuterFrameSprites;
    public Sprite[] SimpleOuterFrameSprites;
    
    public Sprite GetOuterFrameSprite(CardDisplay.CardDisplayType cardDisplayType, int lv = 0)
    {
        Sprite[] arr;
        switch (cardDisplayType)
        {
            case CardDisplay.CardDisplayType.Full:
                return FullOuterFrameSprite;
            case CardDisplay.CardDisplayType.Half:
                arr = HalfOuterFrameSprites;
                break;
            case CardDisplay.CardDisplayType.Simple:
                arr = SimpleOuterFrameSprites;
                break;
            default:
                return null;
        }
        return arr[Mathf.Clamp(lv, 0, arr.Length - 1)];
    }

    public Sprite GetFullOuterFrameSprite() => FullOuterFrameSprite;
    public Sprite GetHalfOuterFrameSprite(int lv)
    {
        return HalfOuterFrameSprites[Mathf.Clamp(lv, 0, HalfOuterFrameSprites.Length - 1)];
    }
    public Sprite GetSimpleOuterFrameSprite(int lv)
    {
        return SimpleOuterFrameSprites[Mathf.Clamp(lv, 0, SimpleOuterFrameSprites.Length - 1)];
    }
}
