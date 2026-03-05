using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSliderColor : MonoBehaviour
{
    private Image targetImage;

    [SerializeField]
    private Gradient gradient = new Gradient
    {
        colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(Color.yellow, 0.5f),
            new GradientColorKey(Color.green, 1f)
        },
        alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(1f, 1f)
        }
    };

    private void Awake()
    {
        targetImage = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateColor(targetImage.fillAmount); // 初期値反映
    }

    /// <summary>
    /// fillAmount(0〜1)に応じて色を更新
    /// </summary>
    public void UpdateColor(float value)
    {
        float t = Mathf.Clamp01(Mathf.Abs(value));
        targetImage.color = gradient.Evaluate(t);
    }

    /// <summary>
    /// 外部から値(現在値, 最大値)で更新したい場合
    /// </summary>
    public void UpdateColor(float current, float max)
    {
        if (max <= 0)
        {
            targetImage.color = Color.red;
            return;
        }

        float t = Mathf.Clamp01(Mathf.Abs(current) / max);
        targetImage.color = gradient.Evaluate(t);
    }
}