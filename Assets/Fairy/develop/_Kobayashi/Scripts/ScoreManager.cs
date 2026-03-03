using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スコアの計算
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("スコア設定")]
    [SerializeField, Tooltip("最大スコア")] private int _maxScore = 50;
    [SerializeField, Tooltip("最大スコア時の判定度")] private float _perfectJudgement = 0.2f;
    //[SerializeField, Tooltip("各評価軸のデータ")] private List<ScoreTimeData> _evaluationData;

    //評価軸
    [Tooltip("正しい位置")] private Vector2 _correctPosition;
    [Tooltip("正しい角度")] private float _correctRotation;
    [Tooltip("制限時間")] private float _timeLimit;

    [Tooltip("位置の猶予")] private float _gracePeriodPos;
    [Tooltip("角度の猶予")] private float _gracePeriodRot;
    [Tooltip("最大スコアの時間")] private float _perfectTime;


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
    public int CalculationScore(Vector2 stampPos,float stampRot,float remainingTime)
    {
        //todo,ゲーマネから正解の情報を取得
        //(例)
        //_correctPosition = _gameManager.正解のポジション
        //_correctRotation = _gameManager.正解の角度
        //_timeLimit = _gameManager.一回の制限時間

        //todo,猶予を現在の状態をみて調整
        //_gracePeriodPos = ますたーでーた.判定猶予距離
        //_gracePeriodRot = ますたーでーた.判定猶予角度
        //
        //float time = GetPerfectTime(_gameManager.今の役職);
        //→これもマスターデータから引っ張ってくるかも

        int score = 0;

        //位置計算
        float distance = Vector2.Distance(stampPos, _correctPosition);
        if(distance <= _gracePeriodPos * _perfectJudgement)
        {
            //位置スコア最大
            score += _maxScore / 3;
        }
        else if (distance <= _gracePeriodPos)
        {
            //どれぐらい離れているかを判定
            float accuracy = Mathf.Clamp01(1f - (distance / _gracePeriodPos));

            //それに応じたスコアを計算
            score += Mathf.RoundToInt((_maxScore / 3f) * accuracy);
        }

        //角度計算
        float angle = Mathf.Abs(Mathf.DeltaAngle(stampRot, _correctRotation));
        if (angle <= _gracePeriodRot * _perfectJudgement)
        {
            //角度スコア最大
            score += _maxScore / 3;
        }
        else if(angle <= _gracePeriodRot)
        {
            //どれぐらい角度が異なるかを計算
            float accuracy = Mathf.Clamp01(1f - (angle / _gracePeriodRot));

            //それに応じたスコアを加算
            score += Mathf.RoundToInt((_maxScore / 3f) * accuracy);
        }

        //時間計算
        float time = 0;
        if(remainingTime　<= time)
        {
            //時間スコア最大
            score += _maxScore / 3;
        }
        else
        {
            //どれぐらい時間に差があるかを計算
            float accuracy = Mathf.Clamp01(1f - (remainingTime / _timeLimit));

            //それに応じたスコアを加算
            score += Mathf.RoundToInt((_maxScore / 3f) * accuracy);
        }


            return score;
    }

    ///// <summary>
    ///// データから判定時間を返す
    ///// </summary>
    ///// <param name="post">現在の役職</param>
    ///// <returns></returns>
    //private float GetPerfectTime(Post post)
    //{
    //    foreach(ScoreTimeData data in _evaluationData)
    //    {
    //        if(data.Post == post)
    //        {
    //            return data.PerfectTimeRemaining;
    //        }
    //    }

    //    return 0;
    //}
}

//マスターデータに変更

//[Serializable]
//public class ScoreTimeData
//{
//    [Header("役職")]
//    private Post _post;

//    [Header("最大スコア時の残り時間")]
//    private float _perfectTimeRemaining;

//    public Post Post => _post;
//    public float PerfectTimeRemaining => _perfectTimeRemaining;
//}
