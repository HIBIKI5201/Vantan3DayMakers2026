using TMPro;
using UnityEngine;
using System;
public class ShowEvaluation : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _postText;
    [SerializeField] private PromotionValue[] _promotionValues;

    public void ShowWindow(Post post, float time, int score)
    {
        _mainPanel.SetActive(true);
        _timeText.text = time.ToString("N2") + "秒";
        _scoreText.text = "スコア" + score.ToString();
        var result = Array.Find(_promotionValues, (PromotionValue x) => x.PostType == post);
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
