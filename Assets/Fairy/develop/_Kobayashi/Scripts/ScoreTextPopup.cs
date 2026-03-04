using DG.Tweening;
using TMPro;
using UnityEngine;
/// <summary>
/// スコア加算時のポップアップアニメーション
/// </summary>
public class ScoreTextPopup : MonoBehaviour
{
    [Header("アニメーション詳細設定")]
    [SerializeField, Tooltip("アニメーション時間")] private float _duration = 1.5f;
    [SerializeField, Tooltip("上昇量")] private float _upPos = 30f;

    private TextMeshProUGUI _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        //UpdateText(1000);
        PlayScoreAnimation();
    }

    public void UpdateText(int score)
    {
        _text.text = "+"+ score;
    }

    private void PlayScoreAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_text.rectTransform.DOAnchorPosY
            (_text.rectTransform.anchoredPosition.y +_upPos,_duration * 0.4f)
                .SetEase(Ease.OutSine));
        seq.Join(_text.DOFade(1f, _duration * 0.3f)
                .SetEase(Ease.OutQuad));
        seq.AppendInterval(_duration * 0.6f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
