using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResult : MonoBehaviour
{
    [System.Serializable]
    public struct RankImageSetting
    {
        public string rankName;    // デバッグ用の名前（平社員、課長など）
        public int threshold;      // この数値「以上」ならこの画像
        public Sprite rankSprite;  // 表示したい画像
    }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _rankImage;

    [Header("Rank Settings")]
    [SerializeField] private List<RankImageSetting> _rankSettings = new List<RankImageSetting>();

    [Header("Debug")]
    [SerializeField] private bool _useDebugScore = false;
    [SerializeField] private int _debugScore = 700;
    [SerializeField] private float _debugTime = 123.45f;
    void Start()
    {
        // GameManager.Instance が存在するかチェック
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManagerが見つかりません。リザルトを表示できません。");
            return;
        }
        int score;
        float time;

        if (_useDebugScore)
        {
            score = _debugScore;
            time = _debugTime;
        }
        else
        {
            // 本番モード：GameManagerから取得
            if (GameManager.Instance == null) return;
            score = GameManager.Instance.Score;
            time = GameManager.Instance.GameTimer;
        }
        // スコアとタイムのテキスト表示 (F2は小数点2桁)
        _scoreText.text = $"スコア：{GameManager.Instance.Score:F1}";
        _timeText.text = $"タイム：{GameManager.Instance.GameTimer:F2}";

        //デバック用
        //_scoreText.text = $"スコア：{score:F1}";
        //_timeText.text = $"タイム：{time:F2}";

        // スコアに応じた画像の差し替え
        SetRankImage(GameManager.Instance.Score);

    }
#if UNITY_EDITOR
    // インスペクターで値を書き換えた瞬間に実行される
    private void OnValidate()
    {
        // 実行中でなくても、プレビューとして反映させる
        if (_scoreText != null) _scoreText.text = $"スコア：{_debugScore}";
        if (_timeText != null) _timeText.text = $"タイム：{_debugTime:F2}";
        SetRankImage(_debugScore);
    }
#endif
    private void SetRankImage(int finalScore)
    {
        if (_rankSettings == null || _rankSettings.Count == 0) return;

        // しきい値が高い順に並び替えて、スコアが条件を満たした最初の画像を採用する
        var target = _rankSettings
            .OrderByDescending(x => x.threshold)
            .FirstOrDefault(x => finalScore >= x.threshold);

        if (target.rankSprite != null)
        {
            Debug.Log($"<color=yellow>[ResultTest] Score: {finalScore} -> Rank: {target.rankName}</color>");
            _rankImage.sprite = target.rankSprite;
            // 画像のサイズを元画像の比率に合わせたい場合は以下を有効に
            // _rankImage.SetNativeSize(); 
        }
        else
        {
            Debug.LogError($"[ResultTest] スコア {finalScore} に該当するランク設定がありません！");
        }
    }
}