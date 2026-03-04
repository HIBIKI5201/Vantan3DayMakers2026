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
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _rankImage;
    [SerializeField] private PostDatabase _postDatabase;

    [Header("Rank Settings")]
    [SerializeField] private List<RankImageSetting> _rankSettings = new List<RankImageSetting>();

    [Header("Debug")]
    [SerializeField] private bool _useDebugScore = false;
    [SerializeField] private int _debugScore = 700;
    [SerializeField] private Post _debugPost;
    [SerializeField] private float _debugTime = 123.45f;
    void Start()
    {
        // GameManager.Instance が存在するかチェック
        //if (GameManager.Instance == null)
        //{
        //    Debug.LogWarning("GameManagerが見つかりません。リザルトを表示できません。");
        //    return;
        //}
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
            score = GameManager.Score;
            time = GameManager.GameTimer;
        }
        // スコアとタイムのテキスト表示 (F2は小数点2桁)
        _scoreText.text = $"スコア：{GameManager.Score:F1}";
        _timeText.text = $"タイム：{GameManager.GameTimer:F2}";
        _nameText.text = $"{DataManager.Instance.UserName}殿";

        //デバック用
        //_scoreText.text = $"スコア：{score:F1}";
        //_timeText.text = $"タイム：{time:F2}";

        // スコアに応じた画像の差し替え
        SetRankImage(GameManager.RankLevel.PostType);

    }
#if UNITY_EDITOR
    // インスペクターで値を書き換えた瞬間に実行される
    private void OnValidate()
    {
        // 実行中でなくても、プレビューとして反映させる
        //if (_scoreText != null) _scoreText.text = $"スコア：{_debugScore}";
        //if (_timeText != null) _timeText.text = $"タイム：{_debugTime:F2}";
        //SetRankImage(_debugPost);
    }
#endif
    private void SetRankImage(Post post)
    {
        _rankImage.sprite = _postDatabase.Get(post).ResultPostImage;
    }
}