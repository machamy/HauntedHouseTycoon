﻿
using Define;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 카드의 종류별 안쪽 프레임
/// </summary>
[CreateAssetMenu(fileName = "CardInnerFrameSprites", menuName = "Card/CardInnerFrameSprites", order = 0)]
public class CardInnerFrameSpritesSO : ScriptableObject
{ 
    public CardType cardType;
    public Sprite simpleInnerFrameSprite;
    public Sprite halfInnerFrameSprite;
}
