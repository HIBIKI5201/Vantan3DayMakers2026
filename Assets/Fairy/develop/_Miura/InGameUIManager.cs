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
}

public class InGameUIObjects : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI ScoreText;
}