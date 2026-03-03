using UnityEngine;

public class StampArea : MonoBehaviour
{
    [SerializeField] private GameObject _stampPrefab;
    [SerializeField] private RectTransform _stampParent;
    [SerializeField] private JudgeUI _judgeUI;

    public void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _stampParent.parent as RectTransform,
            Input.mousePosition,
            null,
            out localPoint
        );

        _stampParent.localPosition = localPoint;

        if (Input.GetMouseButtonDown(0))
        {

            // ハンコ生成
            GameObject stamp = Instantiate(_stampPrefab, _stampParent);
            stamp.transform.position = transform.position;

            // 判定表示
            _judgeUI.ShowJudge("押した！");

            // スコア加算
            GameManager.Instance.AddScore(10);
        }

    }
}