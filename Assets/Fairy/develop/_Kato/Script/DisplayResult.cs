using TMPro;
using UnityEngine;

public class DisplayResult : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _post;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _time;

    GameManager _gameManager;
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _name.text = $"未実装";
        _post.text = $"{_gameManager.RankLevel.PostName}";
        _score.text = $"スコア：{_gameManager.Score}";
        _time.text = $"クリアタイム：{_gameManager.GameTimer}";
    }
}
