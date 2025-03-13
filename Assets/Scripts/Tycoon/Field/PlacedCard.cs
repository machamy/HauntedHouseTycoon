
using DG.Tweening;
using UnityEngine;

public class PlacedCard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Room room;
    
    public void UpdateDisplay(CardData cardData)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = cardData.cardSprite;
    }
    
    public void UpdateDisplay(CardData cardData, Vector3 startSize)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        Vector3 worldSize = startSize;
        if (cardData.cardPlacedSprite)
        {
            spriteRenderer.sprite = cardData.cardPlacedSprite;
        }
        else
        {
            spriteRenderer.sprite = cardData.cardSprite;
        }
        transform.localScale = worldSize;
        transform.DOScale(Vector3.one, 0.25f);
    }
}
