using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tween currentTween;

    [SerializeField] bool hasClickSound = true;

    private void Start()
    {
        if (hasClickSound)
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
    }

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

    private void OnClick()
    {
        AudioManager.Instance.PlayUIClick();
    }

    private void TweenTo(float relativeScale)
    {
        currentTween?.Kill();
        currentTween = transform.DOScale(relativeScale, .6f);
    }
}
