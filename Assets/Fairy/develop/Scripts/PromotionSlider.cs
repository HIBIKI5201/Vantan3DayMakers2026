using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PromotionSlider : MonoBehaviour
{

    [SerializeField] private Image[] PromotionSliderImage;

    [SerializeField] private float doFade = 0.3f;
    public void SetData(int score, PostData postData)
    {
        int segmentCount = PromotionSliderImage.Length;
        float totalFill = (float)score / postData.PromotionScore;
        totalFill = Mathf.Clamp01(totalFill);

        float perSegment = 1f / segmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            Image image = PromotionSliderImage[i];

            float segmentStart = i * perSegment;
            float segmentEnd = (i + 1) * perSegment;

            float fill;

            if (totalFill >= segmentEnd)
            {
                fill = 1f;
            }
            else if (totalFill <= segmentStart)
            {
                fill = 0f;
            }
            else
            {
                // 部分的に埋める
                fill = (totalFill - segmentStart) / perSegment;
            }

            image.DOFillAmount(fill, doFade);
        }
    }
}
