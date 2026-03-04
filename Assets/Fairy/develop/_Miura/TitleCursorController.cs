using System;
using UnityEngine;
using UnityEngine.UI;

public enum CursorType
{
    OnEditName,
    OnGameStart,
    OnCredit,
    OnAudioSettings,
}
public class TitleCursorController : MonoBehaviour
{
    CursorType _cursorType;
    Texture2D centerCursorImage;
    [SerializeField] Sprite editNameDecoration;
    // [SerializeField] Sprite gameStartPrediction;
    [SerializeField] Sprite gameStartDecoration;
    [SerializeField] Sprite creditCursorDecoration;
    // [SerializeField] Sprite audioCursorPrediction;
    [SerializeField] Sprite audioCursorDecoration;
    [Header("GameObject")]
    [SerializeField] GameObject editNameDecorationObject;
    [SerializeField] GameObject gameStartDecorationObject;
    [SerializeField] GameObject creditCursorDecorationObject;
    [SerializeField] GameObject audioCursorDecorationObject;
    // Image _predictionImage;
    [SerializeField] Image _currentDecorationImage; //直接代入中
    
    //パラメータ関連
    [SerializeField] Vector2 editNameDecorationPos;
    [SerializeField] Vector2 gameStartDecorationPos;
    [SerializeField] Vector2 creditCursorDecorationPos;
    [SerializeField] Vector2 audioCursorDecorationPos;
    private void Awake()
    {
        centerCursorImage = CreateBlackCursor();
        Cursor.SetCursor(centerCursorImage, Vector2.zero, CursorMode.Auto); //カーソル画像を変更
        
        //テスト
        OnPointerEnterSetCursorType(CursorType.OnGameStart);
    }

    private void Update()
    {
        // _predictionImage.transform.position = Input.mousePosition;
        switch (_cursorType)
        {
            case CursorType.OnEditName:
                _currentDecorationImage.rectTransform.position = (Vector2)Input.mousePosition + editNameDecorationPos;
                break;
            case CursorType.OnGameStart:
                _currentDecorationImage.rectTransform.position = (Vector2)Input.mousePosition + gameStartDecorationPos;
                break;
            case CursorType.OnCredit:
                _currentDecorationImage.rectTransform.position = (Vector2)Input.mousePosition + creditCursorDecorationPos;
                break;
            case CursorType.OnAudioSettings:
                _currentDecorationImage.rectTransform.position = (Vector2)Input.mousePosition + audioCursorDecorationPos;
                break;
        }
    }

    public void OnPointerEnterSetCursorType(CursorType cursorType) => _cursorType = cursorType;

    private Texture2D CreateBlackCursor()
    {
        // 黒点生成
        int size = 16;
        Texture2D tex = new Texture2D(size, size);
        // 全部透明にする
        for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
            tex.SetPixel(x, y, Color.clear);
        // 中心だけ黒
        tex.SetPixel(size / 2, size / 2, Color.black);
        tex.Apply();
        return tex;
    }
}