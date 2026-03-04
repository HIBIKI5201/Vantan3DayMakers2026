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
    Promotion,
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

    public int Score { get; private set; }

    public PostData RankLevel { get; private set; }
    public float ClearTime { get; private set; }
    public float GameTimer { get; private set; }

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
        _postDatabase.Initialize();

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
        RankLevel = _postDatabase.Get(Post.Staff);
        ClearTime = RankLevel.TimeLimit;
        GameTimer = 0;

        NextStage();

        _uiManager.UpdateTimerUI(ClearTime);
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
            ClearTime -= Time.deltaTime;
            GameTimer += Time.deltaTime;
            _uiManager.UpdateTimerUI(ClearTime);
            if (ClearTime <= 0f)
            {
                SceneController.LoadScene(SceneName.Result);//ゲームオーバー
            }
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

        Vector2 sPos = _stampPointor.ClonedStamp.anchoredPosition;
        Vector2 aPos = _stampArea.anchoredPosition;
        float sRot = _stampPointor.ClonedStamp.eulerAngles.z;

        int scoreAmount = _scoreManager.CalculationScore(sPos, sRot, sPos, ClearTime);
        AddScore(scoreAmount);
        var promotion = CheckRankUp();
        if (promotion == ScoreLevel.Promotion)//昇進した時の処理
        {
        _effectManager.PlayPromotionEffect(_stampPointor.ClonedStamp);
        }
        _stampEvaluation.ShowEvaluation(_stampPointor.ClonedStamp.gameObject, promotion);

        _effectManager.PlayEvaluationEffect(scoreAmount, _stampPointor.ClonedStamp);

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

        if (Score>= RankLevel.PromotionScore)
        {
            if (RankLevel.PostType == Post.President)
            {
                SceneController.LoadScene(SceneName.Result);//クリア
                return ScoreLevel.Promotion;
            }
            int next = ((int)RankLevel.PostType + 1) % System.Enum.GetValues(typeof(Post)).Length;
            RankLevel = _postDatabase.Get((Post)next);
            _uiManager.UpdatePostUI(RankLevel.PostName);
            _uiManager.ChangePost(RankLevel.PostType);
            ClearTime = RankLevel.TimeLimit;
            return ScoreLevel.Promotion;
        }
        else if(Score > RankLevel.GameOverScore) 
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