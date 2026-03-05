using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PromotionSlider : MonoBehaviour
{

    [SerializeField] private Image[] PromotionSliderImage;
    [SerializeField] private ImageSliderColor[] PromotionSliderColor;
    public void SetData(int score, PostData postData)
    {
        int index = 1;
        float doFade = 0.3f;
        foreach (var image in PromotionSliderImage)
        {
            if (index < (int)postData.PostType)
            {
                image.DOFillAmount(1, doFade);
            }
            else if (index > (int)postData.PostType)
            {
                image.DOFillAmount(0, doFade);
            }
            else if (index == (int)postData.PostType)
            {
                image.DOFillAmount((float)score / postData.PromotionScore, doFade);
            }

            PromotionSliderColor[index - 1].UpdateColor(image.fillAmount);
            index++;
        }
    }
}
