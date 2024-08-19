using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Tween currentTween;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayUIHoverEnter();
        TweenTo(1.06f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AudioManager.Instance.PlayUIHoverExit();
        TweenTo(1f);
    }

    private void TweenTo(float relativeScale)
    {
        currentTween?.Kill();
        currentTween = transform.DOScale(relativeScale, .6f);
    }
}
