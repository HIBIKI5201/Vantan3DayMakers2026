using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData")]
public class ScoreData : ScriptableObject
{
    [Header("役職")]
    public Post Post;

    [Header("ゲームオーバースコア（未満）")]
    public int GameOverScore;

    [Header("許容値")]
    public float TolerancePosition;
    public float ToleranceRotation;

    [Header("理想タイム")]
    public float PerfectTime;

    [Header("制限時間")]
    public float TimeLimit;

    [Header("おじぎ角度")]
    public float BowAmount;
}