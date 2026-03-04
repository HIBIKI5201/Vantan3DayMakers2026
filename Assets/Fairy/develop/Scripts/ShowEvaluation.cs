using TMPro;
using UnityEngine;
using System;
/// <summary>
/// 未使用
/// </summary>
public class ShowEvaluation : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _postText;
    [SerializeField] private PostData[] _postDatas;

    public void ShowWindow(Post post, float time, int score)
    {
        _mainPanel.SetActive(true);
        _timeText.text = time.ToString("N2") + "秒";
        _scoreText.text = "スコア" + score.ToString();
        var result = Array.Find(_postDatas, (PostData x) => x.PostType == post);
        if (result != null)
        {
            _postText.text = result.PostName;
        }
    }

    public void HiddenWindow()
    {
        _mainPanel.SetActive(false);
    }
}
