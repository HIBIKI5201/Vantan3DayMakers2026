using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResult : MonoBehaviour
{
    [System.Serializable]
    public struct RankImageSetting
    {
        public string rankName;    // デバッグ用の名前（平社員、課長など）
        public Post post;
        public int threshold;      // この数値「以上」ならこの画像
        public Sprite rankSprite;  // 表示したい画像
        public Sprite frameSprite;  // 表示する枠組み
    }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _rankImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private GameObject _stampObj;
    [SerializeField] private Image[] _buttons;
    //[SerializeField] private PostDatabase _postDatabase;

    [Header("Rank Settings")]
    [SerializeField] private List<RankImageSetting> _rankSettings = new List<RankImageSetting>();

    [Header("Animation Settings")]
    [SerializeField] private float _duration = 0.8f;
    [SerializeField] private float _textAnimWaitSec = 0.5f;
    [SerializeField] private float _rankFadeSec = 0.2f;
    [SerializeField] private float _stampAnimWaitSec = 0.2f;
    [SerializeField] private float _buttonActiveSec = 0.3f;
    [SerializeField] private float _buttonFadeWaitSec = 0.15f;

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
        //_scoreText.text = $"スコア：{GameManager.Score:F1}";
        _timeText.text = $"タイム：{GameManager.GameTimer:F2}";
        _nameText.text = $"{DataManager.Instance.UserName}殿";


        StartCoroutine(AnimationScore(GameManager.Score));


        //デバック用
        //_scoreText.text = $"スコア：{score:F1}";
        //_timeText.text = $"タイム：{time:F2}";

        // スコアに応じた画像の差し替え
        SetRankImage(GameManager.RankLevel.PostType);

    }

    private IEnumerator AnimationScore(float targetScore)
    {
        _scoreText.text = $"スコア：{0:F1}";
        Color color = _rankImage.color;
        color.a = 0f;
        _rankImage.color = color;
        foreach (Image img in _buttons)
        {
            Color col = img.color;
            col.a = 0f;
            img.color = col;
        }

        yield return new WaitForSeconds(_textAnimWaitSec);

        float currentScore = 0f;

        DOTween.To(
            () => currentScore,
            x =>
            {
                currentScore = x;
                _scoreText.text = $"スコア：{currentScore:F1}";
            },
            targetScore,
            _duration
        )
        .SetEase(Ease.OutCubic)
        .OnComplete(() => _rankImage.DOFade(1f, _rankFadeSec)
            .SetEase(Ease.InBack));

        yield return new WaitForSeconds(_stampAnimWaitSec + _rankFadeSec);

        _stampObj.SetActive(true);

        yield return new WaitForSeconds(_buttonActiveSec);

        foreach (Image obj in _buttons)
        {
            obj.DOFade(1f, _buttonActiveSec);
            yield return new WaitForSeconds(_buttonFadeWaitSec);
        }
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
        foreach (RankImageSetting setting in _rankSettings)
        {
            if (setting.post == post)
            {
                _rankImage.sprite = setting.rankSprite;
                _frameImage.sprite = setting.frameSprite;
            }
        }
    }
}
