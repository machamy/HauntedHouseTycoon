using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;


public class HorizontalCardHolder : BaseCardHolder
{

    protected override void OnFocus(CardSelection cardSelect)
    {
        base.OnFocus(cardSelect);
        CardDisplay cardDisplay = cardSelect.CardObject.CardDisplay;
        if(!cardSelect.CardSetting.hoverVisible)
            return;
        float focusedY = cardDisplay.GetClampedVisiblePos(cardSelect.transform.position.y);
        cardSelect.transform.position = new Vector3(cardSelect.transform.position.x, focusedY, cardSelect.transform.position.z);
        cardSelect.transform.localScale = Vector3.one * cardSelect.CardSetting.hoverScale;
        cardDisplay.transform.SetAsLastSibling();
    }

    protected override void OnUnfocus(CardSelection cardSelect)
    {
        cardSelect.transform.localPosition = Vector3.zero;
        cardSelect.transform.localScale = cardSelect.CardSetting.defaultScale * Vector3.one;
        UpdateAllCardIndex();
        base.OnUnfocus(cardSelect);
    }


    // protected override void OnCardDiscard(CardObject cardObject)
    // {
    //     if (!cardObject.CardSelect.IsUsed)
    //     {
    //         var gameObjectTransform = cardObject.gameObject.transform;
    //         cardObject.transform.position = new Vector3(transform.position.x, - gameObjectTransform.position.y * 2, transform.position.z);
    //         RemoveCardFromHolder(cardObject);
    //         
    //         StartCoroutine(WaitDiscardCard(cardObject));
    //         IEnumerator WaitDiscardCard(CardObject cardObject)
    //         {
    //             yield return new WaitForSeconds(0.25f);
    //             CardDisplay cardDisplay = cardObject.CardDisplay;
    //             yield return new WaitWhile(() => DOTween.IsTweening(cardObject.transform) || cardDisplay.IsAnimating);
    //             Destroy(cardDisplay.gameObject);
    //             Destroy(cardObject.gameObject);
    //         }
    //     }
    //     else{
    //         base.OnCardDiscard(cardObject);
    //     }
    // }
}
