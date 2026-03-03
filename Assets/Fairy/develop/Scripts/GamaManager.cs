using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    Ready,
    Playing,
    Paused,
    GameOver
}

/// <summary>
/// ゲーム全体の状態を管理するクラス。
/// UIや演出の制御は行わない。
/// 状態変化はイベントで通知する。
/// </summary>
public class GameManager : MonoBehaviour
{
    // シングルトン
    public static GameManager Instance { get; private set; }

    // ゲーム状態変更通知イベント
    public static event Action<GameState> OnGameStateChanged;

    // 現在のゲーム状態
    public GameState CurrentState { get; private set; }

    // スコア
    public int Score { get; private set; }

    // 昇進レベル
    public int RankLevel { get; private set; }

    private CountdownManager countdownManager;

    /// <summary>
    /// 初期化
    /// </summary>
    private async void Awake()
    {
        // シングルトン保証
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 1フレーム待ってから他オブジェクト検索
        await UniTask.DelayFrame(1);

        countdownManager = FindFirstObjectByType<CountdownManager>();

        CurrentState = GameState.Ready;

        // カウントダウン実行
        if (countdownManager != null)
            await countdownManager.StartCountdownAsync();

        StartGame();
    }

    /// <summary>
    /// 入力監視（新InputSystem使用）
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E押された");

            if (CurrentState == GameState.Playing)
                PauseGame();
            else if (CurrentState == GameState.Paused)
                ResumeGame();
        }
    }

    /// <summary>
    /// 状態変更処理
    /// 状態を更新し、イベント通知する
    /// </summary>
    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("State changed to: " + newState);
        OnGameStateChanged?.Invoke(newState);
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    public void AddScore(int amount)
    {
        Score += amount;
        CheckRankUp();
    }

    /// <summary>
    /// 昇進判定
    /// </summary>
    void CheckRankUp()
    {
        if (Score > 100 && RankLevel == 0)
        {
            RankLevel = 1;
            Debug.Log("昇進！");
        }
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1f;
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    public void PauseGame()
    {
        Debug.Log("PauseGame呼ばれた");

        Time.timeScale = 0f;
        ChangeState(GameState.Paused);
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
    }
}