using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    public async UniTask StartCountdownAsync()
    {

        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
        if (countdownText == null) return;
            countdownText.text = i.ToString();

            countdownText.transform.localScale = Vector3.zero;
            countdownText.transform
                .DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack);

            await UniTask.Delay(1000);
        }

        countdownText.text = "START!";

        countdownText.transform.localScale = Vector3.zero;
        countdownText.transform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutBack);

        await UniTask.Delay(800);

        countdownText.gameObject.SetActive(false);
    }
}