using UnityEngine;
using DG.Tweening;

/// <summary>
/// ゲームの状態変化を監視して、
/// ポーズUIの表示／非表示を制御するクラス。
/// GameManagerとは直接依存しない（イベント購読のみ）。
/// </summary>
public class PauseUIController : MonoBehaviour
{
    // 背景フェード用（ImageにCanvasGroupを付ける）
    [SerializeField] private CanvasGroup canvasGroup;

    // 「PAUSED」テキスト本体（スケールアニメ用）
    [SerializeField] private RectTransform pauseText;

    private void Start()
    {
        // 初期状態を強制的に非表示にする
        canvasGroup.alpha = 0f;
        pauseText.localScale = Vector3.zero;
    }
    /// <summary>
    /// 有効化されたときにイベントを購読する
    /// </summary>
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleStateChanged;
    }

    /// <summary>
    /// 無効化されたときに必ず購読解除する
    /// （しないとメモリリークやエラーの原因になる）
    /// </summary>
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleStateChanged;
    }

    /// <summary>
    /// ゲーム状態が変わったときに呼ばれる
    /// </summary>
    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Paused)
        {
            Show();
        }
        else if (state == GameState.Playing)
        {
            Hide();
        }
    }

    /// <summary>
    /// ポーズUI表示
    /// Time.timeScale = 0 でも動くよう SetUpdate(true) を使用
    /// </summary>
    private void Show()
    {
        // 背景フェードイン
        canvasGroup.DOFade(1f, 0.3f)
            .SetUpdate(true);

        // テキストを0スケールから拡大
        pauseText.localScale = Vector3.zero;
        pauseText.DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    /// <summary>
    /// ポーズUI非表示
    /// </summary>
    private void Hide()
    {
        // テキスト縮小
        pauseText.DOScale(0f, 0.2f)
            .SetEase(Ease.InBack)
            .SetUpdate(true);

        // 背景フェードアウト
        canvasGroup.DOFade(0f, 0.3f)
            .SetUpdate(true);
    }
}