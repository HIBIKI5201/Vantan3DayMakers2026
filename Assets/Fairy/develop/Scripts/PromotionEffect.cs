using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PromotionEffect : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private float _duration;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    [SerializeField] private Ease _scaleEase = Ease.OutQuint;
    [SerializeField] private Ease _alphaEase = Ease.OutQuint;

    public void Start()
    {
        _rect.sizeDelta = Vector2.one * _minScale;
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;
        _rect.DOSizeDelta(Vector2.one * _maxScale, _duration).SetEase(_scaleEase);
        _image.DOFade(1, _duration).SetEase(_alphaEase);
    }
}
