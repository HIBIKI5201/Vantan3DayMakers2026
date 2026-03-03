using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

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
    [Header("パラメーター")]
    public static GameManager Instance { get; private set; }

    // �Q�[����ԕύX�ʒm�C�x���g
    public static event Action<GameState> OnGameStateChanged;

    // ���݂̃Q�[�����
    public GameState CurrentState { get; private set; }

    public int Score { get; private set; }

    public Post RankLevel { get; private set; }
    public float ClearTime { get; private set; }
    public float GameTimer { get; private set; }

    [Header("アタッチ")]
    [SerializeField] private PromotionValue[] _promotionValues;
    [SerializeField] private ShowEvaluation _showEvaluation;
    [SerializeField] private StampPointor _stampPointor;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Vector2 _offScreen;
    [SerializeField] private GameObject _stagePrefab;
    [SerializeField] private Transform _stageParent;
    [SerializeField] private RectTransform _stampArea;
    private CountdownManager countdownManager;
    //private InGameUIManager _inGameUIManager;

    public RectTransform _stageCreate { get; private set; }
    private bool IsAddTime;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await UniTask.DelayFrame(1);

        countdownManager = FindFirstObjectByType<CountdownManager>();

        CurrentState = GameState.Ready;

        NextStage();
        if (countdownManager != null)
            await countdownManager.StartCountdownAsync();

        _showEvaluation.HiddenWindow();

        _timeText.text = ClearTime.ToString("N2") + "秒";
        IsAddTime = false;
        StartGame();
    }
    private void Start()
    {
        Array.Sort(_promotionValues, (a, b) =>
        {
            if (a == null) return -1;
            if (b == null) return 1;
            return a.PromotionScore.CompareTo(b.PromotionScore);
        });
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))  ポーズ一旦削除
        //{
        //    if (CurrentState == GameState.Playing)
        //        PauseGame();
        //    else if (CurrentState == GameState.Paused)
        //        ResumeGame();
        //}
        if (IsAddTime)
        {
            ClearTime -= Time.deltaTime;
            GameTimer += Time.deltaTime;
            _timeText.text = ClearTime.ToString("N2") + "秒";
        }
    }


    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("State changed to: " + newState);
        OnGameStateChanged?.Invoke(newState);
    }
    public void OnStamp()
    {
        IsAddTime = false;
        Vector2 sPos = _stampPointor.ClonedStamp.anchoredPosition;
        Vector2 aPos = _stampArea.anchoredPosition;
        float rRot = _stampPointor.ClonedStamp.eulerAngles.z;
        float aRot = _stampArea.eulerAngles.z;

        int scoreAmount = _scoreManager.CalculationScore(sPos, rRot,sPos,aRot);
        AddScore(scoreAmount);
        _showEvaluation.ShowWindow(RankLevel, ClearTime, scoreAmount);
    }
    /// <summary>
    /// Score追加
    /// </summary>
    public void AddScore(int amount)
    {
        Debug.Log("Amount");
        Score += amount;
        //_inGameUIManager.UpdateScoreUI(Score);

        CheckRankUp();
    }

    /// <summary>
    /// 昇進チェック
    /// </summary>
    void CheckRankUp()
    {
        if (_promotionValues.Length == 0) return;
        PromotionValue previous = _promotionValues[0];
        foreach (var data in _promotionValues)
        {
            if (Score < data.PromotionScore)
            {
                previous = data;
            }
            else
                break;
        }
        Debug.Log(previous.PostName);
        RankLevel = previous.PostType;
    }
    public void NextStage()
    {
        if (_stageCreate != null)
        {
            GameObject deleteStage = _stageCreate.gameObject;
            _stageCreate.DOAnchorPosY(_offScreen.y, 1f)
                .OnComplete(() =>
                {
                    Destroy(deleteStage);
                });
        }

        _stampPointor.RemoveStampObject();
        GameObject newStage = Instantiate(_stagePrefab, _stageParent);
        if (newStage.TryGetComponent(out StageCreate stageCreate))
        {
            stageCreate.Create(0, RankLevel);
            
        }
        if (newStage.TryGetComponent(out RectTransform rectTransform))
        {
            rectTransform.anchoredPosition = new Vector3(-_offScreen.x, 0, 0);
            rectTransform.DOAnchorPosX(0, 1f)
                .OnComplete(() =>
                {
                    _stampArea.anchoredPosition = stageCreate.SstampFrame.anchoredPosition;
                });
            _stageCreate = rectTransform;
        }
    }
    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void StartGame()
    {
        //Time.timeScale = 1f;
        ChangeState(GameState.Playing);
        IsAddTime = true;
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
        //Time.timeScale = 1f;
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