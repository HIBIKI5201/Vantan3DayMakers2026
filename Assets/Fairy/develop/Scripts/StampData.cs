using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StampData : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    public RectTransform Rect => _rectTransform;
    [SerializeField] private Image _stampImage;
    [SerializeField] private TextMeshProUGUI _name;

    public void Start()
    {
        SetName(NameManager.UserName[0].ToString());
    }
    public void ChangeAlpha(float alpha)
    {
        Color imageColor = _stampImage.color;
        Color textColor = _name.color;
        imageColor.a = alpha;
        textColor.a = alpha;
        _stampImage.color = imageColor;
        _name.color = textColor;
    }
    private void SetName(string text)
    {
        _name.text = text;
    }
}
