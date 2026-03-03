using UnityEngine;

public class CreateStamp : MonoBehaviour
{
    public Transform _stampPointer;
    public GameObject _stampPrefab;

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
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
