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
    protected Color activeColor = Color.white;
    protected Color inactiveColor = Color.clear;
}