using UnityEngine;
using UnityEngine.UI;

public class InkManager : MonoBehaviour
{
    [SerializeField] private float _maxAlpha;
    [SerializeField] private float _minAlpha;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _removeMount;

    [SerializeField] private Image _pointerImage;
    [SerializeField] private HoverDetector _inkAreaHover;
    private float _inkValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChargeInk();
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0)&& _inkAreaHover.IsHover)
        {
            ChargeInk();
        }
    }
    public void ChargeInk()
    {
        _inkValue = _maxValue;
        UpdatePointerAlpha();
    }
    public void RemoveValue()
    {
        _inkValue -= _removeMount;
        UpdatePointerAlpha();
    }
    public float GetAlpha()
    {
        return Mathf.Lerp(_minAlpha, _maxAlpha, _inkValue / _maxValue);
    }
    private void UpdatePointerAlpha()
    {
        Color color = _pointerImage.color;
        color.a =GetAlpha();
        _pointerImage.color = color;
    }
}
