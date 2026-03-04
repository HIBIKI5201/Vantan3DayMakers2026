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

    [Header("許容値")]
    public float TolerancePosition;
    public float ToleranceRotation;

    [Header("理想タイム")]
    public float PerfectTime;

    [Header("制限時間")]
    public float TimeLimit;

    [Header("おじぎ角度")]
    public float BowAmount;

    [Header("画像")]
    public Sprite HandImage;
    public Sprite CharacterImage;
    public Sprite BowImage;
}