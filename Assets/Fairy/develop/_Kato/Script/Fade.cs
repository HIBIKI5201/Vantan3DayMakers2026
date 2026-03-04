using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
public enum FadeType
{
    Normal,
    Crazy,
    Zoom,
    Party
}

public class Fade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.InOutQuad;

    private void Awake()
    {
        // もしインスペクターで入れてなければ自動取得
        if (fadeImage == null) fadeImage = GetComponentInChildren<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false;
    }

    public async UniTask FadeOutAsync(FadeType type)
    {
        fadeImage.raycastTarget = true;

        switch (type)
        {
            case FadeType.Crazy:
                await CrazyFadeOut();
                break;
            case FadeType.Zoom:
                await ZoomFadeOut();
                break;
            default: // Normal
                // Tweenを直接awaitせずに、AsyncWaitForCompletionを呼ぶ
                await fadeImage.DOFade(1f, duration).SetEase(ease).AsyncWaitForCompletion();
                break;
        }
    }

    public async UniTask FadeInAsync()
    {
        // フェードインはシンプルに黒から戻るだけでOKなことが多いです
        await fadeImage.DOFade(0f, duration).SetEase(ease).AsyncWaitForCompletion();
        // 回転などを戻しておく（Crazyの後対策）
        fadeImage.rectTransform.rotation = Quaternion.identity;
        fadeImage.rectTransform.localScale = Vector3.one;

        fadeImage.raycastTarget = false;
    }

    private async UniTask CrazyFadeOut()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(fadeImage.DOFade(1f, 0.5f));
        seq.Join(fadeImage.rectTransform.DORotate(new Vector3(0, 0, 720), 0.5f, RotateMode.FastBeyond360));

        await seq.AsyncWaitForCompletion();
    }

    private async UniTask ZoomFadeOut()
    {
        fadeImage.rectTransform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Join(fadeImage.DOFade(1f, 0.5f));
        seq.Join(fadeImage.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));

        await seq.AsyncWaitForCompletion();
    }
}