using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JudgeUI : MonoBehaviour
{
    [SerializeField] private Text judgeText;

    public void ShowJudge(string message)
    {
        judgeText.DOKill();
        judgeText.transform.DOKill();

        judgeText.text = message;

        // 表示状態にする
        judgeText.gameObject.SetActive(true);

        // アルファを1に戻す
        Color color = judgeText.color;
        color.a = 1f;
        judgeText.color = color;

        // スケール初期化
        judgeText.transform.localScale = Vector3.zero;

        // ポップ演出
        judgeText.transform
            .DOScale(1f, 0.3f)
            .SetEase(Ease.OutBack);

        // フェードアウト
        judgeText.DOFade(0f, 0.8f)
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                judgeText.gameObject.SetActive(false);
            });
    }
}