using UnityEngine;
using UnityEngine.UI;

public class StampPointor : MonoBehaviour
{
    [SerializeField] private StampData _stampData;
    [SerializeField] private GameObject _stampPrefab;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private HoverDetector _stampArea;
    [SerializeField] private InkManager _inkManager;
    public bool IsCreateStamp = false;
    public StampData ClonedStamp { get; private set; }
    public void Update()
    {

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _stampData.MainRect.parent as RectTransform,
            Input.mousePosition,
            null,
            out localPoint
        );

        _stampData.MainRect.localPosition = localPoint;

        CreateStamp(localPoint);

        Vector3 rotation = _stampData.ImageRect.localEulerAngles;
        rotation.z += Input.mouseScrollDelta.y * _rotationSpeed;
        _stampData.ImageRect.localEulerAngles = rotation;
    }
    private void CreateStamp(Vector2 localPoint)
    {
        if (Input.GetMouseButtonUp(0) && _stampArea.IsHover && IsCreateStamp&& !_inkManager.IsInkEmpty())
        {
            RemoveStampObject();
            GameObject newStamp = Instantiate(_stampPrefab,GameManager.Instance._stageCreate.transform);
            if(newStamp.TryGetComponent(out StampData stampData))
            {
                stampData.MainRect.localPosition = localPoint;
                stampData.ImageRect.eulerAngles = _stampData.ImageRect.eulerAngles;
                ClonedStamp = stampData;

                stampData.ChangeAlpha(_inkManager.GetAlpha());
                
            }
            _inkManager.RemoveValue();
            GameManager.Instance.OnStamp();
        }
    }
    public void RemoveStampObject()
    {
        if (ClonedStamp == null) return;
        ClonedStamp = null;
    }
}
