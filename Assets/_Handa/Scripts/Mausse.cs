using UnityEngine;

public class Mausse : MonoBehaviour
{
    public RectTransform _stampPointer; // TransformよりRectTransformで持つのが定石
    public GameObject _stampPrefab;

    // CanvasのRenderModeに合わせてカメラを割り当てる（OverlayならnullでOK）
    private Camera uiCamera;

    void Start()
    {
        Canvas canvas = _stampPointer.GetComponentInParent<Canvas>();
        // Render ModeがOverlay以外の場合はカメラが必要
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }
    }

    public void Update()
    {
        Vector2 localPoint;
        RectTransform parentRect = _stampPointer.parent as RectTransform;

        // マウス位置を親要素内のローカル座標に変換
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            Input.mousePosition,
            uiCamera,
            out localPoint
        );

        if (isInside)
        {
            _stampPointer.localPosition = localPoint;
        }

        // クリックでスタンプ生成（プレースホルダー）
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_stampPrefab, _stampPointer.position, Quaternion.identity, parentRect);
        }
    }
}