
using DG.Tweening;
using UnityEngine;

public class PlacedCard : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Room room;
    
    public void UpdateDisplay(CardData cardData)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = cardData.cardSprite;
    }
    
    public void UpdateDisplay(CardData cardData, Vector3 uiSize)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        Camera mainCam = Camera.main;
        float worldScreenHeight = mainCam.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * mainCam.aspect;
        Vector3 worldSize = new Vector3(uiSize.x / Screen.width * worldScreenWidth, uiSize.y / Screen.height * worldScreenHeight, 1);
        spriteRenderer.sprite = cardData.cardSprite;
        transform.localScale = worldSize;
        transform.DOScale(Vector3.one, 0.25f);
    }
}
