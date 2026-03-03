using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] Image _fadeImage;
    [SerializeField] float _fadeTime;

    /// <summary>
    /// フェードイン
    /// </summary>
    public void ScreenFadeIn()
    {
        _fadeImage.DOFade(0, _fadeTime);
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void ScreenFadeOut()
    {
        _fadeImage.DOFade(1, _fadeTime);
    }
}
