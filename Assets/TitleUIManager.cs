using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class TitleUIManager : TitleUIObjects
{
    //Audio
    public void UpdateAudioUI(GameObject imageObj, bool isOn)
    {
        imageObj.GetComponent<Image>().color = isOn? activeColor : inactiveColor;
    }
    //GameStart
    public void OnPointerClickGameStart()
    {
        if (string.IsNullOrEmpty(DataManager.Instance.UserName))
        {
            NoEditNameAlert();
        }
        else
        {
            sceneLoader.LoadScene();
        }
    }
    //SpeechBubble(NoEditNameAlert)
    void NoEditNameAlert()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(speechBubbleImage.rectTransform.DOScale(1, speechBubbleFadeinScaleDuration).SetEase(Ease.OutQuad));
        seq.Append(speechBubbleText.DOFade(1, speechBubbleFadeInTextDuration));
        seq.AppendCallback(() => SpeechMove(true));
        seq.AppendCallback(() => SpeechMove(false));
        seq.AppendInterval(speechBubbleIntervalDuration);
        seq.AppendCallback(() =>
        {
            speechBubbleImage.rectTransform.DOKill();
            speechBubbleText.rectTransform.DOKill();
        });
        seq.Append(speechBubbleImage.rectTransform.DOScale(0, speechBubbleFadeOutScaleDuration).SetEase(Ease.OutQuad));
        seq.Join(speechBubbleImage.rectTransform.DOAnchorPos(originSpeechbubblePos + speechBubblePosDuration, speechBubbleFadeOutScaleDuration).SetEase(Ease.OutQuad));
    }
    void SpeechMove(bool isImage)
    {
        float moveRange = isImage? bubbleImageMoveRange : bubbleTextMoveRange;
        // ランダムな方向に移動
        Vector2 randomPos = originSpeechbubblePos + new Vector2(
            UnityEngine.Random.Range(-moveRange, moveRange),
            UnityEngine.Random.Range(-moveRange, moveRange)
        );
        float minDuration =  isImage? bubbleImageMinDuration : bubbleTextMinDuration, 
              maxDuration = isImage? bubbleImageMaxDuration : bubbleTextMaxDuration;
        float duration = UnityEngine.Random.Range(minDuration, maxDuration);
        var rectTransform = isImage? speechBubbleImage.rectTransform : speechBubbleText.rectTransform;
        rectTransform.DOAnchorPos(randomPos, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => SpeechMove(isImage)); // 完了したら再度ランダムに移動
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
    /// <summary> いちいちこんなことしないで、ぱっとクレジットを出してしまうのもあり </summary>
    public void OnPointerClickActiveCredit()
    {
        creditImage.rectTransform.DOKill();
        creditClickArea_A.gameObject.SetActive(false);
        creditClickArea_B.gameObject.SetActive(true);
        creditCanvas.sortingOrder = 5;
        creditImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        creditImage.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        audioManager.ChangeSE(SEClipType.Paper);
    }
    public void OnPointerClickInActiveCredit()
    {
        creditImage.rectTransform.DOKill();
        creditClickArea_A.gameObject.SetActive(true);
        creditClickArea_B.gameObject.SetActive(false);
        creditCanvas.sortingOrder = 1;
        creditImage.rectTransform.anchoredPosition = originCreditPos;
        creditImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -17.5f);
        audioManager.ChangeSE(SEClipType.Paper);
    }
}

public class TitleUIObjects : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] protected TextMeshProUGUI countdownText;
    [Header("Credit")]
    [SerializeField] protected Image creditImage;
    [SerializeField] protected Image hand_CloseImage;
    protected Canvas creditCanvas;
    protected Image creditClickArea_A;
    protected Image creditClickArea_B;
    [Header("")]
    [Header("パラメータ関連")]
    [SerializeField] protected Vector2 moveCreditPosDuration;
    [SerializeField] protected float moveCreditTimeDuration;
    [Header("SpeechBubble")]
    [SerializeField] protected Image speechBubbleImage;
    [SerializeField] protected TextMeshProUGUI speechBubbleText;
    [SerializeField] protected float speechBubbleFadeinScaleDuration;
    [SerializeField] protected float speechBubbleFadeInTextDuration;
    [SerializeField] protected float speechBubbleIntervalDuration;
    [SerializeField] protected float speechBubbleFadeOutScaleDuration;
    [SerializeField] protected Vector2 speechBubblePosDuration;
    
    [SerializeField] protected float bubbleImageMoveRange = 20f;
    [SerializeField] protected float bubbleImageMinDuration = 0.8f;
    [SerializeField] protected float bubbleImageMaxDuration = 1.5f;
    [SerializeField] protected float bubbleTextMoveRange = 2f;
    [SerializeField] protected float bubbleTextMinDuration = 0.5f;
    [SerializeField] protected float bubbleTextMaxDuration = 1f;
    protected Vector2 originSpeechbubblePos;
    [Header("SceneLoader")]
    [SerializeField] protected SceneLoader sceneLoader;
    [Header("AudioManager.cs")] 
    [SerializeField] protected AudioManager audioManager;
    protected Vector2 originCreditPos;
    protected Color activeColor = Color.white;
    protected Color inactiveColor = Color.clear; //509, -758 : 528, -697

    private void Awake()
    {
        creditCanvas = creditImage.transform.parent.GetComponent<Canvas>();
        creditClickArea_A = creditImage.transform.GetChild(0).GetComponent<Image>();
        creditClickArea_B = creditImage.transform.GetChild(1).GetComponent<Image>();
        creditClickArea_B.gameObject.SetActive(false);
        hand_CloseImage.alphaHitTestMinimumThreshold = 0.1f; //alphaが0.1以上の部分しかRaycastが反応しない
        speechBubbleImage.rectTransform.localScale = Vector3.zero;
        speechBubbleText.color = Color.clear;
        originSpeechbubblePos = speechBubbleImage.rectTransform.anchoredPosition;
        originCreditPos = creditImage.rectTransform.anchoredPosition;
    }
}