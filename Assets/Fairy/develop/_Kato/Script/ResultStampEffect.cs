using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResultStampEffect : MonoBehaviour
{
    [SerializeField, Header("アニメーション待機時間")] float _animationInterval = 2f;
    [SerializeField, Header("アニメーション時間")] float _duration = 0.2f;
    [SerializeField, Header("最初の大きさ")] float _firstScaleMag = 1.2f;

    [SerializeField] Ease _scaleEase = Ease.InElastic;
    [SerializeField] Ease _fadeEase;

    Image _stamp;
    Vector3 _firstScale;

    void Awake()
    {
        _stamp = GetComponent<Image>();
        _firstScale = _stamp.rectTransform.localScale;
        _stamp.color = new Color(1, 1, 1, 0);
        _stamp.rectTransform.localScale = _firstScale * _firstScaleMag;
        StampEffect();
    }

    void StampEffect()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(_animationInterval);
        seq.Append(_stamp.rectTransform.DOScale(_firstScale, _duration).SetEase(_scaleEase));
        seq.Join(_stamp.DOFade(1f, _duration).SetEase(_fadeEase));
    }
}
