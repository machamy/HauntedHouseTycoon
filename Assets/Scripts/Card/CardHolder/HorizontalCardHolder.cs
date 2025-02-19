using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


public class HorizontalCardHolder : BaseCardHolder
{
    [Header("Card display Setting")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float fadeAlpha = 0.25f;
    [Header("Holder Setting")]
    [SerializeField] private int maxVisibleCardAmount = 10;
    [FormerlySerializedAs("currentVisibleCardAmount")] [SerializeField] private int currentMaxVisibleCardAmount = 0;
    
    
    private int _visibleStartIdx = 0;
    
    public override int MaxVisibleCardAmount => currentMaxVisibleCardAmount;
    public override int VisibleStartIdx => _visibleStartIdx;
    public override int VisibleEndIdx => Math.Min(_visibleStartIdx + currentMaxVisibleCardAmount - 1, cardObjects.Count-1);

    public override int CurrentVisibleCardAmount => (VisibleEndIdx - _visibleStartIdx) + 1;

    private void OnRectTransformDimensionsChange()
    {
        var rect = GetComponent<RectTransform>();
        int maxAvailable = Mathf.FloorToInt(rect.rect.width / (cardWidth + cardGap));
        currentMaxVisibleCardAmount = Math.Clamp(maxAvailable,2, maxVisibleCardAmount);
    }

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


    private void Update()
    {
        // TODO : 테스트용
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightShift();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftShift();
        }
    }

    public void UpdateVisible()
    {

        for (int i = _visibleStartIdx + 1; i < VisibleEndIdx; i++)
        {
            // for문으로 다 돌아도 최적화에 큰 문제는 없을듯
            if(!cardObjects[i].slotGO.activeSelf)
            {
                cardObjects[i].slotGO.SetActive(true);
            }
            cardObjects[i].CardDisplay.DoFade(1, fadeDuration);
        }
        if(!cardObjects[_visibleStartIdx].slotGO.activeSelf)
        {
            cardObjects[_visibleStartIdx].slotGO.SetActive(true);
        }
        if(_visibleStartIdx - 1 >= 0)
        {
            // 왼쪽에 카드가 있는경우
            cardObjects[_visibleStartIdx - 1].slotGO.SetActive(false);
            cardObjects[_visibleStartIdx].CardDisplay.DoFade(fadeAlpha, fadeDuration);
        }
        else
        {
            // 왼쪽에 카드가 없는경우
            cardObjects[_visibleStartIdx].CardDisplay.DoFade(1, fadeDuration);
        }
        
        if(!cardObjects[VisibleEndIdx].slotGO.activeSelf)
        {
            cardObjects[VisibleEndIdx].slotGO.SetActive(true);
        }
        if(VisibleEndIdx + 1 < cardObjects.Count)
        {
            // 오른쪽에 카드가 있는경우
            cardObjects[VisibleEndIdx + 1].slotGO.SetActive(false);
            cardObjects[VisibleEndIdx].CardDisplay.DoFade(fadeAlpha, fadeDuration);
        }
        else
        {
            // 오른쪽에 카드가 없는경우
            cardObjects[VisibleEndIdx].CardDisplay.DoFade(1, fadeDuration);
        }
    }
    
    public void RightShift()
    {
        /*
         * 1. 가장 왼쪽의 card 비활성화
         * 2. 그 오른쪽 카드 반투면
         * 3. 가장 오른쪽 카드 + 1 활성화
         * 4. 그 카드 반투명
         */
        if(VisibleEndIdx + 1 < cardObjects.Count)
        {
            // cardObjects[visibleStartIdx].slotGO.SetActive(false);
            _visibleStartIdx++;
            UpdateVisible();
            //
            // if(visibleStartIdx > 0)
            // {
            //     // 왼쪽에 카드가 더 존재하면 반투명하게
            //     cardObjects[visibleStartIdx].CardDisplay.DoFade(0.25f, 0.25f);
            // }
            // else
            // {
            //     // (진)가장 왼쪽 카드임
            //     cardObjects[visibleStartIdx].CardDisplay.DoFade(1, 0.25f);
            // }
            //
            //
            // cardObjects[visibleEndIdx].slotGO.SetActive(true);
            // if(visibleEndIdx + 1 < cardObjects.Count)
            // {
            //     // 오른쪽에 카드가 더 존재하면 반투명하게
            //     cardObjects[visibleEndIdx].CardDisplay.DoFade(0.25f, 0.25f);
            // }
            // else
            // {
            //     // (진)가장 오른쪽 카드임
            //     cardObjects[visibleEndIdx].CardDisplay.DoFade(1, 0.25f);
            // }
            //
            // if (visibleEndIdx - 1 > visibleStartIdx)
            // {
            //     // 낀 카드는 불투명
            //     cardObjects[visibleEndIdx - 1].CardDisplay.DoFade(1, 0.25f);
            // }
        }
        UpdateAllCardIndex();
    }

    public void LeftShift()
    {
        /*
         * 1. 가장 오른쪽 카드 + 1 비활성화
         * 2. 그 왼쪽 카드 반투명
         * 3. 가장 왼쪽 카드 - 1 활성화
         * 4. 그 카드 반투명
         */
        if (_visibleStartIdx > 0)
        {
            // cardObjects[visibleEndIdx].slotGO.SetActive(false);
            _visibleStartIdx--;
            UpdateVisible();
            // if (visibleEndIdx+1 < cardObjects.Count)
            // {
            //     // 오른쪽에 카드가 더 존재하면 반투명하게
            //     cardObjects[visibleEndIdx].CardDisplay.DoFade(0.25f, 0.25f);
            // }
            // else
            // {
            //     // (진)가장 오른쪽 카드임
            //     cardObjects[visibleEndIdx].CardDisplay.DoFade(1, 0.25f);
            // }
            //
            // cardObjects[visibleStartIdx].slotGO.SetActive(true);
            // if (visibleStartIdx > 0)
            // {
            //     // 왼쪽에 카드가 더 존재하면 반투명하게
            //     cardObjects[visibleStartIdx+1].CardDisplay.DoFade(0.25f, 0.25f);
            // }
            // else
            // {
            //     // (진)가장 왼쪽 카드임
            //     cardObjects[visibleStartIdx+1].CardDisplay.DoFade(1, 0.25f);
            // }
            //
            // if (visibleStartIdx + 1 < visibleEndIdx)
            // {
            //     // 낀 카드는 불투명
            //     cardObjects[visibleStartIdx + 1].CardDisplay.DoFade(1, 0.25f);
            // }
        }
    }

    public override CardObject AddCardWithSlot(CardData cardData)
    {
        var cardObject = base.AddCardWithSlot(cardData);
        if (cardObjects.Count-1 > VisibleEndIdx)
        {
            cardObject.slotGO.SetActive(false);
        }
        return cardObject;
    }
    
    public override void RemoveCardFromHolder(CardObject cardObject)
    {
        int beforeEndIdx = VisibleEndIdx;
        base.RemoveCardFromHolder(cardObject);
        if(cardObjects.Count <= currentMaxVisibleCardAmount)
        {
            // 카드 개수가 보일수 있는 것보다 적음
            return;
        }
        if(beforeEndIdx + 1 < cardObjects.Count)
        {
            // 오른쪽에 카드가 있는경우, 오른쪽 카드를 활성화
            UpdateVisible();
            return;
        }
        if(_visibleStartIdx > 0)
        {
            // 왼쪽에 카드가 남아있으면, 왼쪽 카드를 활성화
            LeftShift();
            return;
        }
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
