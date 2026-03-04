using TMPro;
using UnityEngine;
using DG.Tweening;

public class TitleUIManager : TitleUIObjects
{
    
    public void OnPointerEnterCredit()
    {
        creditButton.GetComponent<RectTransform>().DOAnchorPos(endCreditPos, moveCreditDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExitCredit()
    {
        creditButton.GetComponent<RectTransform>().DOAnchorPos(originCreditPos, 0.3f).SetEase(Ease.OutQuad);
    }
}

public class TitleUIObjects : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] protected TextMeshProUGUI countdownText;
    [SerializeField] protected GameObject creditButton;
    [Header("パラメータ関連")]
    protected Vector2 originCreditPos;
    [SerializeField] protected Vector2 endCreditPos;
    [SerializeField] protected float moveCreditDuration;

    void Awake()
    {
        originCreditPos = creditButton.GetComponent<RectTransform>().anchoredPosition;
    }
}