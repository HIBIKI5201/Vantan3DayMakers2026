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
        _name.text = $"";
        _post.text = $"";
        _score.text = $"{_gameManager.Score}";
        _time.text = $"";
    }
}
