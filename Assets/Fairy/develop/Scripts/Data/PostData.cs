using UnityEngine;

[CreateAssetMenu(fileName = "PostData")]
public class PostData : ScriptableObject
{
    [Header("役職")]
    public Post PostType;

    [Header("役職名")]
    public string PostName;

    [Header("昇進スコア")]
    public int PromotionScore;   
    [Header("失敗スコア")]
    public int GameOverScore;
    [Header("==========")]
    [Header("目標角度")]
    public float PerfectAngle;
    [Header("目標時間")]
    public float PerfectTime;
    [Header("==========")]

    [Header("許容角度")]
    public float ToleranceAngle;
    [Header("許容距離差")]
    public float TolerancePosition;
    [Header("==========")]

    [Header("最大角度スコア")]
    public float MaxAngleScore;
    [Header("最大位置スコア")]
    public float MaxPositionScore;
    [Header("最大時間スコア")]
    public float MaxTimeScore;

    [Header("==========")]

    [Header("制限時間")]
    public float TimeLimit;

    [Header("画像")]
    public Sprite[] HandImage;
    public Sprite CharacterImage;
    public Sprite BowImage;
    public Sprite StampImage;
}