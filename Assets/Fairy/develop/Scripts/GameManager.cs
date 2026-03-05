using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
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
public class GameManager : MonoBehaviour
{
    [Header("パラメーター")]
    public static GameManager Instance { get; private set; }
    public static event Action<GameState> OnGameStateChanged;
    public GameState CurrentState { get; private set; }
    public static int Score
    {
        get => _temp;
        set
        {
            _temp = value;
        }
    }

    public static PostData RankLevel { get; private set; }
    public static float TimeLimit { get; private set; }
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
    [SerializeField] private float _resultWait;
    [SerializeField] private GameObject[] _stagePrefabs;
    [SerializeField] private Transform _stageParent;
    [SerializeField] private RectTransform _stampArea;
    [SerializeField] private SceneLoader _sceneLoader;
    private CountdownManager countdownManager;
    //private InGameUIManager _inGameUIManager;

    public RectTransform _stageCreate { get; private set; }
    private bool IsAddTime = false;

    private async void Awake()
    {
        _postDatabase.Initialize();

        CurrentState = GameState.Ready;
        RankLevel = _postDatabase.Get(Post.Staff);
        TimeLimit = RankLevel.TimeLimit;
        GameTimer = 0;
        Score = 0;


        if (Instance != null && Instance != this)
        {
            //Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        await UniTask.DelayFrame(1);

        countdownManager = FindFirstObjectByType<CountdownManager>();



        _uiManager.UpdateTimerUI(TimeLimit);
        _uiManager.UpdatePostUI(RankLevel.PostName);
        _uiManager.UpdateScoreUI(Score);
        _uiManager.ChangePost(RankLevel.PostType);
        _uiManager.UpdatePromotionScoreUI(Score, RankLevel);

        NextStage(ScoreLevel.Keep, true);
        if (countdownManager != null)
        {

            await countdownManager.StartCountdownAsync();
        }
        else
        {
            Debug.LogError("countdowon null");
        }


        IsAddTime = false;
        StartGame();
    }
    private void Update()
    {
        if (IsAddTime)
        {
            TimeLimit -= Time.deltaTime;
            GameTimer += Time.deltaTime;
            _uiManager?.UpdateTimerUI(TimeLimit);

        }
        if (TimeLimit <= 0f)
        {
            Debug.Log("B");
            _sceneLoader.LoadScene(2);
            //SceneController.LoadScene(SceneName.Result);//ゲームオーバー
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
        Vector3 stampPos = _stampPointor.ClonedStamp.transform.position;
        stampPos.z = 0f;
        Vector3 areaPos = _stampArea.transform.position;
        areaPos.z = 0f;
        float stampRot = _stampPointor.ClonedStamp.ImageRect.eulerAngles.z;

        int scoreAmount = _scoreManager.CalculationScore(stampPos, stampRot, areaPos, TimeLimit);
        AddScore(scoreAmount);
        var promotion = CheckRankUp(scoreAmount);

        //円エフェクト　ハンコを押したら無条件で表示
        _effectManager.PlayStampEffect(_stampPointor.ClonedStamp.MainRect);


        _stampEvaluation.ShowEvaluation(_stampPointor.ClonedStamp.gameObject, promotion);

        _effectManager.PlayScoreEffect(scoreAmount, _stampPointor.ClonedStamp.MainRect);

        _uiManager.UpdatePromotionScoreUI(Score, RankLevel);

        // --- 追加：スタンプSEを鳴らす ---
        AudioManager.Play(SEClipType.Stamp);

        IsAddTime = false;
        _stampPointor.IsCreateStamp = false;

        if(RankLevel.PostType == Post.President)
        {
            promotion = ScoreLevel.GameOver;
        }
        NextStage(promotion);
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
    private ScoreLevel CheckRankUp(int scoreAdd)
    {
        if (Score >= RankLevel.PromotionScore)
        {
            if (RankLevel.PostType == Post.President)
            {
                DOVirtual.DelayedCall(_resultWait, () =>
                {
                    _sceneLoader.LoadScene(2);
                    //SceneController.LoadScene(SceneName.Result);//クリア
                });
            }
            else
            {
                int next = ((int)RankLevel.PostType + 1) % System.Enum.GetValues(typeof(Post)).Length;
                RankLevel = _postDatabase.Get((Post)next);
                _uiManager.UpdatePostUI(RankLevel.PostName);
                _uiManager.ChangePost(RankLevel.PostType);
                _effectManager.PlayPromotionEffect();
            }

            return ScoreLevel.Perfect;
        }
        else if (scoreAdd > RankLevel.GameOverScore)
        {
            return ScoreLevel.Keep;
        }
        else
        {
            _effectManager.PlayGameOverEffect();
            DOVirtual.DelayedCall(_resultWait, () =>
            {
                _sceneLoader.LoadScene(2);
                //SceneController.LoadScene(SceneName.Result);//ゲームオーバー
            });
            return ScoreLevel.GameOver;
        }

    }
    public void NextStage(ScoreLevel score, bool skipReset = false)
    {
        // --- 追加：紙が動くSEを鳴らす ---
        AudioManager.Play(SEClipType.Paper);

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
        if (score != ScoreLevel.GameOver)
        {
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
                        TimeLimit = RankLevel.TimeLimit;
                        if (!skipReset)
                        {
                            _stampPointor.IsCreateStamp = true;
                            IsAddTime = true;
                        }
                    }).SetDelay(_nextDelay);
                _stageCreate = rectTransform;
            }
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        IsAddTime = true;
        _stampPointor.IsCreateStamp = true;
    }
}