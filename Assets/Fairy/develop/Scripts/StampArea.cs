using UnityEngine;
using UnityEngine.EventSystems;

public class StampArea : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject stampPrefab;
    [SerializeField] private Transform stampParent;
    [SerializeField] private JudgeUI judgeUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        // ハンコ生成
        GameObject stamp = Instantiate(stampPrefab, stampParent);
        stamp.transform.position = transform.position;

        // 判定表示
        judgeUI.ShowJudge("押した！");

        // スコア加算
        GameManager.Instance.AddScore(10);
    }
}