using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
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
public enum ScoreLevel
{
    Perfect,
    Keep,
    GameOver
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

    public static int Score
    {
        get => _temp;
        set
        {
            _temp = value;
            Debug.Log("A");
        }
    }

    public static PostData RankLevel { get; private set; }
    public static float TimeLimit{ get; private set; }
    private static int _temp;
    public static float GameTimer { get; private set; }

    [Header("アタッチ")]
    [SerializeField] private PostDatabase _postDatabase;
    [SerializeField] private StampPointor _stampPointor;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private InGameUIManager _uiManager;
    [SerializeField] private EffectManager _effectManager;
    [SerializeField] private StampEvaluation _stampEvaluation;
    [SerializeField] private Vector2 _offScreen;
    [SerializeField] private float _nextDelay;
    [SerializeField] private GameObject[] _stagePrefabs;
    [SerializeField] private Transform _stageParent;
    [SerializeField] private RectTransform _stampArea;
    private CountdownManager countdownManager;
    //private InGameUIManager _inGameUIManager;

    public RectTransform _stageCreate { get; private set; }
    private bool IsAddTime = false;

    private async void Awake()
    {

        CurrentState = GameState.Ready;
        RankLevel = _postDatabase.Get(Post.Staff);
        TimeLimit = RankLevel.TimeLimit;
        GameTimer = 0;

        _postDatabase.Initialize();

        if (Instance != null && Instance != this)
        {
            //Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        await UniTask.DelayFrame(1);

        countdownManager = FindFirstObjectByType<CountdownManager>();

        
        NextStage();

        _uiManager.UpdateTimerUI(TimeLimit);
        _uiManager.UpdatePostUI(RankLevel.PostName);
        _uiManager.UpdateScoreUI(Score);
        _uiManager.ChangePost(RankLevel.PostType);

        if (countdownManager != null)
            await countdownManager.StartCountdownAsync();


        IsAddTime = false;
        StartGame();
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
            TimeLimit -= Time.deltaTime;
            GameTimer += Time.deltaTime;
            _uiManager?.UpdateTimerUI(TimeLimit);

        }
        if (TimeLimit <= 0f)
        {
            Debug.Log("B");
            SceneController.LoadScene(SceneName.Result);//ゲームオーバー
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
        Vector2 sPos = _stampPointor.ClonedStamp.ImageRect.anchoredPosition;
        Vector2 aPos = _stampArea.anchoredPosition;
        float sRot = _stampPointor.ClonedStamp.ImageRect.eulerAngles.z;

        int scoreAmount = _scoreManager.CalculationScore(sPos, sRot, sPos, TimeLimit);
        AddScore(scoreAmount);
        var promotion = CheckRankUp();

        //円エフェクト　ハンコを押したら無条件で表示
        _effectManager.PlayPromotionEffect(_stampPointor.ClonedStamp.MainRect);

        //円エフェクト　ハンコを押したら無条件で表示
        _stampEvaluation.ShowEvaluation(_stampPointor.ClonedStamp.gameObject, promotion);

        _effectManager.PlayEvaluationEffect(scoreAmount, _stampPointor.ClonedStamp.MainRect);

        IsAddTime = false;
        _stampPointor.IsCreateStamp = false;

        NextStage();
        //_showEvaluation.ShowWindow(RankLevel, ClearTime, scoreAmount);
    }
    /// <summary>
    /// Score追加
    /// </summary>
    public void AddScore(int amount)
    {
        Score += amount;
        _uiManager.UpdateScoreUI(Score);

    }

    /// <summary>
    /// 昇進チェック
    /// </summary>
    private ScoreLevel CheckRankUp()
    {

        if (Score >= RankLevel.PromotionScore)
        {
            if (RankLevel.PostType == Post.President)
            {
                SceneController.LoadScene(SceneName.Result);//クリア
            }
            int next = ((int)RankLevel.PostType + 1) % System.Enum.GetValues(typeof(Post)).Length;
            RankLevel = _postDatabase.Get((Post)next);
            _uiManager.UpdatePostUI(RankLevel.PostName);
            _uiManager.ChangePost(RankLevel.PostType);
            TimeLimit = RankLevel.TimeLimit;


            return ScoreLevel.Perfect;
        }
        else if (Score > RankLevel.GameOverScore)
        {
            return ScoreLevel.Keep;
        }
        else
        {
            SceneController.LoadScene(SceneName.Result);//ゲームオーバー
            return ScoreLevel.GameOver;
        }

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
                }).SetDelay(_nextDelay);
        }

        _stampPointor.RemoveStampObject();
        GameObject prefab = _stagePrefabs[Random.Range(0, _stagePrefabs.Length)];
        GameObject newStage = Instantiate(prefab, _stageParent);
        if (newStage.TryGetComponent(out StageCreate stageCreate))
        {

            stageCreate.Create(RankLevel.PerfectAngle, RankLevel.PostType);

        }
        if (newStage.TryGetComponent(out RectTransform rectTransform))
        {
            rectTransform.anchoredPosition = new Vector3(-_offScreen.x, 0, 0);
            rectTransform.DOAnchorPosX(0, 1f)
                .OnComplete(() =>
                {
                    _stampArea.anchoredPosition = stageCreate.SstampFrame.anchoredPosition;
                    _stampPointor.IsCreateStamp = true;
                    IsAddTime = true;
                }).SetDelay(_nextDelay);
            _stageCreate = rectTransform;
        }
    }
    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void StartGame()
    {
        ChangeState(GameState.Playing);
        IsAddTime = true;
        _stampPointor.IsCreateStamp = true;
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