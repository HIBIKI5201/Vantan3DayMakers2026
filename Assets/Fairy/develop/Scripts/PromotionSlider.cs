using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PromotionSlider : MonoBehaviour
{

    [SerializeField] private Image[] PromotionSliderImage;
    [SerializeField] private ImageSliderColor[] PromotionSliderColor;

    [SerializeField] private float doFade = 0.3f;
    public void SetData(int score, PostData postData)
    {
        for(int i = 0; i < PromotionSliderImage.Length; i++)
        {
            Image image = PromotionSliderImage[i];

            //FillAmount調整
            float fillAmount = (float)score / postData.PromotionScore;
            if(fillAmount >= (float)i + 1 / PromotionSliderImage.Length)
            {
                image.DOFillAmount(1, doFade);
            }
            else
            {
                image.DOFillAmount(fillAmount, doFade);
            }

            //色調整
            PromotionSliderColor[i].UpdateColor(image.fillAmount);
        }
    }
}
