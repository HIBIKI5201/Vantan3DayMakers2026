using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public enum ScoreLevel
{
    Miss,
    Perfect
}
public class StampEvaluation : MonoBehaviour
{
    [SerializeField] private GameObject _evaluationPrefab;

    [Header("アニメーション")]
    [SerializeField] private float _startY;
    [SerializeField] private float _endY;
    [SerializeField] private float _duration;
    public void ShowEvaluation(GameObject stamp,int score)
    {
        GameObject newObject = Instantiate(_evaluationPrefab, stamp.transform);

        RectTransform rect = newObject.GetComponent<RectTransform>();

        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        // 🔹 アニメーション開始位置を明示
        Vector2 startPos = rect.anchoredPosition;
        startPos.y = _startY;   // ← ここを指定
        rect.anchoredPosition = startPos;

        // 🔹 終了位置
        float endY = _endY;

        //TODO:荒ぶるので一旦保留

        // 移動
        //rect.DOAnchorPosY(endY, _duration);

        //// フェード
        //if (newObject.TryGetComponent(out TextMeshProUGUI text))
        //{
        //    text.DOFade(0, _duration);
        //}
    }
}
