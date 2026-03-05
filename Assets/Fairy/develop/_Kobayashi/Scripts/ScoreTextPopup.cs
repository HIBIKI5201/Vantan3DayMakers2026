using Cysharp.Threading.Tasks;
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
    [SerializeField, Tooltip("自分のRect")] private TextMeshProUGUI _text;
    public RectTransform Rect => _rect;
    [SerializeField, Tooltip("アニメーション時間")] private float _duration = 1.5f;
    [SerializeField, Tooltip("上昇量")] private float _upPos = 30f;

    public void OnEnable()
    {
        if (_rect == null)
        {
            _rect = GetComponent<RectTransform>();
        }
        if (_text == null)
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        //UpdateText(1000);
        PlayScoreAnimation();
    }
    public void UpdateText(int score)
    {
        _text.text = "+"+ (score/10f).ToString("N1");
    }

    private async void PlayScoreAnimation()
    {
        await UniTask.DelayFrame(1);
        Color textColor = _text.color;
        textColor.a = 1f;
        _text.color = textColor;
        Sequence seq = DOTween.Sequence();
        seq.Append(_rect.DOAnchorPosY
            (_rect.anchoredPosition.y + _upPos, _duration * 0.4f)
                .SetEase(Ease.OutSine));
        seq.AppendInterval(_duration / 2f * 0.3f);
        seq.Append(_text.DOFade(0f, _duration/2f * 0.3f)
                .SetEase(Ease.OutQuad));
        seq.AppendInterval(_duration * 0.6f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
        seq.Play();
    }
}
