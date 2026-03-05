using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private ScorePopupCreate _scorePopupCreate;
    [SerializeField] private GameObject _promotionPrefab;
    [SerializeField] private GameObject _gameOverStampPrefab;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private PromotionInfoAnimation _promotionInfoAnimation;
    public void PlayScoreEffect(int score,RectTransform stampRect)
    {
        _scorePopupCreate.Create(score, stampRect);
    }
    public void PlayStampEffect(RectTransform stampRect)
    {
        GameObject newEvalutionEffect = Instantiate(_promotionPrefab,stampRect);
        newEvalutionEffect.transform.localPosition = Vector3.zero;
    }
    public void PlayPromotionEffect()
    {
        _promotionInfoAnimation.PlayAnimation();
    }
    public void PlayGameOverEffect()
    {
        GameObject newEffect = Instantiate(_gameOverStampPrefab, _mainCanvas.transform);
        newEffect.transform.localPosition = Vector3.zero;
        newEffect.transform.localScale = Vector3.one;
    }
}
