using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Fade : MonoBehaviour
{
    private Image _fade;
    [SerializeField] private float _duration = 0.5f;

    private void Awake()
    {
        _fade = GetComponentInChildren<Image>();
        _fade.color = Color.clear;
        _fade.raycastTarget = false;
    }

    public void FadeIn()
    {
        _fade.color = Color.black;
        _fade.raycastTarget = false;
        _fade.DOFade(0f, _duration);
    }

    public void FadeOut()
    {
        _fade.color = Color.clear;
        _fade.raycastTarget = true;
        _fade.DOFade(1f, _duration);
    }
}