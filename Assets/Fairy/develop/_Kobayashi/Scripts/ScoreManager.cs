using System;
using System.Collections.Generic;
using UnityEngine;
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
    }

    /// <summary>
    /// ハンコスコアの計算
    /// </summary>
    /// <param name="stampPos">ハンコの位置</param>
    /// <param name="stampRot">ハンコの角度</param>
    /// <param name="remainingTime">押されたときの余りの時間</param>
    /// <returns></returns>
    public int CalculationScore(Vector2 stampPos, float stampRot, Vector2 correcPos, float remainingTime)
    {
        GetPostData();

        //todo;ゲーマネから正解の情報を取得
        _correctPosition = correcPos;
        //todo,ゲーマネから正解の情報を取得
        //(例)
        //_correctPosition = _gameManager.正解のポジション
        //_correctRotation = _gameManager.正解の角度
        _timeLimit = GameManager.RankLevel.TimeLimit;

        //todo,猶予を現在の状態をみて調整
        //_gracePeriodPos = ますたーでーた.判定猶予距離
        //_gracePeriodRot = ますたーでーた.判定猶予角度
        //
        //float time = GetPerfectTime(_gameManager.今の役職);
        //→これもマスターデータから引っ張ってくるかも

        float score = 0;

        //位置計算
        float distance = Vector2.Distance(stampPos, _correctPosition);
        if (distance <= _gracePeriodPos )
        {
            //位置スコア最大
            score += _maxPositionScore;
            Debug.Log("位置 :" + _maxPositionScore);
        }
        else
        {
            //どれぐらい離れているかを判定
            float accuracy = Mathf.Clamp01(1f - (distance / _gracePeriodPos));

            //それに応じたスコアを計算
            score += Mathf.RoundToInt(_maxPositionScore * accuracy);
            Debug.Log("位置 :" + Mathf.RoundToInt(_maxPositionScore * accuracy));
        }

        //角度計算
        float angleDiff = Mathf.Abs(_correctRotation - stampRot);
        if (angleDiff <= _gracePeriodRot)
        {
            //角度スコア最大
            score += _maxRotationScore;
            Debug.Log("角度 :" + _maxRotationScore);
        }
        else
        {
            //どれぐらい角度が異なるかを計算
            float accuracy = Mathf.Clamp01(1f - (angleDiff / _gracePeriodRot));

            //それに応じたスコアを加算
            score += Mathf.RoundToInt(_maxRotationScore * accuracy);
            Debug.Log("角度 :" + Mathf.RoundToInt(_maxRotationScore * accuracy));
        }

        //時間計算
        float limitHalfTime = _timeLimit / 2;
        if (limitHalfTime <= remainingTime)
        {
            //時間スコア最大
            score += _maxTimeScore;
            Debug.Log("時間 :" +_maxTimeScore);   
        }
        else
        {
            //どれぐらい時間に差があるかを計算
            float accuracy = Mathf.Clamp01(remainingTime / limitHalfTime);

            //それに応じたスコアを加算
            score += Mathf.RoundToInt(_maxTimeScore * accuracy);
            Debug.Log("時間 :" + Mathf.RoundToInt(_maxTimeScore * accuracy));
        }

        Debug.Log("総合 :" + score);
        return (int)score;
    }

    /// <summary>
    /// データから判定時間を返す
    /// </summary>
    /// <param name="post">現在の役職</param>
    /// <returns></returns>
    private void GetPostData()
    {
        PostData postData = GameManager.RankLevel;
        Post post = postData.PostType;

        PostData data = _postDatabase.Get(post);

        _gracePeriodPos = data.TolerancePosition;
        _gracePeriodRot = data.ToleranceAngle;
        _maxPositionScore = data.MaxPositionScore;
        _maxRotationScore = data.MaxAngleScore;
        _maxTimeScore = data.MaxTimeScore;
        _correctRotation = data.PerfectAngle;

    }
}
