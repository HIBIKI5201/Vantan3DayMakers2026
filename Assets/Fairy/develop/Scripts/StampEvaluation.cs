using DG.Tweening;
using TMPro;
using UnityEngine;
public class StampEvaluation : MonoBehaviour
{
    [SerializeField] private GameObject _evaluationPrefab;
    [SerializeField] private float _positionY;
    [Header("テキスト")]
    [SerializeField] private string _missText;
    [SerializeField] private Color _missColor;
    [SerializeField] private string _clearText;
    [SerializeField] private Color _clearColor;

    public void ShowEvaluation(GameObject stamp,ScoreLevel promotion)
    {
        Debug.Log(promotion.ToString());
        if (promotion == ScoreLevel.Keep) return;

        GameObject newObject = Instantiate(_evaluationPrefab, stamp.transform);
        newObject.transform.rotation = Quaternion.identity;
        newObject.transform.position = stamp.transform.position + (Vector3.up * _positionY);
        if(newObject.TryGetComponent(out TextMeshProUGUI text))
        {
            text.text = promotion == ScoreLevel.Perfect ? _clearText : _missText;
            text.color = promotion == ScoreLevel.Perfect ? _clearColor : _missColor;
        }
    }
}
