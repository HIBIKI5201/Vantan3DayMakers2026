using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class TitleUIManager : TitleUIObjects
{
    //Audio
    public void UpdateAudioUI(GameObject imageObj, bool isOn)
    {
        imageObj.GetComponent<Image>().color = isOn? activeColor : inactiveColor;
    }
    //Credit
    public void OnPointerEnterCredit()
    {
        creditImage.rectTransform.DOKill();
        creditImage.rectTransform.DOAnchorPos(originCreditPos + moveCreditPosDuration, moveCreditTimeDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExitCredit()
    {
        creditImage.rectTransform.DOKill();
        creditImage.rectTransform.DOAnchorPos(originCreditPos, 0.3f).SetEase(Ease.OutQuad);
    }
}

public class TitleUIObjects : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] protected TextMeshProUGUI countdownText;
    [SerializeField] protected Image creditImage;
    [Header("パラメータ関連")]
    [SerializeField] protected Vector2 moveCreditPosDuration;
    [SerializeField] protected float moveCreditTimeDuration;
    protected Vector2 originCreditPos;
    
    protected Color activeColor = Color.white;
    protected Color inactiveColor = Color.clear; //509, -758 : 528, -697

    private void Awake()
    {
        originCreditPos = creditImage.rectTransform.anchoredPosition;
    }
}