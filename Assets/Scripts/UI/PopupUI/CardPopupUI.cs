
using UnityEngine;
using UnityEngine.UI;

public class CardPopupUI : BasePopupUI
{
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;

    public void Initialize(CardData cardData)
    {
        leftImage.sprite = cardData.halfCardSprite;
        rightImage.sprite = cardData.fullCardSprite;
    }
}
