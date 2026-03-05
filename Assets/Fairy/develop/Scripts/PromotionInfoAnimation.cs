using DG.Tweening;
using UnityEngine;

public class PromotionInfoAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _image;
    [SerializeField] private RectTransform _text;
    [SerializeField] private float _imageMaxScale;
    [SerializeField] private float _imageMinScale;
    [SerializeField] private float _textMaxScale;
    [SerializeField] private float _textMinScale;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;
    public void PlayAnimation()
    {
        _image.localScale = Vector3.one * _imageMinScale;
        _image.DOScale(Vector3.one *  _imageMaxScale, _duration).SetEase(_ease);
        _text.localScale = Vector2.one * _textMinScale;
        _text.DOScale(Vector3.one * _textMaxScale, _duration).SetEase(_ease);
    }

}
