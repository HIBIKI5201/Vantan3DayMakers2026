using UnityEngine;

public class StampPointor : MonoBehaviour
{
    [SerializeField] private Transform _stampPointer;
    [SerializeField] private GameObject _stampPrefab;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private HoverDetector _stampArea;

    public RectTransform ClonedStamp { get; private set; }
    public void Update()
    {
        RectTransform rect = _stampPointer.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            Input.mousePosition,
            null,
            out localPoint
        );

        rect.localPosition = localPoint;

        CreateStamp(localPoint);

        Vector3 rotation = _stampPointer.localEulerAngles;
        rotation.z += Input.mouseScrollDelta.y * _rotationSpeed;
        _stampPointer.localEulerAngles = rotation;
    }

    private void CreateStamp(Vector2 localPoint)
    {
        if (Input.GetMouseButtonUp(0) && _stampArea.IsHover)
        {
            RemoveStampObject();
            GameObject newStamp = Instantiate(_stampPrefab,GameManager.Instance._stageCreate.transform);
            if (newStamp.TryGetComponent(out RectTransform rectTransform))
            {
                rectTransform.localPosition = localPoint;
                rectTransform.eulerAngles = _stampPointer.eulerAngles;
                ClonedStamp = rectTransform;
            }
            GameManager.Instance.OnStamp();
        }
    }
    public void RemoveStampObject()
    {
        if (ClonedStamp == null) return;
        Destroy(ClonedStamp.gameObject);
    }
}
