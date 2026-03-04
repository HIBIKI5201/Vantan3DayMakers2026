using UnityEngine;

public class ScorePopupCreate : MonoBehaviour
{
    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private RectTransform _pointierRect;
    [SerializeField] private float _switchPointerAnlge;
    [SerializeField] private Vector2 _scorePopupOffset;

    public void Create(int score, RectTransform stampRect)
    {

        GameObject newScoreEffect = Instantiate(_scorePrefab, stampRect);
        if (newScoreEffect.TryGetComponent(out ScoreTextPopup scoreTextPopup))
        {
            scoreTextPopup.Rect.anchoredPosition = _scorePopupOffset;
            scoreTextPopup.UpdateText(score);
        }
        //データの設定など行う
    }
}
