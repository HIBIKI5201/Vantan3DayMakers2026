using UnityEngine;

public class StampPointor : MonoBehaviour
{
    public Transform _stampPointer;
    public GameObject _stampPrefab;
    public float _rotationSpeed;

    private float _pressureTime;
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

        Debug.Log(Input.mousePosition);
        CreateStamp(localPoint);

        Vector3 rotation = _stampPointer.localEulerAngles;
        rotation.z += Input.mouseScrollDelta.y * _rotationSpeed;
        _stampPointer.localEulerAngles = rotation;
    }

    private void CreateStamp(Vector2 localPoint)
    {
        if (Input.GetMouseButton(0))
        {
            _pressureTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            GameObject newStamp = Instantiate(_stampPrefab, _stampPointer.parent);
            _stampPointer.transform.SetAsLastSibling();
            if (newStamp.TryGetComponent(out RectTransform rectTransform))
            {
                rectTransform.localPosition = localPoint;
                rectTransform.eulerAngles = _stampPointer.eulerAngles;
            }

            Debug.Log(_pressureTime);
        }
    }
}
