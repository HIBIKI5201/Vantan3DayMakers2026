using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class TitleUIManager : TitleUIObjects
{
    Sequence _alertSeq;

    private void Start()
    {
        fadePrefab.GetComponent<Fade>().FadeIn();
    }

    //Audio
    public void UpdateAudioUI(GameObject imageObj, bool isOn)
    {
        imageObj.GetComponent<Image>().color = isOn ? activeColor : inactiveColor;
    }

    //GameStart
    public void OnPointerClickGameStart()
    {
        AudioManager.Instance.ChangeSE(SEClipType.Stamp);
        if (string.IsNullOrEmpty(DataManager.Instance.UserName)){
            NoEditNameAlert();
        }
        else{
            fadePrefab.GetComponent<Fade>().FadeOut();
            sceneLoader.LoadScene(1);}
    }

    //SpeechBubble
    void NoEditNameAlert()
    {
        if (_alertSeq.IsActive()) return; //TWeen再生中は返す
        _alertSeq = DOTween.Sequence();
        _alertSeq
            .Append(speechBubbleImage.rectTransform.DOScale(1, speechBubbleFadeinScaleDuration).SetEase(Ease.OutQuad))
            .Append(speechBubbleText.DOFade(1, speechBubbleFadeInTextDuration))
            .AppendCallback(() => { SpeechMove(speechBubbleImage.rectTransform, originSpeechbubbleImagePos, true); SpeechMove(speechBubbleText.rectTransform, originSpeechbubbleTextPos, false); })
            .AppendInterval(speechBubbleIntervalDuration)
            .AppendCallback(() => speechBubbleImage.rectTransform.DOKill())
            .Append(speechBubbleImage.rectTransform.DOScale(0, speechBubbleFadeOutScaleDuration).SetEase(Ease.OutQuad))
            .Join(speechBubbleImage.rectTransform.DOAnchorPos(originSpeechbubbleImagePos + speechBubblePosDuration, speechBubbleFadeOutScaleDuration).SetEase(Ease.OutQuad))
            .OnComplete(() =>
            {
                speechBubbleText.rectTransform.DOKill();
                speechBubbleText.alpha = 0;
                speechBubbleImage.rectTransform.anchoredPosition = originSpeechbubbleImagePos;
                speechBubbleText.rectTransform.anchoredPosition = originSpeechbubbleTextPos;
            });
    }

    void SpeechMove(RectTransform rect, Vector2 originPos, bool isImage)
    {
        float moveRange = isImage ? bubbleImageMoveRange : bubbleTextMoveRange;
        float minDuration = isImage ? bubbleImageMinDuration : bubbleTextMinDuration;
        float maxDuration = isImage ? bubbleImageMaxDuration : bubbleTextMaxDuration;

        Vector2 randomPos = originPos + new Vector2(
            Random.Range(-moveRange, moveRange),
            Random.Range(-moveRange, moveRange)
        );

        rect.DOAnchorPos(randomPos, Random.Range(minDuration, maxDuration))
            .SetEase(Ease.InOutSine)
            .OnComplete(() => SpeechMove(rect, originPos, isImage));
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

    void SetCreditActive(bool isActive)
    {
        creditImage.rectTransform.DOKill();
        creditClickArea_A.gameObject.SetActive(!isActive);
        creditClickArea_B.gameObject.SetActive(isActive);
        creditCanvas.sortingOrder = isActive ? 5 : 1;
        creditImage.rectTransform.anchoredPosition = isActive ? Vector2.zero : originCreditPos;
        creditImage.rectTransform.localRotation = Quaternion.Euler(0, 0, isActive ? 0 : -17.5f);
        AudioManager.Instance.ChangeSE(SEClipType.Paper);
    }

    public void OnPointerClickActiveCredit()   => SetCreditActive(true);
    public void OnPointerClickInActiveCredit() => SetCreditActive(false);
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
    protected Vector2 originSpeechbubbleImagePos;
    protected Vector2 originSpeechbubbleTextPos;
    [Header("SceneLoader")]
    [SerializeField] protected SceneLoader sceneLoader;
    [Header("FadePrefab")]
    [SerializeField] protected GameObject fadePrefab;
    protected Vector2 originCreditPos;
    protected Color activeColor = Color.white;
    protected Color inactiveColor = Color.clear;

    void Awake()
    {
        creditCanvas      = creditImage.transform.parent.GetComponent<Canvas>();
        creditClickArea_A = creditImage.transform.GetChild(0).GetComponent<Image>();
        creditClickArea_B = creditImage.transform.GetChild(1).GetComponent<Image>();
        creditClickArea_B.gameObject.SetActive(false);
        hand_CloseImage.alphaHitTestMinimumThreshold = 0.1f;
        speechBubbleImage.rectTransform.localScale   = Vector3.zero;
        speechBubbleText.color                       = Color.clear;
        originSpeechbubbleImagePos = speechBubbleImage.rectTransform.anchoredPosition;
        originSpeechbubbleTextPos  = speechBubbleText.rectTransform.anchoredPosition;
        originCreditPos            = creditImage.rectTransform.anchoredPosition;
    }
}