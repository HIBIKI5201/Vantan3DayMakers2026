using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("スコア設定")]
    [SerializeField, Tooltip("最大スコア")] private int _maxScore = 45;
    [SerializeField, Tooltip("最大スコア時の判定度")] private float _perfectJudgement = 0.2f;
    [SerializeField, Tooltip("最大スコアの時間")] private float _perfectTime = 5f;
    [SerializeField, Tooltip("評価の減衰率"),Range(0,20)] private int _attenuationRate = 10;

    //評価軸
    [Tooltip("正しい位置")] private Vector2 _correctPosition;
    [Tooltip("正しい角度")] private float _correctRotation;
    [Tooltip("最短時間")] private float _minTime;

    [Tooltip("位置の猶予")] private float _gracePeriodPos;
    [Tooltip("角度の猶予")] private float _gracePeriodRot;


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
    /// <param name="time">押すまでの時間</param>
    /// <returns></returns>
    public int CalculationScore(Vector2 stampPos,float stampRot,float time)
    {
        //ゲーマネから正解の情報を取得


        //猶予を現在の状態をみて調整


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
        if(time <= _perfectTime)
        {
            //時間スコア最大
            score += _maxScore / 3;
        }
        //else if(time <= )

            return score;
    }
}
