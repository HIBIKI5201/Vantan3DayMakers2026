using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
/// <summary>
/// スコアの計算
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("スコア設定")]
    //[SerializeField, Tooltip("最大スコア")] private int _maxScore = 50;
    [SerializeField, Tooltip("各評価軸のデータ")] private PostDatabase _postDatabase;

    //評価軸
    [Tooltip("正しい位置")] private Vector2 _correctPosition;
    [Tooltip("正しい角度")] private float _correctRotation;
    [Tooltip("最低角度")] private float _worstPosition;
    [Tooltip("制限時間")] private float _timeLimit;

    [Tooltip("位置の猶予")] private float _gracePeriodPos;
    [Tooltip("角度の猶予")] private float _gracePeriodRot;
    [Tooltip("最大スコアの時間")] private float _perfectTime;

    [Tooltip("最大位置スコア")] private float _maxPositionScore;
    [Tooltip("最大角度スコア")] private float _maxRotationScore;
    [Tooltip("最大時間スコア")] private float _maxTimeScore;


    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        GetPostData(Post.Staff);
        string st = ""; 
        for(int i = 0; i < 200; i++)
        {
            float distance = i;
            st += $"距離{distance} スコア";
            float score;
            if (distance <= _gracePeriodPos)
            {
                score = _maxPositionScore;
            }
            else if (distance >= _worstPosition)
            {
                score = 0f;
            }
            else
            {
                float t = (distance - _gracePeriodPos) /
                          (_worstPosition - _gracePeriodPos);

                score = Mathf.Lerp(_maxPositionScore, 0f, t);
            }
            st += Mathf.RoundToInt(score) + "\n";
        }
        Debug.Log(st);
    }

    /// <summary>
    /// ハンコスコアの計算
    /// </summary>
    /// <param name="stampPos">ハンコの位置</param>
    /// <param name="stampRot">ハンコの角度</param>
    /// <param name="remainingTime">押されたときの余りの時間</param>
    /// <returns></returns>
    public int CalculationScore(Vector3 stampPos, float stampRot, Vector3 correcPos, float remainingTime)
    {
        GetPostData(GameManager.RankLevel.PostType);

        //todo;ゲーマネから正解の情報を取得
        _correctPosition = correcPos;
        //todo,ゲーマネから正解の情報を取得

        _timeLimit = GameManager.RankLevel.TimeLimit;

        float score = 0;
        string resultSt = "==計算結果==" + "\n";
        string inputSt = "==入力情報==" + "\n";
        //位置計算
        float distance = Vector3.Distance(stampPos, _correctPosition);
        inputSt += "位置 : " + distance + "\n";
        float posScore = 0;
        if (distance <= _gracePeriodPos)
        {
            posScore = _maxPositionScore;
            resultSt += "位置　許容 :" + posScore + "\n";
        }
        else if (distance >= _worstPosition)
        {
            posScore = 0f;
            resultSt += "位置　最低 :" + posScore + "\n";
        }
        else
        {
            float t = (distance - _gracePeriodPos) /
                      (_worstPosition - _gracePeriodPos);

            posScore = Mathf.Lerp(_maxPositionScore, 0f, t);
            resultSt += "位置 :" + posScore + "\n";
        }
        score += posScore;

        //角度計算
        float angleDiff = Mathf.Abs(_correctRotation - stampRot);
        inputSt += "角度 : " + angleDiff + "\n";
        if (angleDiff <= _gracePeriodRot)
        {
            //角度スコア最大
            score += _maxRotationScore;
            resultSt += "角度  許容:" + _maxRotationScore + "\n";
        }
        else
        {
            //どれぐらい角度が異なるかを計算
            float removeRate = (angleDiff - _gracePeriodRot) / _gracePeriodRot;
            removeRate = Mathf.Clamp01(removeRate);
            float removeScore = _maxRotationScore * removeRate;

            //それに応じたスコアを加算
            score += Mathf.RoundToInt(_maxRotationScore -removeScore);
            resultSt += "角度 :" + Mathf.RoundToInt(_maxRotationScore -removeScore) + "\n";
        }

        //時間計算
        float limitHalfTime = _timeLimit / 2f;
        inputSt += "時間 : " + remainingTime + "\n";
        if (limitHalfTime <= remainingTime)
        {
            //時間スコア最大
            score += _maxTimeScore;
            resultSt += "時間 許容:" + _maxTimeScore + "\n";
        }
        else
        {
            //どれぐらい時間に差があるかを計算
            float removeRate = (remainingTime - limitHalfTime) / limitHalfTime;
            removeRate = Mathf.Clamp01(removeRate);
            float removeScore = _maxTimeScore * removeRate;

            //それに応じたスコアを加算
            score += Mathf.RoundToInt(_maxTimeScore - removeScore);
            resultSt += "時間 :" + Mathf.RoundToInt(_maxTimeScore - removeScore) + "\n";
        }

        resultSt += "総合 :" + score;
        Debug.Log(inputSt);
        Debug.Log(resultSt);
        return (int)score;
    }

    /// <summary>
    /// データから判定時間を返す
    /// </summary>
    /// <param name="post">現在の役職</param>
    /// <returns></returns>
    private void GetPostData(Post post)
    {

        PostData data = _postDatabase.Get(post);

        _gracePeriodPos = data.TolerancePosition;
        _gracePeriodRot = data.ToleranceAngle;
        _maxPositionScore = data.MaxPositionScore;
        _maxRotationScore = data.MaxAngleScore;
        _maxTimeScore = data.MaxTimeScore;
        _correctRotation = data.PerfectAngle;
        _worstPosition = data._worstPosition;

    }
}
