using DG.Tweening;
using TMPro;
using UnityEngine;
/// <summary>
/// スコア加算時のポップアップアニメーション
/// </summary>
public class ScoreTextPopup : MonoBehaviour
{
    [Header("アニメーション詳細設定")]
    [SerializeField, Tooltip("自分のRect")] private RectTransform _rect;
    public RectTransform Rect => _rect;
    [SerializeField, Tooltip("アニメーション時間")] private float _duration = 1.5f;
    [SerializeField, Tooltip("上昇量")] private float _upPos = 30f;

    private TextMeshProUGUI _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        if(_rect == null)
        {
            _rect = GetComponent<RectTransform>();
        }
        //UpdateText(1000);
        PlayScoreAnimation();
    }

    public void UpdateText(int score)
    {
        _text.text = "+"+ score;
    }

    private void PlayScoreAnimation()
    {
        Color textColor = _text.color;
        textColor.a = 1f;
        _text.color = textColor;
        Sequence seq = DOTween.Sequence();
        seq.Append(_text.rectTransform.DOAnchorPosY
            (_text.rectTransform.anchoredPosition.y +_upPos,_duration * 0.4f)
                .SetEase(Ease.OutSine));
        seq.AppendInterval(_duration / 2f);
        seq.Append(_text.DOFade(0f, _duration/2f * 0.3f)
                .SetEase(Ease.OutQuad));
        seq.AppendInterval(_duration * 0.6f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
