using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject _evaluationPrefab;
    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private GameObject _promotionPrefab;
    
    public void PlayEvaluationEffect(int score,RectTransform stampRect)
    {
        
        //GameObject newEvalutionEffect = Instantiate(_evaluationPrefab);
        //GameObject newScoreEffect = Instantiate(_scorePrefab);

        //データの設定など行う
    }
    public void PlayPromotionEffect(RectTransform stampRect)
    {
        GameObject newEvalutionEffect = Instantiate(_promotionPrefab,stampRect);
        newEvalutionEffect.transform.localPosition = Vector3.zero;
    }
}
