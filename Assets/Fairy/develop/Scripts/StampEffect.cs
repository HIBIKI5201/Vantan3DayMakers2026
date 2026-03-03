using UnityEngine;
using DG.Tweening;

public class StampEffect : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(1f, 0.2f)
            .SetEase(Ease.OutBack);
    }
}