using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary> InGameのUI処理全般 </summary>
public class InGameUIManager : InGameUIObjects
{
    public void UpdateScoreUI(int amount)
    {
        ScoreText.text = amount.ToString();
    }
    public void UpdateTimerUI(float amount)
    {
        TimerText.text = amount.ToString("N2") + "秒";
    }
    public void UpdatePostUI(string postName)
    {
        PostText.text = postName;
    }
}

public class InGameUIObjects : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI ScoreText;
    [SerializeField] protected TextMeshProUGUI TimerText;
    [SerializeField] protected TextMeshProUGUI PostText;
}