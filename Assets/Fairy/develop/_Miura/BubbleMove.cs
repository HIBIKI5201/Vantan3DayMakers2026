using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BubbleMove : MonoBehaviour
{
    [SerializeField] float moveRange = 20f;   // 揺れの範囲
    [SerializeField] float minDuration = 0.8f; // 最小時間
    [SerializeField] float maxDuration = 1.5f; // 最大時間

    RectTransform _rectTransform;
    Vector2 _originPos;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originPos = _rectTransform.anchoredPosition;
        SpeechMove();
    }

    void SpeechMove()
    {
        // ランダムな方向に移動
        Vector2 randomPos = _originPos + new Vector2(
            Random.Range(-moveRange, moveRange),
            Random.Range(-moveRange, moveRange)
        );

        float duration = Random.Range(minDuration, maxDuration);

        _rectTransform.DOAnchorPos(randomPos, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(SpeechMove); // 完了したら再度ランダムに移動
    }
}