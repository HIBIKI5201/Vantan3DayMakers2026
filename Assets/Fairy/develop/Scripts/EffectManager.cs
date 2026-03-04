using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private Vector2 _scorePopupOffset;
    [SerializeField] private GameObject _promotionPrefab;
    
    public void PlayEvaluationEffect(int score,RectTransform stampRect)
    {

        GameObject newScoreEffect = Instantiate(_scorePrefab,stampRect);
        if(newScoreEffect.TryGetComponent(out ScoreTextPopup scoreTextPopup))
        {
            scoreTextPopup.Rect.anchoredPosition = _scorePopupOffset;
            scoreTextPopup.UpdateText(score);
        }
        //データの設定など行う
    }
    public void PlayPromotionEffect(RectTransform stampRect)
    {
        GameObject newEvalutionEffect = Instantiate(_promotionPrefab,stampRect);
        newEvalutionEffect.transform.localPosition = Vector3.zero;
    }
}
