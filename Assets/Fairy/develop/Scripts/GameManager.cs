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

public enum Post
{
    None,
    Staff,
    SectionChief,
    Manager,
    Director,
    President
}
/// <summary>
/// �Q�[���S�̂̏�Ԃ��Ǘ�����N���X�B
/// UI�≉�o�̐���͍s��Ȃ��B
/// ��ԕω��̓C�x���g�Œʒm����B
/// </summary>
public class GameManager : MonoBehaviour
{
    // �V���O���g��
    public static GameManager Instance { get; private set; }

    // �Q�[����ԕύX�ʒm�C�x���g
    public static event Action<GameState> OnGameStateChanged;

    // ���݂̃Q�[�����
    public GameState CurrentState { get; private set; }

    // �X�R�A
    public int Score { get; private set; }
    // 

    // ���i���x��
    public int RankLevel { get; private set; }

    private CountdownManager countdownManager;
    private InGameUIManager _inGameUIManager;

    /// <summary>
    /// ������
    /// </summary>
    private async void Awake()
    {
        // �V���O���g���ۏ�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 1�t���[���҂��Ă��瑼�I�u�W�F�N�g����
        await UniTask.DelayFrame(1);

        countdownManager = FindFirstObjectByType<CountdownManager>();
        _inGameUIManager = FindFirstObjectByType<InGameUIManager>();

        CurrentState = GameState.Ready;

        // �J�E���g�_�E�����s
        if (countdownManager != null)
            await countdownManager.StartCountdownAsync();

        StartGame();
    }

    /// <summary>
    /// ���͊Ď��i�VInputSystem�g�p�j
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E�����ꂽ");

            if (CurrentState == GameState.Playing)
                PauseGame();
            else if (CurrentState == GameState.Paused)
                ResumeGame();
        }
    }

    /// <summary>
    /// ��ԕύX����
    /// ��Ԃ��X�V���A�C�x���g�ʒm����
    /// </summary>
    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("State changed to: " + newState);
        OnGameStateChanged?.Invoke(newState);
    }

    /// <summary>
    /// �X�R�A���Z
    /// </summary>
    public void AddScore(int amount)
    {
        Score += amount;
        _inGameUIManager.UpdateScoreUI(Score);
        CheckRankUp();
    }

    /// <summary>
    /// ���i����
    /// </summary>
    void CheckRankUp()
    {
        if (Score > 100 && RankLevel == 0)
        {
            RankLevel = 1;
            Debug.Log("���i�I");
        }
    }

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1f;
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// �|�[�Y
    /// </summary>
    public void PauseGame()
    {
        Debug.Log("PauseGame�Ă΂ꂽ");

        //Time.timeScale = 0f;
        ChangeState(GameState.Paused);
    }

    /// <summary>
    /// �ĊJ
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameOver()
    {
        //Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
    }
}