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
    [Header("==========")]
    [Header("目標角度")]
    public float PerfectAngle;
    [Header("目標位置")]
    public float PerfectPosition;//多分関数の引数から読み取ることになるだろう
    [Header("目標時間")]
    public float PerfectTime;
    [Header("==========")]

    [Header("許容角度")]
    public float ToleranceAngle;
    [Header("許容位置")]
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

    [Header("おじぎ角度")]
    public float BowAmount;

    [Header("画像")]
    public Sprite HandImage;
    public Sprite CharacterImage;
    public Sprite BowImage;
}