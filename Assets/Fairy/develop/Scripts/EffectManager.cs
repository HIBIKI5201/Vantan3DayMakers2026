using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private ScorePopupCreate _scorePopupCreate;
    [SerializeField] private GameObject _promotionPrefab;
    
    public void PlayScoreEffect(int score,RectTransform stampRect)
    {
        _scorePopupCreate.Create(score, stampRect);
    }
    public void PlayStampEffect(RectTransform stampRect)
    {
        GameObject newEvalutionEffect = Instantiate(_promotionPrefab,stampRect);
        newEvalutionEffect.transform.localPosition = Vector3.zero;
    }
}
