using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary> InGameのUI処理全般 </summary>
public class InGameUIManager : InGameUIObjects
{
    public void UpdateScoreUI(int amount)
    {
        ScoreText.text = (amount/10).ToString("N1");
    }
    public void UpdateTimerUI(float amount)
    {
        TimerText.text = amount.ToString("N2") + "秒";
    }
    public void UpdatePostUI(string postName)
    {
        PostText.text = postName;
    }
    public void ChangePost(Post post)
    {
        _handImage.ChangePost(post);
        _characterImage.ChangePost(post);
        _bowImage.ChangePost(post);
        _stampImage.ChangePost(post);
    }
}

public class InGameUIObjects : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI ScoreText;
    [SerializeField] protected TextMeshProUGUI TimerText;
    [SerializeField] protected TextMeshProUGUI PostText;
    [SerializeField] protected PostImage _handImage;
    [SerializeField] protected PostImage _characterImage;
    [SerializeField] protected PostImage _bowImage;
    [SerializeField] protected PostImage _stampImage;
}